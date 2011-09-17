using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Configuration;

namespace Anito.Data.MongoDBClient
{
    public class MongoDBProvider : IProvider
    {
        private const string MONGODB_CLIENT = "MongoDBClient";
        private Mapper m_mapper = null;
        private CommandExecutor m_executor = null;
        private CommandBuilder m_builder = null;

        public string ConnectionString
        {
            get
            {
                return MongoDBConnection;
            }
            set
            {
                MongoDBConnection = value;
            }
        }

        public string MongoDBConnection
        {
            get
            {
                return m_executor.MongoDBConnection;
            }
            set
            {
                m_executor.MongoDBConnection = value;
            }
        }

        public string MongoDBDataBase 
        {
            get
            {
                return m_executor.MongoDBDataBase;
            }
            set
            {
                m_executor.MongoDBDataBase = value;    
            }
        }

        public string ProviderName
        {
            get
            {
                return MONGODB_CLIENT;
            }
        }

        ICommandExecutor IProvider.CommandExecutor
        {
            get
            {
                return m_executor;
            }
        }

        IMapper IProvider.Mapper
        {
            get
            {
                return m_mapper;
            }
        }

        ICommandBuilder IProvider.CommandBuilder
        {
            get
            {
                return m_builder;
            }
        }

        public void SetConfiguration(ProviderConfig config)
        {
            MongoDBConnection = config.GetConfigValue("MongoDBConnection").ToString();
            MongoDBDataBase = config.GetConfigValue("MongoDBDataBase").ToString();
        }

        public MongoDBProvider()
        {
            m_mapper = new Mapper();
            m_executor = new CommandExecutor();
            m_builder = new CommandBuilder();
        }
    }
}
