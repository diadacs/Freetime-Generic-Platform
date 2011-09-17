/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Collections.Generic;

namespace Anito.Data.Schema
{
    internal static class CacheManager
    {
        private static Dictionary<string, TypeSchemaSource> s_schemaCache;

        public static Dictionary<string, TypeSchemaSource> SchemaCache
        {
            get
            {
                s_schemaCache = s_schemaCache ?? new Dictionary<string, TypeSchemaSource>();
                return s_schemaCache;
            }
        }
    }
}
