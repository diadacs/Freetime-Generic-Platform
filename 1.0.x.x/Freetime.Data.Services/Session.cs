using System;
using System.ServiceModel;

namespace Freetime.Data.Services
{   

    public class Session
    {
        public enum SessionStatus
        { 
            Open,
            Closed
        }

        private ServiceHost Host { get; set; }

        public Type Contract { get; private set; }

        public string ContractName { get; private set; }

        public Type Service { get; private set; }

        public SessionStatus Status { get; private set; }

        public Session(ServiceHost host, Type contract, Type service)
        {
            Host = host;
            Contract = contract;
            ContractName = Contract.Name;
            Service = service;
            Status = SessionStatus.Closed;
        }

        public virtual void Open()
        {
            Host.Open();
            Status = SessionStatus.Open;
        }

        public virtual void Close()
        {
            Host.Close();
            Status = SessionStatus.Closed;
        }

        public override int GetHashCode()
        {
            return (Contract != null && Contract.FullName != null) ? Contract.FullName.GetHashCode()
                : 0;
        }
    }
}
