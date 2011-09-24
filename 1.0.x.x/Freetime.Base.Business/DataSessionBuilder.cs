using System;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public class DataSessionBuilder
    {
        private static DataSessionBuilder s_dataSessionBuilder;
        
        public static DataSessionBuilder Current
        {
            get
            {
                s_dataSessionBuilder = s_dataSessionBuilder ?? CreateInstance();
                return s_dataSessionBuilder;
            }
        }

        private static DataSessionBuilder CreateInstance()
        { 
            var builder = new DataSessionBuilder();
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
            var session = DataSessionFactory.GetDataSession<TDataSession>();
            if (Equals(session, null))
                throw new Exception(string.Format("Unable to instantiate IDataSession of type {0}", typeof(TDataSession).FullName));
            return session;
        }

        public virtual TDataSession GetDataSession<TDataSession>(TDataSession defaultSession)
            where TDataSession : IDataSession
        {
            if (Equals(defaultSession, null))
                throw new ArgumentNullException("defaultSession");
            return DataSessionFactory.GetDataSession(defaultSession);
        }
        
    }
}
