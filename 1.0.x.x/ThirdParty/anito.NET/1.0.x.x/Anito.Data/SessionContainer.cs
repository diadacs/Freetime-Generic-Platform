/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anito.Data
{
    public abstract class SessionContainer : ISessionContainer
    {
        private object m_keyValue = null;
        private ISession m_session = null;

        internal SessionContainer(ISession session)
        {
            m_session = session;
        }

        ISession ISessionContainer.DataSession
        {
            get
            {
                return m_session;
            }
        }

        protected virtual ISession DataSession
        {
            get
            {
                return m_session;
            }
        }

        public Object KeyValue
        {
            get
            {
                return m_keyValue;
            }
            set
            {
                m_keyValue = value;
            }
        }

        protected abstract void Load();
    }
}
