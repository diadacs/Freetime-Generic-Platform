using System;
using System.ServiceModel;

namespace Freetime.Data.Services
{
    public abstract class EndpointHandle
    {
        public abstract void AssignEndpointToHost(ServiceHost host, Type contractType);
    }
}
