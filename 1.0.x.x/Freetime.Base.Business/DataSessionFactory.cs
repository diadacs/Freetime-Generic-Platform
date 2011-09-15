using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Freetime.Base.Business
{
    internal class DataSessionFactory : IDataSessionFactory
    {
        TContract IDataSessionFactory.GetDataSession<TContract>() 
        {
            return default(TContract);
        }
        
        TContract IDataSessionFactory.GetDataSession<TContract>(TContract defaultContract) 
        {

            ChannelFactory<TContract> httpFactory =
                new ChannelFactory<TContract>(
                  new BasicHttpBinding(),
                  new EndpointAddress(
                    string.Format("http://192.168.175.190:8000/FreetimeDataServices/{0}", typeof(TContract).Name)));

            TContract contract = httpFactory.CreateChannel();

            if(contract == null)
                return defaultContract;
            return contract;
        }
    }
}
