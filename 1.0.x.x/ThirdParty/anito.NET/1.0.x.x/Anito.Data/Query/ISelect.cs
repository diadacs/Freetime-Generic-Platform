using System.Collections.Generic;


namespace Anito.Data.Query
{
    public interface ISelect<T> where T : class
    {
        IEnumerable<T> Execute();

        
    }
}
