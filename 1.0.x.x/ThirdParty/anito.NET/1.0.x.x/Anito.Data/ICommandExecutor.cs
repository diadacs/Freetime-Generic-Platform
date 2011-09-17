/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Data.Common;

namespace Anito.Data
{
    public interface ICommandExecutor : IDisposable
    {
        ITransaction InitiateTransaction();
        void CommitTransaction(ITransaction transaction);
        void RollbackTransaction(ITransaction transaction);

        DbDataReader ExecuteReader(ICommand command);
        DbDataReader ExecuteReader(ICommand command, ITransaction transaction);

        int ExecuteNonQuery(ICommand command, ITransaction transaction);
        int ExecuteNonQuery(ICommand command);

        object ExecuteScalar(ICommand command);
        object ExecuteScalar(ICommand command, ITransaction transaction);

        int ExecuteCount(ICommand command);
        int ExecuteCount(ICommand command, ITransaction transaction);

        void FinalizeProcedureParameters(Procedure procedure, ICommand command);
        void FinalizeCommand(ICommand command);
    }
}
