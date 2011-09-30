using Freetime.Base.Data.Contracts;
using Freetime.Base.Business.Implementable;

namespace Freetime.Base.Business
{
    public interface IDataSessionFactory
    {
        TContract GetDataSession<TContract>(ILogic logic, TContract defaultContract) where TContract : IDataSession;
    }
}
