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
using System.Linq.Expressions;

namespace Anito.Data.Linq
{
    internal class QueryProvider<T> : IQueryProvider 
    {
        private ISession m_session = null;

        protected ISession Session
        {
            get
            {
                return m_session;
            }
        }

        public QueryProvider(ISession session)
        {
            m_session = session;
        }

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
        {
            return new Queryable<S>(Session, expression);           
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return new Queryable<T>(Session, expression);
        }

        S IQueryProvider.Execute<S>(Expression expression)
        {
            return (S) Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return Execute(expression);
        }

        private object Execute(Expression expression) 
        {
            switch (expression.NodeType)
            {                
                case ExpressionType.Call:
                    return ExecuteMethodCall(expression as MethodCallExpression); 
            }
            return null;
        }

        private object ExecuteMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.Name == "Count")
            {
                if (expression.Arguments.Count == 1)
                    return m_session.Count<T>();
                else if (expression.Arguments.Count == 2)
                    return m_session.Count<T>(expression.Arguments[1]);
            }
            else if (expression.Method.Name == "Single")
            {
                return m_session.GetT<T>((expression as Expression));
            }
            else if (expression.Method.Name == "Select")
            {
                
            }
            return null;
        }


        public IEnumerator<T> GetEnumerator(Expression expression)
        {
            return new TEnumerator<T>(Session, expression);
        }
    }
}
