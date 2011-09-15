using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public class DataSessionBuilder
    {
        private static DataSessionBuilder __INSTANCE;
        
        public static DataSessionBuilder Current
        {
            get
            {
                __INSTANCE = __INSTANCE ?? CreateInstance();
                return __INSTANCE;
            }
        }

        private static DataSessionBuilder CreateInstance()
        { 
            DataSessionBuilder builder = new DataSessionBuilder();
            IDataSessionFactory factory = new DataSessionFactory();
            builder.SetDataSessionFactory(factory);
            return builder;
        }

        private IDataSessionFactory DataSessionFactory { get; set; }

        public void SetDataSessionFactory(IDataSessionFactory dataSessionFactory)
        {
            DataSessionFactory = dataSessionFactory;
        }

        public virtual TDataSession GetDataSession<TDataSession>()
            where TDataSession : IDataSession
        {
            TDataSession session = DataSessionFactory.GetDataSession<TDataSession>();
            if (session == null)
                throw new Exception(string.Format("Unable to instantiate IDataSession of type {0}", typeof(TDataSession).FullName));
            return session;
        }

        public virtual TDataSession GetDataSession<TDataSession>(TDataSession defaultSession)
            where TDataSession : IDataSession
        {
            if (defaultSession == null)
                throw new Exception("Parameter defaultSession can't be null");
            return DataSessionFactory.GetDataSession<TDataSession>(defaultSession);
        }
        
    }
}
