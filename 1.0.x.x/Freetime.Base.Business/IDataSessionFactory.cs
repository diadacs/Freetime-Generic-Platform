using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public interface IDataSessionFactory
    {
        TContract GetDataSession<TContract>() where TContract : IDataSession;
        TContract GetDataSession<TContract>(TContract defaultContract) where TContract : IDataSession;
    }
}
