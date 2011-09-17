/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Data;
using Anito.Data.Common;
using MySql.Data.MySqlClient;

namespace Anito.Data.MySqlClient
{
    public class CommandParameter : AdoCommandParameterBase
    {
        protected override IDbDataParameter NewSqlParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value);
        }

        public CommandParameter(ref IDbCommand command, string parameterName)
            : base(ref command, parameterName)
        {
        }

        public CommandParameter(ref IDbCommand command, string parameterName, object value)
            : base(ref  command, parameterName, value)
        {
        }
    }
}
