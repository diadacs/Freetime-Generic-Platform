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
using System.Data.Linq;

namespace Anito.Data
{
    public class DataTable<T> : Linq.Queryable<T> where T : class, new()
    {
        public DataTable(IProvider provider)
            :base(provider)
        {                            
        }

        public DataTable(ISession session)
            : base(session)
        {            
        }

        public T New()
        {
            return new T();
        }

        public void Add(T item)
        {
            if(!Session.IsTransactionInitiated)
                Session.BeginTransaction();
            if(typeof(IDataObject).IsAssignableFrom(typeof(T)))
                Session.Save(item as IDataObject);   
        }

        public void UndoChanges()
        {
            Session.RollBackTransaction();
        }

        public void SubmitChanges()
        {
            Session.CommitTransaction();
        }

        public IEnumerable<T> Paged(int pageSize, int page)
        {
            return Session.GetPagedList<List<T>, T>(pageSize, page);
        }

        public IEnumerable<T> Paged(int pageSize, int page, Expression<Func<T, bool>> expression)
        {
            return Session.GetPagedList<List<T>, T>(pageSize, page, expression);
        }
    }
}
