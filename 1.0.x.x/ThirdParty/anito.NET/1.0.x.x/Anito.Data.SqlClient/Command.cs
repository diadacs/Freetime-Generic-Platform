﻿/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Data;
using System.Data.SqlClient;
using Anito.Data.Common;

namespace Anito.Data.SqlClient
{
    public class Command : AdoCommandBase
    {
  
        protected override ICommandParameter NewCommandParameter(ref IDbCommand command, string parameterName)
        {
            return new CommandParameter(ref command, parameterName);
        }

        protected override ICommandParameter NewCommandParameter(ref IDbCommand command, string parameterName, object value)
        {
            return new CommandParameter(ref command, parameterName, value);
        }
        
        public Command()
            : base(new SqlCommand())
        {

        }

        public Command(SqlCommand command)
            : base(command)
        {
        }

    }
}
