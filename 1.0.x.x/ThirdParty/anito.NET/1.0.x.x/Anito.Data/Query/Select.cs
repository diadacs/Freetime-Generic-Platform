using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anito.Data.Query
{
    public class Select<T> : ISelect<T>
        where T : class
    {
        public void Where(string column)
        { 
        
        }

        internal Select(ISession session)
        { 
        
        }

        public IEnumerable<T> Execute()
        {
            return default(IEnumerable<T>);
        }
    }
}
