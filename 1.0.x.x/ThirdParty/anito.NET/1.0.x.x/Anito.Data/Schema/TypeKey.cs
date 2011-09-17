/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;

namespace Anito.Data.Schema
{
    public class TypeKey
    {
        public string FieldName { get; private set; }

        public Type FieldType { get; private set; }

        public TypeKey(string fieldName, Type fieldType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }

        public static TypeKey GetTypeKey(Type type)
        {
            var table = new TypeTable(type);

            var key = table.First(col => col.IsPrimaryKey);
           
            return (key != null ) ? new TypeKey(key.Name, key.Type)
                : null;
        }
    }
}
