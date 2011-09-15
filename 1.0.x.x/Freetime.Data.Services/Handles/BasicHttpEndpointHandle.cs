using System;
using System.ServiceModel;

namespace Freetime.Data.Services.Handles
{
    public class BasicHttpEndpointHandle : EndpointHandle
    {
        public string Address { get; set; }

        public BasicHttpSecurityMode SecurityMode { get; set; }

        public BasicHttpEndpointHandle()
        {
            SecurityMode = BasicHttpSecurityMode.None;
        }

        public override void AssignEndpointToHost(ServiceHost host, Type contractType)
        {
            var binding = new BasicHttpBinding(SecurityMode);
            var fixedAddress = Address.EndsWith("/") ? Address : string.Format("{0}/",Address);
            
            host.AddServiceEndpoint(contractType, binding, string.Format("{0}{1}", fixedAddress, contractType.Name));
        }
    }
}
