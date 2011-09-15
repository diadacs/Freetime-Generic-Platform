using System;
using System.Collections.Generic;
using System.ServiceModel;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;

namespace Freetime.Data.Services
{
    public class Service : IDisposable
    {
        private DataSessionServiceList m_serviceList;
        private SessionCollection m_serviceHosts;
        private List<EndpointHandle> m_handles;

        protected virtual DataSessionServiceList DataSessionServices
        {
            get
            {
                m_serviceList = m_serviceList ?? PluginManagement.PluginManager.Current.GetDataSessionServices();
                return m_serviceList;
            }
        }

        public virtual SessionCollection ServiceHosts
        {
            get
            {
                m_serviceHosts = m_serviceHosts ?? new SessionCollection();
                return m_serviceHosts;
            }
        }

        protected virtual List<EndpointHandle> EndpointHandles
        {
            get
            {
                m_handles = m_handles ?? new List<EndpointHandle>();
                return m_handles;
            }
        }

        public virtual void Load()
        {
            foreach (DataSessionService service in DataSessionServices)
            {
                Type serviceType = Type.GetType(string.Format("{0}, {1}", service.Type, service.Assembbly));

                if (serviceType == null)
                    throw new Exception("Unrecognizable Service Type");


                Type contractType = Type.GetType(string.Format("{0}, {1}", service.Contract, service.ContractAssembly));

                if (contractType == null)
                    throw new Exception("Unrecognizable Service Contract Type");

                if (!contractType.IsAssignableFrom(serviceType))
                    throw new Exception(string.Format("Service Contract {0} does not match Service Type {1}", contractType.FullName, serviceType.FullName));


                var host = new ServiceHost(serviceType);

                foreach (EndpointHandle handle in EndpointHandles)
                    handle.AssignEndpointToHost(host, contractType);
                
                var session = new Session(host, contractType, serviceType);

                if (ServiceHosts.Contains(session))
                    throw new Exception(string.Format("There is already a contract of type {0} hosted", contractType.FullName));

                ServiceHosts.Add(session);
            }
        }

        public virtual void Start()
        {
            foreach (Session session in ServiceHosts)
                session.Open();
        }        

        public virtual void Stop()
        {
            foreach (Session session in ServiceHosts)
                session.Close();
        }

        public virtual void ReStart()
        {
            Stop();
            Start();
        }

        public void Dispose()
        {
            m_serviceHosts = null;
            m_serviceList = null;
        }

        public virtual void AddEndpointHandle(EndpointHandle handle)
        {
            EndpointHandles.Add(handle);
        }

        public virtual void ClearEndpointHandles()
        {
            EndpointHandles.Clear();
        }

        public virtual void RemoveEndpointHandle(EndpointHandle handle)
        {            
            EndpointHandles.Remove(handle);
        }
    }
}

