/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using Anito.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Anito.Data.SqlClient
{
    public class CommandExecutor : AdoCommandExecutorBase
    {
        protected override IDbConnection NewConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected override ITransaction NewTransaction(IDbTransaction transaction)
        {
            return new Transaction(transaction);
        }
    }
}
