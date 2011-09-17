/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using Anito.Data.Common;
using System.Data;

namespace Anito.Data.SqliteClient
{
    public class Transaction : AdoTransactionBase
    {
        public Transaction(IDbTransaction transaction)
            : base(transaction)
        {
        }

    }
}
