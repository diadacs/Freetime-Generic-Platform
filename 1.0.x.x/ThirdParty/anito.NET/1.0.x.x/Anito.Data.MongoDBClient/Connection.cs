using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Anito.Data.MongoDBClient
{
    internal class Connection : DbConnection
    {
        
        #region Variables
        private string m_connectionString = null;
        private string m_database = null;

        private MongoServer m_mongoServer = null;
        private MongoDatabase m_mongoDatabase = null;


        #endregion

        private MongoServer Server
        {
            get
            {
                return m_mongoServer;
            }
        }

        public MongoDatabase MongoDatabase
        {
            get
            {
                if (m_mongoDatabase == null || m_mongoDatabase.Name != Database)
                    m_mongoDatabase = Server.GetDatabase(Database);                
                return m_mongoDatabase;
            }
        }

        #region Properties

        public override string DataSource
        {
            get { throw new NotImplementedException(); }
        }

        public override int ConnectionTimeout
        {
            get { throw new NotImplementedException(); }
        }

        public override string Database
        {
            get
            {
                return m_database;
            }
        }

        public bool UseCompression
        {
            get { throw new NotImplementedException(); }
        }


        public override ConnectionState State
        {
            get 
            {
                if (Server == null)
                    return ConnectionState.Closed;
                else
                {
                    switch (Server.State)
                    { 
                        case MongoServerState.Connected:
                            return ConnectionState.Open;
                        case MongoServerState.Connecting:
                            return ConnectionState.Connecting;
                        case MongoServerState.Disconnected:
                            return ConnectionState.Closed;
                        default:
                            return ConnectionState.Closed;
                    }
                }
            }
        }

        public override string ServerVersion
        {
            get 
            { 
                throw new NotImplementedException(); 
            }
        }

        public override string ConnectionString
        {
            get
            {
                return m_connectionString;
            }
            set
            {
                m_connectionString = value;
            }
        }


        protected override DbProviderFactory DbProviderFactory
        {
            get { throw new NotImplementedException(); }
        }
		


        #endregion


        #region Methods
        public override void ChangeDatabase(string databaseName)
        {
            m_database = databaseName;
        }
        
        public override void Open()
        {
            m_mongoServer = MongoServer.Create(ConnectionString);
            Server.Connect();
		}

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            if (isolationLevel == IsolationLevel.Unspecified)
                return BeginTransaction();
            return BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return CreateCommand();
        }

        
        public override void Close()
        {
            if(Server != null && (Server.State == MongoServerState.Connected || Server.State == MongoServerState.Connecting))
                Server.Disconnect();
        }       

        public void CancelQuery(int timeout)
        {
            throw new NotImplementedException();
        }

        #region GetSchema Support

        public override DataTable GetSchema()
        {
            return GetSchema(null);
        }

        public override DataTable GetSchema(string collectionName)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposeable

        protected override void Dispose(bool disposing)
        {
            if (State == ConnectionState.Open)
                Close();
            base.Dispose(disposing);
        }

        #endregion

        #endregion


    }
}
