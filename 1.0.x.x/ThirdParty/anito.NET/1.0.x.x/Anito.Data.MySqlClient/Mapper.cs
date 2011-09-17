/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Anito.Data;
using System.Reflection.Emit;
using System.Reflection;
using Anito.Data.Util;


namespace Anito.Data.MySqlClient
{   
    public class Mapper: Anito.Data.IMapper
    {
        #region Variables
        private static Dictionary<Type, Delegate> m_mapperCache;      
        #endregion

        #region Properties

        #region Provider
        public IProvider Provider
        {
            get;
            set;
        }
        #endregion

        #region MapperCache
        private Dictionary<Type, Delegate> MapperCache
        {
            get
            {
                if (m_mapperCache == null)
                    m_mapperCache = new Dictionary<Type, Delegate>();
                return m_mapperCache;
            }
        }
        #endregion

        #endregion

        #region Constructor
        public Mapper(IProvider provider)
        {
            Provider = provider;
        }
        #endregion

        #region GetMappingMethods
        ToDataObjectDelegate<T> IMapper.GetDataObjectMappingMethod<T>()
        {
            return GetIDataObjectMapper<T>();
        }

        ToDataObjectDelegate IMapper.GetDataObjectMappingMethod(Type type)
        {
            return GetIDataObjectMapper(type);
        }

        ToTDelegate<T> IMapper.GetTMappingMethod<T>()
        {
            return GetObjectMapper<T>();
        }

        ToObjectDelegate IMapper.GetObjectMappingMethod(Type type)
        {
            return GetObjectMapper(type);
        }
        #endregion

        #region Mapping

        #region GetMappers
        private ToTDelegate<T> GetObjectMapper<T>()
        {
            if (!MapperCache.ContainsKey(typeof(T)))
            {
                Type[] methodArgs = { typeof(DbDataReader) };
                DynamicMethod dm = new DynamicMethod("MapDatareader", typeof(T), methodArgs, typeof(T));

                ILGenerator il = dm.GetILGenerator();
                il.DeclareLocal(typeof(T));
                il.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Stloc_0);

                CreateAssignmentMethods<T>(il);

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);
                MapperCache.Add(typeof(T), dm.CreateDelegate(typeof(ToTDelegate<T>)));
            }
            return (ToTDelegate<T>)MapperCache[typeof(T)];
        }

        private ToObjectDelegate GetObjectMapper(Type type)
        {
            if (!MapperCache.ContainsKey(type))
            {
                Type[] methodArgs = { typeof(DbDataReader) };
                DynamicMethod dm = new DynamicMethod("MapDatareader", type, methodArgs, type);

                ILGenerator il = dm.GetILGenerator();
                il.DeclareLocal(type);
                il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Stloc_0);

                CreateAssignmentMethods(type, il);

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);
                MapperCache.Add(type, dm.CreateDelegate(typeof(ToObjectDelegate)));
            }
            return (ToObjectDelegate)MapperCache[type];
        }

        private ToDataObjectDelegate<T> GetIDataObjectMapper<T>()
        {
            if (!MapperCache.ContainsKey(typeof(T)))
            {
                Type[] methodArgs = { typeof(DbDataReader), typeof(ISession) };
                DynamicMethod dm = new DynamicMethod("MapDatareader", typeof(T), methodArgs, typeof(T));

                ILGenerator il = dm.GetILGenerator();
                il.DeclareLocal(typeof(T));
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Newobj, typeof(T).GetConstructor(new Type[] { typeof(ISession) }));

                il.Emit(OpCodes.Stloc_0);
                
                CreateAssignmentMethods<T>(il);

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Call, typeof(IDataObject).GetMethod("AcceptChanges"));

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);
                MapperCache.Add(typeof(T), dm.CreateDelegate(typeof(ToDataObjectDelegate<T>)));
            }
            return (ToDataObjectDelegate<T>)MapperCache[typeof(T)];        
        }

        private ToDataObjectDelegate GetIDataObjectMapper(Type type)
        {
            if (!MapperCache.ContainsKey(type))
            {
                Type[] methodArgs = { typeof(DbDataReader), typeof(ISession) };
                DynamicMethod dm = new DynamicMethod("MapDatareader", type, methodArgs, type);

                ILGenerator il = dm.GetILGenerator();
                il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Newobj, type.GetConstructor(new Type[] { typeof(ISession) }));

                il.Emit(OpCodes.Stloc_0);

                CreateAssignmentMethods(type, il);

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Call, typeof(IDataObject).GetMethod("AcceptChanges"));

                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);
                MapperCache.Add(type, dm.CreateDelegate(typeof(ToDataObjectDelegate)));
            }
            return (ToDataObjectDelegate)MapperCache[type];
        }
        #endregion

        #region CreateAssignmentMethod
        private void CreateAssignmentMethods<T>(ILGenerator il)
        {            
            Type typeT = typeof(T);
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);
            
            foreach (Anito.Data.Schema.TypeColumn column in schemaTable)
            {
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldarg_0);

                il.Emit(OpCodes.Ldstr, column.Name);

                MethodInfo methodInfo = typeof(DbDataReader).GetMethod("get_Item", new Type[] { typeof(string) });

                il.Emit(OpCodes.Callvirt, methodInfo);

                if (column.IsNullable || column.Type == typeof(string))
                    InitMethodParam(il, column.Type, column.IsNullable);
                else if (Misc.IsNumericType(column.Type) || typeof(Guid) == column.Type || typeof(DateTime) == column.Type)
                    InitMethodParam(il, column.Type);

                if(column.StructureStructureType == Anito.Data.Schema.TypeColumn.ColumnStructureType.Field)
                    il.Emit(OpCodes.Stfld, typeof(T).GetField(column.MemberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                else if(column.StructureStructureType == Anito.Data.Schema.TypeColumn.ColumnStructureType.Property)
                    il.Emit(OpCodes.Callvirt, typeof(T).GetMethod("set_" + column.MemberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                else
                    il.Emit(OpCodes.Stfld, typeof(T).GetField(column.MemberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            }
        }

        private void CreateAssignmentMethods(Type typeT, ILGenerator il)
        {
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

            foreach (Anito.Data.Schema.TypeColumn column in schemaTable)
            {
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldarg_0);

                il.Emit(OpCodes.Ldstr, column.Name);

                MethodInfo methodInfo = typeof(DbDataReader).GetMethod("get_Item", new Type[] { typeof(string) });

                il.Emit(OpCodes.Callvirt, methodInfo);

                if (column.IsNullable || column.Type == typeof(string))
                    InitMethodParam(il, column.Type, column.IsNullable);
                else if (Misc.IsNumericType(column.Type) || typeof(Guid) == column.Type || typeof(DateTime) == column.Type)
                    InitMethodParam(il, column.Type);

                if (column.StructureStructureType == Anito.Data.Schema.TypeColumn.ColumnStructureType.Field)
                    il.Emit(OpCodes.Stfld, typeT.GetField(column.MemberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                else if (column.StructureStructureType == Anito.Data.Schema.TypeColumn.ColumnStructureType.Property)
                    il.Emit(OpCodes.Callvirt, typeT.GetMethod("set_" + column.MemberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                else
                    il.Emit(OpCodes.Stfld, typeT.GetField(column.MemberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            }
        }
        #endregion

        #region InitMethodParam
        protected virtual void InitMethodParam(ILGenerator il, Type type)
        {
            switch (type.Name.ToUpper())
            {
                case "INT16":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt16"));
                    break;
                case "INT32":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt32"));
                    break;
                case "INT64":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt64"));
                    break;
                case "SINGLE":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToSingle"));
                    break;
                case "BOOLEAN":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToBoolean"));
                    break;
                case "STRING":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToString"));
                    break;
                case "DATETIME":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDateTime"));
                    break;
                case "DECIMAL":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDecimal"));
                    break;
                case "DOUBLE":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDouble"));
                    break;
                case "GUID":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToGuid"));
                    break;
                case "BYTE[]":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToBytes"));
                    break;
                case "BYTE":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToByte"));
                    break;

            }
        }

        protected virtual void InitMethodParam(ILGenerator il, Type type, bool isNullable)
        {

            switch (type.Name.ToUpper())
            {
                case "INT16":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt16", isNullable));
                    break;
                case "INT32":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt32", isNullable));
                    break;
                case "INT64":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToInt64", isNullable));
                    break;
                case "SINGLE":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToSingle", isNullable));
                    break;
                case "BOOLEAN":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToBoolean", isNullable));
                    break;
                case "STRING":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToString", isNullable));
                    break;
                case "DATETIME":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDateTime", isNullable));
                    break;
                case "DECIMAL":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDecimal", isNullable));
                    break;
                case "DOUBLE":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToDouble", isNullable));
                    break;
                case "GUID":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToGuid", isNullable));
                    break;
                case "BYTE[]":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToBytes", isNullable));
                    break;
                case "BYTE":
                    il.Emit(OpCodes.Call, CreateConverterMethodInfo("ToByte", isNullable));
                    break;
            }
        }
        #endregion

        #region CreateConverterMethodInfo
        protected virtual MethodInfo CreateConverterMethodInfo(string method)
        {
            return CreateConverterMethodInfo(method, new Type[] { typeof(object) });
        }

        protected virtual MethodInfo CreateConverterMethodInfo(string method, Type[] types)
        {
            return typeof(Convert).GetMethod(method, types);
        }

        protected virtual MethodInfo CreateConverterMethodInfo(string method, bool isNullable)
        {
            if (isNullable)
                return typeof(DbNullConverter).GetMethod(method, new Type[] { typeof(object) });
            return typeof(Converter).GetMethod(method, new Type[] { typeof(object) });
        }
        #endregion

        #endregion
    }
}
