/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using Anito.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace Anito.Data.MySqlClient
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
            : base(new MySqlCommand())
        {

        }

        public Command(MySqlCommand command)
            : base(command)
        {
        }
    }
}
