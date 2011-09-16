using System.ServiceModel;

namespace Freetime.Base.Business
{
    internal class DataSessionFactory : IDataSessionFactory
    {
        private bool UseDefaultDataSession { get; set; }

        internal DataSessionFactory()
        {
            UseDefaultDataSession = true;
        }

        TContract IDataSessionFactory.GetDataSession<TContract>() 
        {
            var httpFactory =
                new ChannelFactory<TContract>(
                  new BasicHttpBinding(),
                  new EndpointAddress(
                    string.Format("http://192.168.175.190:8000/FreetimeDataServices/{0}", typeof(TContract).Name)));
            var contract = httpFactory.CreateChannel();
            return contract;
        }
        
        TContract IDataSessionFactory.GetDataSession<TContract>(TContract defaultContract) 
        {
            if (UseDefaultDataSession) return defaultContract;

            var contract = (this as IDataSessionFactory).GetDataSession<TContract>();
            return Equals(contract, null) ? defaultContract : contract;
        }
    }
}
