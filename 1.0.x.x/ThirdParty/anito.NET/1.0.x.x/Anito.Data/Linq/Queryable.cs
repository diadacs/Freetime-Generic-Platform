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
using System.Linq.Expressions;

namespace Anito.Data.Linq
{
    public class Queryable<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable,
        IOrderedQueryable<T>, IOrderedQueryable 
    {

        private Expression m_expression;
        private IQueryProvider m_queryProvider = null;
        private ISession m_session = null;

        protected virtual ISession Session
        {
            get
            {
                return m_session;    
            }
        }

        public Queryable(IProvider provider)
        {
            m_expression = Expression.Constant(this);
            m_session = new DataSession(provider);
            m_queryProvider = new QueryProvider<T>(Session);
        }

        public Queryable(ISession session)
        {
            m_expression = Expression.Constant(this);
            m_session = session;
            m_queryProvider = new QueryProvider<T>(Session);
        }

        public Queryable(ISession session, Expression expression)
        {
            m_expression = expression;
            m_queryProvider = new QueryProvider<T>(session);
        }

        Type IQueryable.ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        Expression IQueryable.Expression
        {
            get
            {
                return m_expression;
            }

        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                return m_queryProvider;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {            
            return (m_queryProvider as QueryProvider<T>).GetEnumerator(m_expression);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }        

    }
}
