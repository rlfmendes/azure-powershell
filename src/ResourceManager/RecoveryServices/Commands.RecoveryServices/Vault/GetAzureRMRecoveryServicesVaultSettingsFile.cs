﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Portal.RecoveryServices.Models.Common;

namespace Microsoft.Azure.Commands.RecoveryServices
{
    /// <summary>
    /// Retrieves Azure Recovery Services Vault Settings File.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureRmRecoveryServicesVaultSettingsFile")]
    [OutputType(typeof(VaultSettingsFilePath))]
    public class GetAzureRmRecoveryServicesVaultSettingsFile : RecoveryServicesCmdletBase
    {
        /// <summary>
        /// Expiry in hours for generated certificate.
        /// </summary>
        private const int VaultCertificateExpiryInHoursForHRM = 120; 

        #region Parameters

        /// <summary>
        /// Gets or sets vault Object.
        /// </summary>
        [Parameter(ParameterSetName = ARSParameterSets.ByDefault, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ARSParameterSets.ForSite, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public ARSVault Vault { get; set; }

        /// <summary>
        /// Gets or sets Site Identifier.
        /// </summary>
        [Parameter(ParameterSetName = ARSParameterSets.ForSite, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public String SiteIdentifier { get; set; }

        /// <summary>
        /// Gets or sets SiteFriendlyName.
        /// </summary>
        [Parameter(ParameterSetName = ARSParameterSets.ForSite, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public String SiteFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the path where the credential file is to be generated
        /// </summary>
        /// <summary>
        /// Gets or sets vault Object.
        /// </summary>
        [Parameter(ParameterSetName = ARSParameterSets.ByDefault)]
        [Parameter(ParameterSetName = ARSParameterSets.ForSite)]
        public string Path { get; set; }

        #endregion Parameters

        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            try
            {
                this.GetVaultSettingsFile();
            }
            catch (AggregateException aggregateEx)
            {
                // if an exception is thrown from a task, it will be wrapped in AggregateException 
                // and propagated to the main thread. Just throwing the first exception in the list.
                Exception exception = aggregateEx.InnerExceptions.First<Exception>();
                this.HandleException(exception);
            }
        }

        /// <summary>
        /// Method to execute the command
        /// </summary>
        private void GetVaultSettingsFile()
        {
            AzureSubscription subscription = DefaultProfile.Context.Subscription;

            // Generate certificate
            X509Certificate2 cert = CertUtils.CreateSelfSignedCertificate(VaultCertificateExpiryInHoursForHRM, subscription.Id.ToString(), this.Vault.Name);

            ASRSite site = new ASRSite();

            if (!string.IsNullOrEmpty(this.SiteIdentifier) && !string.IsNullOrEmpty(this.SiteFriendlyName))
            {
                site.ID = this.SiteIdentifier; 
                site.Name = this.SiteFriendlyName;
            }

            // Generate file.
            ARSVaultCreds vaultCreds = RecoveryServicesClient.GenerateVaultCredential(
                                            cert,
                                            this.Vault,
                                            site);

            string filePath = string.IsNullOrEmpty(this.Path) ? Utilities.GetDefaultPath() : this.Path;
            string fileName = this.GenerateFileName();

            // write the content to a file.
            VaultSettingsFilePath output = new VaultSettingsFilePath()
            {
                FilePath = Utilities.WriteToFile<ARSVaultCreds>(vaultCreds, filePath, fileName)
            };

            // print the path to the user.
            this.WriteObject(output, true);
        }

        /// <summary>
        /// Method to generate the file name
        /// </summary>
        /// <returns>file name as string.</returns>
        private string GenerateFileName()
        {
            string fileName;
            string format = "yyyy-MM-ddTHH-mm-ss";

            if (string.IsNullOrEmpty(this.SiteIdentifier) || string.IsNullOrEmpty(this.SiteFriendlyName))
            {
                fileName = string.Format("{0}_{1}.VaultCredentials", this.Vault.Name, DateTime.UtcNow.ToString(format));
            }
            else
            {
                fileName = string.Format("{0}_{1}_{2}.VaultCredentials", this.SiteFriendlyName, this.Vault.Name, DateTime.UtcNow.ToString(format));
            }

            return fileName;
        }
    }
}
