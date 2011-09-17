/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Data;
using System.Data.SqlClient;
using Anito.Data.Common;


namespace Anito.Data.SqliteClient 
{
    public class CommandParameter : AdoCommandParameterBase
    {
        protected override IDbDataParameter NewSqlParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }

        public CommandParameter(ref IDbCommand command, string parameterName)
            : base(ref command, parameterName)
        {
        }

        public CommandParameter(ref IDbCommand command, string parameterName, object value)
            : base(ref command, parameterName, value)
        {
        }
    }
}
