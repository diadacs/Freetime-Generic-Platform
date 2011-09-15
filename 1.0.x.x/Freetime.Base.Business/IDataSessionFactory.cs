using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public interface IDataSessionFactory
    {
        TContract GetDataSession<TContract>() where TContract : IDataSession;
        TContract GetDataSession<TContract>(TContract defaultContract) where TContract : IDataSession;
    }
}
