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

using System.Management.Automation;
using Microsoft.WindowsAzure.Management.Network.Models;
using Microsoft.WindowsAzure.Commands.ServiceManagement.Network.Gateway.Model;

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.Network.Gateway
{
    [Cmdlet(VerbsCommon.Set, "AzureVirtualNetworkGatewayIPsecParameters"), OutputType(typeof(GatewayGetOperationStatusResponse))]
    public class SetAzureVirtualNetworkGatewayIPsecParametersV2 : NetworkCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The virtual network gateway id.")]
        [ValidateGuid]
        [ValidateNotNullOrEmpty]
        public string GatewayId
        {
            get;
            set;
        }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The virtual network gateway connected entityId.")]
        [ValidateGuid]
        [ValidateNotNullOrEmpty]
        public string ConnectedEntityId
        {
            get;
            set;
        }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The type of encryption that will " +
            "be used for the connection between the virtual network gateway and the local network. " +
            "Valid values are RequireEncryption/NoEncryption/AES256/AES128/DES3.")]
        public string EncryptionType
        {
            get;
            set;
        }

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The PFS gruop that will be used " +
            "for the connection between the virtual network gateway and the local network. Valid " +
            "values are PFS1 and None.")]
        public string PfsGroup
        {
            get;
            set;
        }

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The SA Data Size Kilobytes value " +
            "is used to determine how many kilobytes of traffic can be sent before the SA for the" +
            "connection will be renegotiated.")]
        public int SADataSizeKilobytes
        {
            get;
            set;
        }

        [Parameter(Position = 5, Mandatory = false, HelpMessage = "The SA Lifetime Seconds value " +
            "is used to determine how long (in seconds) this connection's SA will be valid before " +
            "a new SA will be negotiated.")]
        public int SALifetimeSeconds
        {
            get;
            set;
        }

        public override void ExecuteCmdlet()
        {
            WriteObject(Client.SetIPsecParametersV2(GatewayId, ConnectedEntityId, EncryptionType, PfsGroup, SADataSizeKilobytes, SALifetimeSeconds));
        }
    }
}
