/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using System.Data;
using System.Data.Common;

namespace Anito.Data.Common
{
    public abstract class AdoCommandExecutorBase : ICommandExecutor
    {
        private string m_connectionString = string.Empty;

        #region Constants
        private const string PARAM_IDENTIFIER = "@";
        #endregion

        #region ConnectionString
        public virtual string ConnectionString
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
        #endregion

        #region NewConnection
        protected abstract IDbConnection NewConnection();      
        #endregion

        #region NewTransaction
        protected abstract ITransaction NewTransaction(IDbTransaction transaction);
        #endregion

        #region Transaction
        public virtual ITransaction InitiateTransaction()
        {

            var connection = NewConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var sqlTransaction = connection.BeginTransaction();
            var transaction = NewTransaction(sqlTransaction);
            return transaction;
            
        }

        public void CommitTransaction(ITransaction transaction)
        {
            var sqlClientTransaction = transaction as AdoTransactionBase;

            if (sqlClientTransaction == null) return; 

            try
            {
                sqlClientTransaction.SqlTransaction.Commit();
            }
            finally
            {
                if (sqlClientTransaction.SqlTransaction.Connection != null &&
                    sqlClientTransaction.SqlTransaction.Connection.State == ConnectionState.Open)
                    sqlClientTransaction.SqlTransaction.Connection.Close();
            }
        }

        public void RollbackTransaction(ITransaction transaction)
        {
            var sqlClientTransaction = transaction as AdoTransactionBase;

            if (sqlClientTransaction == null) return;

            try
            {
                sqlClientTransaction.SqlTransaction.Rollback();
            }
            finally
            {
                if (sqlClientTransaction.SqlTransaction.Connection != null &&
                    sqlClientTransaction.SqlTransaction.Connection.State == ConnectionState.Open)
                    sqlClientTransaction.SqlTransaction.Connection.Close();
            }
            
        }
        #endregion

        #region ExecuteReader
        public DbDataReader ExecuteReader(ICommand command)
        {

            var baseCommand = command as AdoCommandBase;

            if (baseCommand != null)
            {
                var sqlCommand = baseCommand.SqlCommand;
                var connection = NewConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                sqlCommand.Connection = connection;

                return sqlCommand.ExecuteReader() as DbDataReader;
            }
            return null;
        }

        public DbDataReader ExecuteReader(ICommand command, ITransaction transaction)
        {
            var commandBase = command as AdoCommandBase;
            if(commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");
            var sqlCommand = commandBase.SqlCommand;

            var transactionBase = transaction as AdoTransactionBase;
            if(transactionBase == null) throw new Exception("Unable to cast transaction as Anito.Data.Common.AdoTransactionBase");
            var sqlTransaction = (transaction as AdoTransactionBase).SqlTransaction;

            if (sqlTransaction.Connection.State != ConnectionState.Open)
                sqlTransaction.Connection.Open();

            sqlCommand.Connection = sqlTransaction.Connection;
            sqlCommand.Transaction = sqlTransaction;

            return sqlCommand.ExecuteReader() as DbDataReader;
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(ICommand command)
        {
            IDbConnection connection = null;
            try
            {
                var commandBase = command as AdoCommandBase;
                if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");

                connection = NewConnection();

                var sqlCommand = commandBase.SqlCommand;
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                sqlCommand.Connection = connection;
                return sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public int ExecuteNonQuery(ICommand command, ITransaction transaction)
        {

            var commandBase = command as AdoCommandBase;
            if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");
            var sqlCommand = commandBase.SqlCommand;

            var transactionBase = transaction as AdoTransactionBase;
            if (transactionBase == null) throw new Exception("Unable to cast transaction as Anito.Data.Common.AdoTransactionBase");
            var sqlTransaction = (transaction as AdoTransactionBase).SqlTransaction;

            if (sqlTransaction.Connection.State != ConnectionState.Open)
                sqlTransaction.Connection.Open();

            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.Connection = sqlTransaction.Connection;
            return sqlCommand.ExecuteNonQuery();
            
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(ICommand command)
        {
            IDbConnection connection = null;
            try
            {
                var commandBase = command as AdoCommandBase;
                if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");

                connection = NewConnection();

                var sqlCommand = commandBase.SqlCommand;
                
                if (connection.State != ConnectionState.Open)
                    connection.Open();
    
                sqlCommand.Connection = connection;
                return sqlCommand.ExecuteScalar();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) 
                    connection.Close();
            }
        }

        public object ExecuteScalar(ICommand command, ITransaction transaction)
        {

            var commandBase = command as AdoCommandBase;
            if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");
            var sqlCommand = commandBase.SqlCommand;

            var transactionBase = transaction as AdoTransactionBase;
            if (transactionBase == null) throw new Exception("Unable to cast transaction as Anito.Data.Common.AdoTransactionBase");
            var sqlTransaction = (transaction as AdoTransactionBase).SqlTransaction;

            if (sqlTransaction.Connection.State != ConnectionState.Open)
                sqlTransaction.Connection.Open();

            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.Connection = sqlTransaction.Connection;
            return sqlCommand.ExecuteScalar();
            
        }
        #endregion

        #region ExecuteCount
        public int ExecuteCount(ICommand command)
        {
           IDbConnection connection = null;
            try
            {
                var commandBase = command as AdoCommandBase;
                if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");

                connection = NewConnection();

                var sqlCommand = commandBase.SqlCommand;

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                sqlCommand.Connection = connection;
                return (int)sqlCommand.ExecuteScalar();
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public int ExecuteCount(ICommand command, ITransaction transaction)
        {
            var commandBase = command as AdoCommandBase;
            if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");
            var sqlCommand = commandBase.SqlCommand;

            var transactionBase = transaction as AdoTransactionBase;
            if (transactionBase == null) throw new Exception("Unable to cast transaction as Anito.Data.Common.AdoTransactionBase");
            var sqlTransaction = (transaction as AdoTransactionBase).SqlTransaction;

            if (sqlTransaction.Connection.State != ConnectionState.Open)
                sqlTransaction.Connection.Open();

            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.Connection = sqlTransaction.Connection;
            return (int)sqlCommand.ExecuteScalar();
        }
        #endregion

        #region FinalizeProcedureParameters
        public virtual void FinalizeProcedureParameters(Procedure procedure, ICommand command)
        {
            var commandBase = command as AdoCommandBase;
            if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");

            var parameters = from param in procedure.Parameters
                                                         where param.Direction == ParameterDirection.InputOutput
                                                             || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.Returned
                                                         select param;

            var sqlCommand = commandBase.SqlCommand;

            foreach (var param in parameters)
                param.Value = sqlCommand.Parameters[PARAM_IDENTIFIER + param.Name];

        }
        #endregion

        #region FinalizeCommand
        public virtual void FinalizeCommand(ICommand command)
        {
            if (command == null) return;

            var commandBase = command as AdoCommandBase;
            if (commandBase == null) throw new Exception("Unable to cast command as Anito.Data.Common.AdoCommandBase");

            var sqlCommand = commandBase.SqlCommand;
            if(sqlCommand.Connection != null && sqlCommand.Connection.State != ConnectionState.Closed)
                sqlCommand.Connection.Close();
        }
        #endregion

        #region Dispose
        public virtual void Dispose()
        {

        }
        #endregion
    }
}
