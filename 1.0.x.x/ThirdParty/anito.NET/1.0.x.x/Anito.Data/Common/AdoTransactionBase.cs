/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Data;

namespace Anito.Data.Common
{
    public abstract class AdoTransactionBase : ITransaction
    {
        private IDbTransaction m_transaction;

        public virtual IDbTransaction SqlTransaction
        {
            get
            {
                return m_transaction;    
            }
        }

        protected AdoTransactionBase(IDbTransaction transaction)
        {
            m_transaction = transaction;
        }

        public virtual void Commit()
        {
            m_transaction.Commit();
        }

        public virtual void Rollback()
        {
            m_transaction.Rollback();
        }

        public virtual void Cancel()
        {
            m_transaction = null;
        }
    }
}
