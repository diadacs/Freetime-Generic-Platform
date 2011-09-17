/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Serialization
{
    public class SchemaSerializer
    {
        public Type EntityType { get; set; }

        public SchemaSerializer()
        {         
        }

        public SchemaSerializer(Type type)
        {
            EntityType = type;
        }

        public void Serialize(string fileName)
        { 
        
        }
    }
}
