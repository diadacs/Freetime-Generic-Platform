using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Schema;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Anito.Data.MongoDBClient
{
    internal class CommandBuilder : ICommandBuilder
    {             

        

        #region Methods

        #region CreateCommands

        #region CreateGetTCommand
        public ICommand CreateGetTCommand<T>(Expression expression)
        {
            Type typeT = typeof(T);
            TypeTable schemaTable = TypeTable.GetTypeTableSchema(typeT);
            Command command = new Command();
            command.CollectionName = schemaTable.ViewSource;
            
            return command;
           
        }
        #endregion

        #region CreateGetObjectByKeyCommand
        public ICommand CreateGetObjectByKeyCommand<T>()
        {
           throw new NotImplementedException();
        }
        #endregion

        #region CreateGetListCommand
        public ICommand CreateGetListCommand<T>()
        {
            Type typeT = typeof(T);
            TypeTable schemaTable = TypeTable.GetTypeTableSchema(typeT);
            Command command = new Command();
            command.CollectionName = schemaTable.ViewSource;

            return command;
        }

        public ICommand CreateGetListCommand<T>(IFilterCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateGetListCommand<T>(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateGetListByPageCommand
        public ICommand CreateGetListByPageCommand<T>()
        {
            throw new NotImplementedException();
        }

        public ICommand CreateGetListByPageCommand<T>(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateCommandFromProcedure
        public ICommand CreateCommandFromProcedure(Procedure procedure)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Read
        public ICommand CreateReadCommand<T>(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Insert

        #region CreateInsertCommand
        public ICommand CreateInsertCommand(object data)
        {
            Type typeT = data.GetType();
            Command command = new Command();
            BsonDocument document = new BsonDocument();
            Query query = new Query { 
                Statement = StatementType.INSERT,
                Document = document
            };
            command.Queries.Add(query);
            
            Anito.Data.Schema.TypeTable schemaTable = TypeTable.GetTypeTableSchema(typeT);
            command.CollectionName = schemaTable.ViewSource;
            foreach (Schema.TypeColumn column in (from col in schemaTable where !col.IsIdentity && !col.ViewOnly select col))
            {
                command.AddParam(column.Name);
            }            
            return command;
        }
        #endregion

        #endregion

        #region Update

        #region CreateUpdateCommand
        public ICommand CreateUpdateCommand(object data)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateUpdateCommand<T>(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateUpdateByKeyCommand
        public ICommand CreateUpdateByKeyCommand(object data)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateUpdateByKeyCommand(object data, Type type)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CreateUpdateByIdCommand
        public ICommand CreateUpdateByIdCommand(object data)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateUpdateByIdCommand(object data, Type type)
        {
           throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Delete

        #region CreateDeleteCommand
        public ICommand CreateDeleteCommand(object data)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateDeleteCommand<T>(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateDeleteByIdCommand
        public ICommand CreateDeleteByIdCommand(object data)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateDeleteByIdCommand(object data, Type type)
        {
            throw new NotImplementedException();
        }

        
        #endregion

        #region CreateDeleteByKeyCommand
        public ICommand CreateDeleteByKeyCommand(object data)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateDeleteByKeyCommand(object data, Type type)
        {
            throw new NotImplementedException();
        }
     
        #endregion

        #endregion

        #region Count
        public ICommand CreateCountCommand(string source)
        {
            throw new NotImplementedException();
        }

        public ICommand CreateCountCommand<T>()
        {
            throw new NotImplementedException();
        }

        public ICommand CreateCountCommand<T>(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region SupplyParameters

        #region SupplyGetObjectByKeyCommandParameters
        public void SupplyGetObjectByKeyCommandParameters(ref ICommand command, params object[] keyValues)
        {
            for (int i = 0; i < command.Parameters.Count; i++)
                command.Parameters[i].Value = keyValues.ElementAt(i);

        }
        #endregion

        #region SupplyGetListByPageCommandParameters
        public void SupplyGetListByPageCommandParameters(ref ICommand command, int page, int pageSize)
        {
            command.Parameters["@Page"].Value = page;
            command.Parameters["@PageSize"].Value = pageSize;
        }
        #endregion

        #region Insert

        #region SupplyInsertCommandParameters
        public void SupplyInsertCommandParameters(ref ICommand command, object data)
        {
            Command mongoCommand = command as Command;
            
            Type typeT = data.GetType();
            Anito.Data.Schema.TypeTable schemaTable = TypeTable.GetTypeTableSchema(typeT);

            foreach (Schema.TypeColumn column in (from col in schemaTable where !col.IsIdentity && !col.ViewOnly select col))
            {
                mongoCommand.Parameters[column.Name].Value = column.GetFieldValue(data);
            }
        }
        #endregion

        #endregion

        #region Update

        #region SupplyUpdateCommandParameters
        public void SupplyUpdateCommandParameters(ref ICommand command, IDataObject data)
        {
            throw new NotImplementedException();
        }

        public void SupplyUpdateCommandParameters(ref ICommand command, object data)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SupplyUpdateByKeyCommandParameters
        public void SupplyUpdateByKeyCommandParameters(ref ICommand command, IDataObject data)
        {
         
            throw new NotImplementedException();
        }

        public void SupplyUpdateByKeyCommandParameters(ref ICommand command, IDataObject data, Anito.Data.Schema.TypeTable schemaTable)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SupplyUpdateByIdCommandParameters
        public void SupplyUpdateByIdCommandParameters(ref ICommand command, IDataObject data)
        {
           throw new NotImplementedException();
        }

        public void SupplyUpdateByIdCommandParameters(ref ICommand command, IDataObject data, Anito.Data.Schema.TypeTable schemaTable)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Delete

        #region  SupplyDeleteCommandParameters
        public void SupplyDeleteCommandParameters(ref ICommand command, IDataObject data)
        {
            throw new NotImplementedException();
        }

        public void SupplyDeleteCommandParameters(ref ICommand commmand, object data)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SupplyDeleteByIdCommandParameters
        public void SupplyDeleteByIdCommandParameters(ref ICommand command, IDataObject data)
        {
            throw new NotImplementedException();
        }

        public void SupplyDeleteByIdCommandParameters(ref ICommand command, IDataObject data, Anito.Data.Schema.TypeTable schemaTable)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SupplyDeleteByKeyCommandParameters
        public void SupplyDeleteByKeyCommandParameters(ref ICommand command, IDataObject data)
        {
           throw new NotImplementedException();
        }

        public void SupplyDeleteByKeyCommandParameters(ref ICommand command, IDataObject data, Anito.Data.Schema.TypeTable schemaTable)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #endregion

        #region Misc
        private System.Data.ParameterDirection GetParameterDirection(ParameterDirection direction)
        {
            switch (direction)
            {
                case ParameterDirection.Input:
                    return System.Data.ParameterDirection.Input;
                case ParameterDirection.Output:
                    return System.Data.ParameterDirection.Output;
                case ParameterDirection.InputOutput:
                    return System.Data.ParameterDirection.InputOutput;
                case ParameterDirection.Returned:
                    return System.Data.ParameterDirection.ReturnValue;
                default:
                    return System.Data.ParameterDirection.Input;
            }
        }

        private System.Data.DbType GetSqlDbType(ParameterType type)
        {
            switch (type)
            {
                case ParameterType.Int32:
                    return System.Data.DbType.Int32;
                case ParameterType.String:
                    return System.Data.DbType.String;
                case ParameterType.Single:
                    return System.Data.DbType.Single;
                default:
                    return System.Data.DbType.String;
            }
        }

        //private string SelectColumnsStatement(Type type)
        //{
        //    if (!ColumnSelectCache.ContainsKey(type))
        //    {
        //        Anito.Data.Schema.TypeTable schemaTable = TypeTable.GetTypeTableSchema(type);
        //        StringBuilder columnSb = new StringBuilder();
        //        foreach (TypeColumn column in schemaTable)
        //        {
        //            if (columnSb.Length > 0) columnSb.Append(", ");

        //            if (column.IsIdentity)
        //            {
        //                columnSb.Append(string.Format("{0}.{1} AS {2}", schemaTable.ViewSource, column.Name, column.Name));
        //            }
        //            else
        //            {
        //                if (!column.IsNullable)
        //                {
        //                    switch (column.Type.Name.ToUpper())
        //                    {
        //                        case "INT16":
        //                        case "INT32":
        //                        case "INT64":
        //                        case "SINGLE":
        //                        case "DATETIME":
        //                        case "DECIMAL":
        //                        case "DOUBLE":
        //                        case "BOOLEAN":
        //                            columnSb.Append(string.Format("ISNULL({0}.{1},{2}) AS {3}", schemaTable.ViewSource, column.Name, 0, column.Name));
        //                            break;
        //                        case "STRING":
        //                        case "BYTE[]":
        //                        case "BYTE":
        //                            columnSb.Append(string.Format("{0}.{1} AS {2}", schemaTable.ViewSource, column.Name, column.Name));
        //                            break;
        //                        case "GUID":
        //                            columnSb.Append(string.Format("ISNULL({0}.{1},'{2}') AS {3}", schemaTable.ViewSource, column.Name, default(Guid), column.Name));
        //                            break;
        //                    }
        //                }
        //                else
        //                    columnSb.Append(string.Format("{0}.{1} AS {2}", schemaTable.ViewSource, column.Name, column.Name));

        //            }
        //        }
        //        ColumnSelectCache.Add(type, columnSb.ToString());
        //    }
        //    return ColumnSelectCache[type];
        //}


        #endregion

        #endregion
    }
}
