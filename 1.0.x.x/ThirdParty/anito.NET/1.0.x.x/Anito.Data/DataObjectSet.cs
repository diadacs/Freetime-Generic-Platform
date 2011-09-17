/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;


namespace Anito.Data
{
    public class DataObjectSet<TList, TEntity>  : SessionContainer
        , IEnumerable
        where TList : IList<TEntity> , new()
        where TEntity : class, new()
    {
        private TList m_list;

        public TList Details
        {
            get
            {
                if(Equals(m_list, default(TList))) Load();
                return m_list;
            }
            set
            {
                m_list = value;
            }
        }

        internal DataObjectSet(ISession session)
            : base(session)

        {
 
        }

        protected override void Load()
        {
            if (DataSession == null || KeyValue == null)
                m_list = default(TList);
            else
                m_list = DataSession.GetList<TList, TEntity>(KeyValue);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {            
            return Details.GetEnumerator();
        }
    }
}

