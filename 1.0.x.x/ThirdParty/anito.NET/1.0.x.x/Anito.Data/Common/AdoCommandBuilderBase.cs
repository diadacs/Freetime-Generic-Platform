/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using System.Linq.Expressions;
using Anito.Data.Util;
using Anito.Data.Schema;

namespace Anito.Data.Common
{
    public abstract class AdoCommandBuilderBase : ICommandBuilder
    {
        private const string PARAM_IDENTIFIER = "@";
        private const string PARAM_IDENTIFIER_ORIGINAL = "@Original_";

        #region Properties
        #region ProviderNamespace
        public IProvider Provider
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Constructor
        protected AdoCommandBuilderBase(IProvider provider)
        {
            Provider = provider;
        }
        #endregion

        #region Methods

        #region Create

        public abstract ICommand CreateGetTCommand<T>(Expression expression);
             
        public abstract ICommand CreateGetObjectByKeyCommand<T>();

        public abstract ICommand CreateGetListCommand<T>();

        public abstract ICommand CreateGetListCommand<T>(IFilterCriteria criteria);

        public abstract ICommand CreateGetListCommand<T>(Expression expression);

        public abstract ICommand CreateGetListByPageCommand<T>();

        public abstract ICommand CreateGetListByPageCommand<T>(Expression expression);

        public abstract ICommand CreateCommandFromProcedure(Procedure procedure);

        public abstract ICommand CreateReadCommand<T>(Expression expression);

        public abstract ICommand CreateInsertCommand(object data);

        public abstract ICommand CreateUpdateCommand(object data);

        public abstract ICommand CreateUpdateCommand<T>(Expression expression);

        public abstract ICommand CreateUpdateByKeyCommand(object data);

        public abstract ICommand CreateUpdateByKeyCommand(object data, Type type);

        public abstract ICommand CreateUpdateByIdCommand(object data);

        public abstract ICommand CreateUpdateByIdCommand(object data, Type type);

        public abstract ICommand CreateDeleteCommand(object data);

        public abstract ICommand CreateDeleteCommand<T>(Expression expression);

        public abstract ICommand CreateDeleteByIdCommand(object data);

        public abstract ICommand CreateDeleteByIdCommand(object data, Type type);

        public abstract ICommand CreateDeleteByKeyCommand(object data);

        public abstract ICommand CreateDeleteByKeyCommand(object data, Type type);

        public abstract ICommand CreateCountCommand(string source);

        public abstract ICommand CreateCountCommand<T>();

        public abstract ICommand CreateCountCommand<T>(Expression expression);

        #endregion


        public virtual void SupplyGetObjectByKeyCommandParameters(ref ICommand command, params object[] keyValues)
        {
            for (var i = 0; i < command.Parameters.Count; i++)
                command.Parameters[i].Value = keyValues.ElementAt(i); 
        
        }

        public virtual void SupplyGetListByPageCommandParameters(ref ICommand command, int page, int pageSize)
        {
            command.Parameters["@Page"].Value = page;
            command.Parameters["@PageSize"].Value = pageSize;
        }

        public virtual void SupplyInsertCommandParameters(ref ICommand command, object data) 
        { 
            //TODO : Implement using dynamic method(emit) rather than reflection
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);

            foreach (var column in (from col in schemaTable where !col.IsIdentity && !col.ViewOnly select col))
            {
                if (command.Parameters.ContainsKey(PARAM_IDENTIFIER + column.Name))
                    if (column.Type == typeof(DateTime))
                        if (column.GetFieldValue(data) == null)
                            command.Parameters[PARAM_IDENTIFIER + column.Name].Value = DBNull.Value;
                        else if ((DateTime) column.GetFieldValue(data) == default(DateTime))
                            command.Parameters[PARAM_IDENTIFIER + column.Name].Value = DBNull.Value;
                        else 
                            //TODO should check max date and minimum date of db
                            command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);
                else
                    command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);
            }
        }


        public virtual void SupplyUpdateCommandParameters(ref ICommand command, IDataObject data)
        {
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);
            
            if (schemaTable.HasIdentity)
                SupplyUpdateByIdCommandParameters(ref command, data, schemaTable);
            else if (schemaTable.HasKey)
                SupplyUpdateByKeyCommandParameters(ref command, data, schemaTable);           
        }

        public virtual void SupplyUpdateCommandParameters(ref ICommand command, object data)
        {
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);

            foreach (var column in (from col in schemaTable where !col.IsIdentity && !col.ViewOnly select col))
                if (command.Parameters.ContainsKey(PARAM_IDENTIFIER + column.Name))
                    command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);
        }

        public virtual void SupplyUpdateByKeyCommandParameters(ref ICommand command, IDataObject data) 
        {
            //TODO : Implement using dynamic method(emit) rather than reflection
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);
            
            foreach (var column in (from col in schemaTable where !col.IsIdentity && !col.ViewOnly select col))
            {
                if (command.Parameters.ContainsKey(PARAM_IDENTIFIER + column.Name))
                    command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);

                if (column.IsPrimaryKey)
                    command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalKeyValue(column.Name), DBNull.Value);
            }
        }

        public virtual void SupplyUpdateByKeyCommandParameters(ref ICommand command, IDataObject data, TypeTable schemaTable)
        {
            foreach (var column in (from col in schemaTable where !col.IsIdentity && !col.ViewOnly select col))
            {
                if (command.Parameters.ContainsKey(PARAM_IDENTIFIER + column.Name))
                    command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);

                if (column.IsPrimaryKey)
                    command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalKeyValue(column.Name), DBNull.Value);
            }
        }

        public virtual void SupplyUpdateByIdCommandParameters(ref ICommand command, IDataObject data)
        {
            //TODO : Implement using dynamic method(emit) rather than reflection
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);

            foreach (var column in (from col in schemaTable where !col.ViewOnly select col))
            {
                if (command.Parameters.ContainsKey(PARAM_IDENTIFIER + column.Name) && !column.IsIdentity)
                    command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);

                if (column.IsIdentity && command.Parameters.ContainsKey(PARAM_IDENTIFIER_ORIGINAL + column.Name))
                    command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalIdValue(column.Name), DBNull.Value);
            }
        }

        public virtual void SupplyUpdateByIdCommandParameters(ref ICommand command, IDataObject data, TypeTable schemaTable)
        {
            foreach (var column in (from col in schemaTable where !col.ViewOnly select col))
            {
                if (command.Parameters.ContainsKey(PARAM_IDENTIFIER + column.Name) && !column.IsIdentity)
                    command.Parameters[PARAM_IDENTIFIER + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);

                if (column.IsIdentity && command.Parameters.ContainsKey(PARAM_IDENTIFIER_ORIGINAL + column.Name))
                    command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalIdValue(column.Name), DBNull.Value);
            }
        }

        public virtual void SupplyUpdateCommandWhereParameters(ref ICommand command, object data)
        {
            var type = data.GetType();
            var schemaTable = Provider.GetSchema(type);

            if (!schemaTable.HasIdentity) return;

            var column = schemaTable.IdentityColumn;
            if (column.IsIdentity && command.Parameters.ContainsKey(PARAM_IDENTIFIER_ORIGINAL + column.Name))
                command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);
            
        }

        public virtual void SupplyDeleteCommandParameters(ref ICommand command, IDataObject data)
        {
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);
            
            if (schemaTable.HasIdentity)
                SupplyDeleteByIdCommandParameters(ref command, data);
            else if (schemaTable.HasKey)
                SupplyDeleteByKeyCommandParameters(ref command, data);
        }

        public virtual void SupplyDeleteCommandParameters(ref ICommand commmand, object data)
        {
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);
            if (schemaTable.HasIdentity)
                foreach(var column in (from col in schemaTable where col.IsIdentity select col))
                    commmand.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);
            else if (schemaTable.HasKey)
                foreach (var column in (from col in schemaTable where col.IsPrimaryKey select col))
                    commmand.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(column.GetFieldValue(data), DBNull.Value);
        }

        public virtual void SupplyDeleteByIdCommandParameters(ref ICommand command, IDataObject data)
        {
            //TODO : Implement using dynamic method(emit) rather than reflection
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);
            foreach (var column in (from col in schemaTable where col.IsIdentity && !col.ViewOnly select col))
                command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalIdValue(column.Name), DBNull.Value);
        }

        public virtual void SupplyDeleteByIdCommandParameters(ref ICommand command, IDataObject data, TypeTable schemaTable)
        {
            //TODO : Implement using dynamic method(emit) rather than reflection
            foreach (var column in (from col in schemaTable where col.IsIdentity && !col.ViewOnly select col))
                command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalIdValue(column.Name), DBNull.Value);
        }

        public virtual void SupplyDeleteByKeyCommandParameters(ref ICommand command, IDataObject data)
        {
            //TODO : Implement using dynamic method(emit) rather than reflection
            var typeT = data.GetType();
            var schemaTable = Provider.GetSchema(typeT);
            foreach (var column in (from col in schemaTable where col.IsPrimaryKey && !col.ViewOnly select col))
                command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalKeyValue(column.Name), DBNull.Value);

        }

        public virtual void SupplyDeleteByKeyCommandParameters(ref ICommand command, IDataObject data, TypeTable schemaTable)
        {
            //TODO : Implement using dynamic method(emit) rather than reflection            
            foreach (var column in (from col in schemaTable where col.IsPrimaryKey && !col.ViewOnly select col))
                command.Parameters[PARAM_IDENTIFIER_ORIGINAL + column.Name].Value = Misc.IsNull(data.GetOriginalKeyValue(column.Name), DBNull.Value);

        }

        #region Misc
        
        protected virtual System.Data.ParameterDirection GetParameterDirection(ParameterDirection direction)
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

        protected virtual System.Data.DbType GetSqlDbType(ParameterType type)
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

        #endregion

        #endregion
    }
}
