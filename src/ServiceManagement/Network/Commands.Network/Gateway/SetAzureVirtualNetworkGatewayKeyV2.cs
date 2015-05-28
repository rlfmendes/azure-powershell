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

using Microsoft.WindowsAzure.Commands.ServiceManagement.Network.Gateway.Model;
using System.Management.Automation;

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.Network.Gateway
{
    [Cmdlet(VerbsCommon.Set, "AzureVirtualNetworkGatewayKey"), OutputType(typeof(SharedKeyContext))]
    public class SetAzureVirtualNetworkGatewayKeyV2 : NetworkCmdletBase
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

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The shared key that will be used on the Azure Gateway and the customer's VPN device.")]
        [ValidateNotNullOrEmpty]
        public string SharedKey
        {
            get;
            set;
        }

        public override void ExecuteCmdlet()
        {
            WriteObject(Client.SetSharedKeyV2(GatewayId, ConnectedEntityId, SharedKey));
        }       
    }
}
