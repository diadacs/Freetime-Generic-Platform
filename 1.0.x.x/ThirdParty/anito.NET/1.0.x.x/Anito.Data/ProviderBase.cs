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
using Anito.Data;
using Anito.Data.Schema;

namespace Anito.Data
{
    public abstract class ProviderBase : IProvider
    {
        

        public virtual string ProviderName { get; private set; }

        public virtual string ConnectionString { get; set; }

        public abstract ICommandBuilder CommandBuilder { get; }

        public abstract IMapper Mapper { get; }

        public abstract ICommandExecutor CommandExecutor { get; }

        public ProviderBase(string name)
        {
            ProviderName = name;
        }

        internal void SetProviderName(string name)
        {
            ProviderName = name;
        }

        public virtual TypeTable GetSchema(Type type)
        {
            if (CacheManager.SchemaCache.ContainsKey(ProviderName))
                if (CacheManager.SchemaCache[ProviderName].ContainsKey(type))
                    return CacheManager.SchemaCache[ProviderName][type];
            return TypeTable.GetTypeTableSchema(type);
        }

        public virtual EntityStatus GetEntityStatus(object item)
        {
            TypeTable schema = GetSchema(item.GetType());

            if (schema.HasIdentity)
            {
                //Get the first Identity Column
                TypeColumn column = schema.IdentityColumn;
                var value = column.GetFieldValue(item);

                if (Decimal.Zero.Equals(value))
                    return EntityStatus.Insert;
                return EntityStatus.Update;
            }
            return EntityStatus.Unchanged;
        }

    }
}
