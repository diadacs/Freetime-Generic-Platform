using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Anito.Data;


namespace Anito.Data.MongoDBClient
{
    internal class CommandExecutor : ICommandExecutor, IDisposable
    {
        private string m_connectionString = string.Empty;

        #region Constants
        private const string PARAM_IDENTIFIER = "@";
        #endregion

        public string MongoDBConnection { get; set; }
        public string MongoDBDataBase { get; set; }

        #region SetConfig
        public void SetConfiguration(Configuration.ProviderConfig config)
        {
            if (!(config.GetConfigValue(Configuration.ProviderConfig.CONNECTION_STRING) == null))
                m_connectionString = config.GetConfigValue(Configuration.ProviderConfig.CONNECTION_STRING).ToString();
        }
        #endregion

        #region NewConnection
        private Connection NewConnection()
        {
            Connection connection = new Connection();
            connection.ConnectionString = MongoDBConnection;
            connection.ChangeDatabase(MongoDBDataBase);
            return connection;
        }
       
        #endregion

        #region Transaction
        public ITransaction InitiateTransaction()
        {
            throw new NotImplementedException();
        }

        public void CommitTransaction(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void RollbackTransaction(ITransaction transaction)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ExecuteReader
        public DbDataReader ExecuteReader(ICommand command)
        {            
            try
            {
                Connection connection = NewConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                Command mongoCommand = command as Command;
                mongoCommand.Connection = connection;
                
                return mongoCommand.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }            
        }

        public DbDataReader ExecuteReader(ICommand command, ITransaction transaction)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(ICommand command)
        {
            try
            {
                Connection connection = NewConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                Command mongoCommand = command as Command;

                mongoCommand.Connection = connection;

                return mongoCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw ex;
            } 
        }

        public int ExecuteNonQuery(ICommand command, ITransaction transaction)
        {
            throw new NotImplementedException();      
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(ICommand command)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(ICommand command, ITransaction transaction)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ExecuteCount
        public int ExecuteCount(ICommand command)
        {
            throw new NotImplementedException();
        }

        public int ExecuteCount(ICommand command, ITransaction transaction)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FinalizeProcedureParameters
        public void FinalizeProcedureParameters(Procedure procedure, ICommand command)
        {
            throw new NotImplementedException();

        }
        #endregion

        #region Dispose
        public virtual void Dispose()
        {

        }
        #endregion

    }
}
