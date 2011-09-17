/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using System.Collections.Generic;
using Anito.Data.Query;
using Anito.Data.Exceptions;
using System.Collections;
using System.Linq.Expressions;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

namespace Anito.Data
{
    public class DataSession : ISession
    {
        #region Variables

        private ITransaction CurrentTransaction { get; set; }

        #endregion

        #region Properties

        #region Provider

        public IProvider Provider { get; private set; }

        #endregion

        #region IsTransactionInitiated

        public bool IsTransactionInitiated { get; private set; }

        #endregion

        #endregion

        #region Methods

        #region Constructor
        public DataSession(IProvider provider)
        {
            Provider = provider;
        }
        #endregion

        #region Save

        /// <summary>
        /// Saves the item to the database
        /// </summary>
        /// <param name="item">The item to save</param>        
        public void Save(object item)
        {
            var transaction = IsTransactionInitiated ? CurrentTransaction
                : Provider.CommandExecutor.InitiateTransaction();

            try
            {
                Save(item, transaction);
                if (!IsTransactionInitiated) Provider.CommandExecutor.CommitTransaction(transaction);
            }
            catch
            {
                if (IsTransactionInitiated)
                    RollBackTransaction();
                else if (transaction != null)
                    Provider.CommandExecutor.RollbackTransaction(transaction);
                throw;
            } 
        }
        /// <summary>
        /// Saves the item to the database within the given transaction, if it has been modified.
        /// </summary>
        /// <param name="item">The item to save</param>
        /// <param name="transaction">The object representing the transaction</param>
        protected virtual void Save(object item, ITransaction transaction)
        {
            if (item == null)
                return;

            ICommand command = null;

            var itemStatus = Provider.GetEntityStatus(item);

            switch (itemStatus)
            {
                case EntityStatus.Insert:
                    command = Provider.CommandBuilder.CreateInsertCommand(item);
                    Provider.CommandBuilder.SupplyInsertCommandParameters(ref command, item);
                    break;
                case EntityStatus.Update:

                    command = Provider.CommandBuilder.CreateUpdateCommand(item);
                    Provider.CommandBuilder.SupplyUpdateCommandParameters(ref command, item);
                    Provider.CommandBuilder.SupplyUpdateCommandWhereParameters(ref command, item);
                    break;
            }

            if (command == null) return;

            var itemType = item.GetType();
            var reader = Provider.CommandExecutor.ExecuteReader(command, transaction);
            if (reader.Read())
            {
                item = (typeof(IDataObject).IsAssignableFrom(itemType)) ? Provider.Mapper.GetDataObjectMappingMethod(item.GetType())(reader, this) : Provider.Mapper.GetObjectMappingMethod(itemType)(reader);
            }
            else
            {
                if (typeof(IDataObject).IsAssignableFrom(itemType))
                    if ((item as IDataObject) != null) (item as IDataObject).AcceptChanges();
            }


            reader.Close();


            var schema = Provider.GetSchema(item.GetType());

            foreach (var child in (from a in schema.Associations where a.GetFieldValue(item) != null select a.GetFieldValue(item)))
            {
                if (typeof(ISingleChild).IsAssignableFrom(child.GetType()))
                {
                    var singleChild = child as ISingleChild;
                    if (singleChild != null) Save(singleChild.Child, transaction);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(child.GetType()))
                {
                    var enumerable = child as IEnumerable;
                    if (enumerable != null) foreach (var obj in enumerable) Save(obj, transaction);
                }
            } 
        }        
        #endregion

        #region Delete

        /// <summary>
        /// Delete the given data object from the database.
        /// </summary>
        /// <param name="item">The object to delete</param>
        public void Delete(object item)
        {
            var transaction = (IsTransactionInitiated) ? CurrentTransaction
                    : Provider.CommandExecutor.InitiateTransaction();
            try
            {
                Delete(item, transaction);
                if(!IsTransactionInitiated) Provider.CommandExecutor.CommitTransaction(transaction);
            }
            catch
            {
                if(IsTransactionInitiated) 
                    RollBackTransaction();
                else if(transaction != null)
                    Provider.CommandExecutor.RollbackTransaction(transaction);
                throw;
            } 
        }

        /// <summary>
        /// Delete the given data object from the database using the given transaction.
        /// </summary>
        /// <param name="item">The object to delete</param>
        /// <param name="transaction">The object representing the transaction</param>
        protected virtual void Delete (object item, ITransaction transaction )
        {
            var command = Provider.CommandBuilder.CreateDeleteCommand(item);
            Provider.CommandBuilder.SupplyDeleteCommandParameters(ref command, item);
            if (command == null) throw new Exception();
            Provider.CommandExecutor.ExecuteNonQuery(command, transaction);
        }
        #endregion

        #region BeginTransaction

        /// <summary>
        /// Starts a new transaction within this session.
        /// </summary>
        public void BeginTransaction()
        {
            if (IsTransactionInitiated) return;
            CurrentTransaction = Provider.CommandExecutor.InitiateTransaction();
            IsTransactionInitiated = true;
        }
        #endregion

        #region RollBackTransaction

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void RollBackTransaction()
        {
            if (!IsTransactionInitiated) throw new TransactionNotInstantiatedException();
            Provider.CommandExecutor.RollbackTransaction(CurrentTransaction);
            CurrentTransaction = null;
            IsTransactionInitiated = false;
        }
        #endregion

        #region CommitTransaction

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (!IsTransactionInitiated) throw new TransactionNotInstantiatedException();
            Provider.CommandExecutor.CommitTransaction(CurrentTransaction);
            CurrentTransaction = null;
            IsTransactionInitiated = false;

        }
        #endregion

        #region CancelTransaction

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void CancelTransaction()
        {
            if (!IsTransactionInitiated) return;
            Provider.CommandExecutor.RollbackTransaction(CurrentTransaction);
            CurrentTransaction = null;
            IsTransactionInitiated = false;
        }
        #endregion       

        #region GetT

        /// <summary>
        /// Fetches the first record from the table represented by TEntity that match the given expression.
        /// </summary>
        /// <typeparam name="TEntity">The class representing the table to search</typeparam>
        /// <param name="expression">A predicate indicating which records to return</param>
        /// <returns></returns>
        public TEntity GetT<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            return GetT<TEntity>((Expression)expression);
        }

        /// <summary>
        /// Fetches the first record from the table represented by TEntity that match the given expression.
        /// </summary>
        /// <typeparam name="TEntity">The class representing the table to search</typeparam>
        /// <param name="expression">A predicate indicating which records to return</param>
        /// <returns></returns>
        public TEntity GetT<TEntity> ( Expression expression )
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateGetTCommand<TEntity>(expression);

                reader = GetReader(command);
                
                var newT = default(TEntity);
                if (reader.Read())
                {   
                    newT = (typeof(IDataObject).IsAssignableFrom(typeof(TEntity))) ? Provider.Mapper.GetDataObjectMappingMethod<TEntity>()(reader, this) : Provider.Mapper.GetTMappingMethod<TEntity>()(reader);
                    LoadAssociations(ref newT);                    
                }
                return newT;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);                
            }
        }


        public TEntity GetT<TEntity>(params object[] keyValues)
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateGetObjectByKeyCommand<TEntity>();

                Provider.CommandBuilder.SupplyGetObjectByKeyCommandParameters(ref command, keyValues);

                reader = GetReader(command);

                var newT = default(TEntity);
                if (reader.Read())
                {
                    newT = (typeof(IDataObject).IsAssignableFrom(typeof(TEntity))) ? Provider.Mapper.GetDataObjectMappingMethod<TEntity>()(reader, this) : Provider.Mapper.GetTMappingMethod<TEntity>()(reader);
                    LoadAssociations(ref newT);
                }               
                return newT;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public TEntity GetT<TEntity>(Procedure procedure)
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateCommandFromProcedure(procedure);
                
                reader = GetReader(command);

                var newT = default(TEntity);
                if (reader.Read())
                {
                    newT = GetTEntity<TEntity>(reader);
                    LoadAssociations(ref newT);
                }
                return newT;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    if (command != null)
                        Provider.CommandExecutor.FinalizeProcedureParameters(procedure, command);
                }
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }
        #endregion

        #region GetList

        public TList GetList<TList, TEntity>(params object[] keyValues)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateGetObjectByKeyCommand<TEntity>();

                Provider.CommandBuilder.SupplyGetObjectByKeyCommandParameters(ref command, keyValues);

                reader = GetReader(command);

                var list = new TList();
                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader, this);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }

                }
                return list;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public TList GetList<TList, TEntity>()
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateGetListCommand<TEntity>();

                reader = GetReader(command);

                var list = new TList();

                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader, this);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }
 
                }
                return list;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public TList GetList<TList, TEntity>(Expression<Func<TEntity, bool>> expression)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            return GetList<TList, TEntity>((Expression)expression);
        }

        public TList GetList<TList, TEntity>(Expression expression)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateGetListCommand<TEntity>(expression);

                reader = GetReader(command);

                var list = new TList();

                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader, this);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }                        
                }
                return list;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public TList GetList<TList, TEntity>(Procedure procedure)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateCommandFromProcedure(procedure);

                reader = GetReader(command);

                var list = new TList();

                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader, this);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }                        
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }                        
                }
                return list;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    if (command != null)
                        Provider.CommandExecutor.FinalizeProcedureParameters(procedure, command);
                }
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }
        #endregion

        #region GetPagedList
        public TList GetPagedList<TList, TEntity>(int pageSize, int page)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            DbDataReader reader = null;
            ICommand command = null;
            try
            {

                command = Provider.CommandBuilder.CreateGetListByPageCommand<TEntity>();
                Provider.CommandBuilder.SupplyGetListByPageCommandParameters(ref command, page, pageSize);

                reader = GetReader(command);

                var list = new TList();

                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader, this);
                        LoadAssociations(ref newT);
                        list.Add(newT);                        
                    }                        
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }                        
                }
                return list;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public TList GetPagedList<TList, TEntity>(int pageSize, int page, Expression<Func<TEntity, bool>> expression)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            return GetPagedList<TList, TEntity>(pageSize, page, (Expression)expression);
        }

        public TList GetPagedList<TList, TEntity>(int pageSize, int page, Expression expression)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            DbDataReader reader = null;
            ICommand command = null;

            try
            {
                command = Provider.CommandBuilder.CreateGetListByPageCommand<TEntity>(expression);
                Provider.CommandBuilder.SupplyGetListByPageCommandParameters(ref command, page, pageSize);

                reader = GetReader(command);

                var list = new TList();

                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader, this);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }                    
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var newT = mapper(reader);
                        LoadAssociations(ref newT);
                        list.Add(newT);
                    }                    
                }
                return list;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public TList GetPagedList<TList, TEntity>(int pageSize, Page page)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            if (page == Page.First)
                return GetPagedList<TList, TEntity>(pageSize, 1);

            var rowCount = Count<TEntity>();
            if (rowCount == pageSize)
                return GetPagedList<TList, TEntity>(pageSize, 1);

            var i = rowCount / pageSize;
            var pageIndex = Math.Ceiling((double)i);

            return GetPagedList<TList, TEntity>(pageSize, (int)pageIndex + 1);
            
        }

        public TList GetPagedList<TList, TEntity>(int pageSize, Page page, Expression<Func<TEntity, bool>> expression)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            return GetPagedList<TList, TEntity>(pageSize, page, (Expression)expression);
        }

        public TList GetPagedList<TList, TEntity>(int pageSize, Page page, Expression expression)
            where TList : IList<TEntity>, IEnumerable, new()
            where TEntity : class, new()
        {
            if (page == Page.First)
                return GetPagedList<TList, TEntity>(pageSize, 1, expression);

            var rowCount = Count<TEntity>(expression);
            if (rowCount == pageSize)
                return GetPagedList<TList, TEntity>(pageSize, 1, expression);

            var i = rowCount / pageSize;
            var pageIndex = Math.Ceiling((double)i);
            
            return GetPagedList<TList, TEntity>(pageSize, (int)pageIndex + 1, expression);
            
        }
        #endregion

        #region GetEnumerable
        public IEnumerable<TEntity> GetEnumerable<TEntity>()
            where TEntity : class
        {
            DbDataReader reader = null;
            var command = Provider.CommandBuilder.CreateGetListCommand<TEntity>();
            try
            {

                reader = GetReader(command);

                if (typeof(IDataObject).IsAssignableFrom(typeof(TEntity)))
                {
                    var mapper = Provider.Mapper.GetDataObjectMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var entity = mapper(reader, this);
                        LoadAssociations(ref entity);
                        yield return entity;
                    }
                }
                else
                {
                    var mapper = Provider.Mapper.GetTMappingMethod<TEntity>();
                    while (reader.Read())
                    {
                        var entity = mapper(reader);
                        LoadAssociations(ref entity);
                        yield return entity;
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }
        #endregion

        #region Count
        public int Count<TEntity>()
        {
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateCountCommand<TEntity>();
                if (IsTransactionInitiated)
                    return Provider.CommandExecutor.ExecuteCount(command, CurrentTransaction);
                return Provider.CommandExecutor.ExecuteCount(command);
            }
            finally
            {
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public int Count<TEntity>(Expression expression)
        {
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateCountCommand<TEntity>(expression);
                return IsTransactionInitiated ? Provider.CommandExecutor.ExecuteCount(command, CurrentTransaction)
                    : Provider.CommandExecutor.ExecuteCount(command);
            }
            finally
            {
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateCountCommand<TEntity>(expression.Body);
                return IsTransactionInitiated ? Provider.CommandExecutor.ExecuteCount(command, CurrentTransaction) :
                    Provider.CommandExecutor.ExecuteCount(command);
            }
            finally
            {
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Provider = null;
            CurrentTransaction = null;
        }
        #endregion

        #endregion

        #region Insert
        public void Insert<TEntity>(ref TEntity item) where TEntity : class
        {
            ICommand command = null;
            DbDataReader reader = null;
            try
            {                
                command = Provider.CommandBuilder.CreateInsertCommand(item);
                Provider.CommandBuilder.SupplyInsertCommandParameters(ref command, item);               

                reader = (IsTransactionInitiated) ? Provider.CommandExecutor.ExecuteReader(command, CurrentTransaction) : Provider.CommandExecutor.ExecuteReader(command);
                if (reader.Read())
                {
                    item = GetTEntity<TEntity>(reader);
                    LoadAssociations(ref item);
                }               
            }
            finally
            {
                if(reader != null)
                    reader.Close();
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }
        #endregion

        #region Update
        public void Update<TEntity>(TEntity item, Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            ICommand command = null;
            try
            {                
                command = Provider.CommandBuilder.CreateUpdateCommand<TEntity>(expression);
                Provider.CommandBuilder.SupplyUpdateCommandParameters(ref command, item);
                if (IsTransactionInitiated)
                    Provider.CommandExecutor.ExecuteNonQuery(command, CurrentTransaction);
                else
                    Provider.CommandExecutor.ExecuteNonQuery(command);
            }
            finally
            {
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }
        #endregion

        #region Delete
        public void Delete<TEntity>(object item) where TEntity : class
        {
            ICommand command = null;
            try
            {
                command = Provider.CommandBuilder.CreateDeleteCommand(item);
                Provider.CommandBuilder.SupplyDeleteCommandParameters(ref command, item);

                if (IsTransactionInitiated)
                    Provider.CommandExecutor.ExecuteNonQuery(command, CurrentTransaction);
                else
                    Provider.CommandExecutor.ExecuteNonQuery(command);
            }
            finally
            {
                Provider.CommandExecutor.FinalizeCommand(command);
            }
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            ICommand command = null;
            try
            {                
                command = Provider.CommandBuilder.CreateDeleteCommand<TEntity>(expression);
                if (IsTransactionInitiated)
                    Provider.CommandExecutor.ExecuteNonQuery(command, CurrentTransaction);
                else
                    Provider.CommandExecutor.ExecuteNonQuery(command);
            }
            finally
            {
                Provider.CommandExecutor.FinalizeCommand(command);
            }

        }
        #endregion

        #region Associations and Relations

        private static Dictionary<Type, Delegate> s_createMethods;

        private static Dictionary<Type, Delegate> CreateMethodsCache
        {
            get
            {
                s_createMethods = s_createMethods ?? new Dictionary<Type, Delegate>();
                return s_createMethods;
            }
        }

        private void LoadAssociations<TEntity>(ref TEntity item)
        {
            var typeT = typeof(TEntity);
            var schema = Provider.GetSchema(typeT);
            if (schema == null) throw new Exception("Schema for the Entity not found");
                
            
            foreach (var relation in schema.Associations)
            {
                var keyValue = relation.SourceKeyColumn.GetFieldValue(item);

                var dataObjectRef = CreateDataObjectRefInstance(relation.ObjectType, this) as SessionContainer;

                if (dataObjectRef == null) continue;
                
                dataObjectRef.KeyValue = keyValue;
                relation.SetFieldValue(item, dataObjectRef);
            }
        }

        

        private static object CreateDataObjectRefInstance(Type type, ISession session)
        {
            var createMethod = GetCreateISessionContainerMethod(type);
            return createMethod(session);
        }

        private static CreateISessionContainerDelegate GetCreateISessionContainerMethod(Type type)
        {
            if (!CreateMethodsCache.ContainsKey(type))
            {
                Type[] methodArgs = { typeof(ISession) };
                var dm = new DynamicMethod("CreateISessionContainer", typeof(object), methodArgs, type);
                var il = dm.GetILGenerator();
                il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, methodArgs, null));
                il.Emit(OpCodes.Ret);
                CreateMethodsCache.Add(type, dm.CreateDelegate(typeof(CreateISessionContainerDelegate)));
            }
            return CreateMethodsCache[type] as CreateISessionContainerDelegate;
        }
        #endregion

        #region Misc
        private DbDataReader GetReader(ICommand command)
        {
            return (IsTransactionInitiated) ? Provider.CommandExecutor.ExecuteReader(command, CurrentTransaction) : Provider.CommandExecutor.ExecuteReader(command);
        }

        private TEntity GetTEntity<TEntity>(DbDataReader reader)
        {
            return (typeof(IDataObject).IsAssignableFrom(typeof(TEntity))) ? Provider.Mapper.GetDataObjectMappingMethod<TEntity>()(reader, this) :
                Provider.Mapper.GetTMappingMethod<TEntity>()(reader); 
        }
        #endregion
        
        #region Select
        public ISelect<TEntity> CreateSelect<TEntity>()
            where TEntity : class
        {
            return new Select<TEntity>(this);
        }
        #endregion

    }
}
