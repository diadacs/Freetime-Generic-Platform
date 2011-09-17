/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Linq.Expressions;

namespace Anito.Data.Linq
{
    internal class TEnumerator<T> : IEnumerator<T>
    {
        ISession m_session = null;
        IDataReader m_reader = null;

        protected ISession Session
        {
            get
            {
                return m_session;
            }
        }

        protected IDataReader Reader
        {
            get
            {
                return m_reader;
            }
            set
            {
                m_reader = value;
            }
        }

        public TEnumerator(ISession session)
        {
            m_session = session;
            Reader = m_session.GetReader<T>();
        }

        public TEnumerator(ISession session, Expression expression)
        {
            m_session = session;
            Reader = m_session.GetReader<T>(expression);
        }

        public T Current
        {
            get
            {
                return Session.GetT<T>(Reader);
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        void IDisposable.Dispose() { }


        public void Reset()
        {
            Reader.Close();
            //Reader.CloseReader();
            //if (!Session.IsTransactionInitiated)

            //Reader.CloseConnection();
            Reader = Session.GetReader<T>();
        }

        public bool MoveNext()
        {
            //Lazy Load
            return Reader.Read();
        }
    }
}

