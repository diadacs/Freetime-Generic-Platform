using Freetime.Base.Data.Entities;
using System.ServiceModel;

namespace Freetime.Base.Data.Contracts
{
    [ServiceContract]
    public interface IAuthenticationSession : IDataSession
    {
        [OperationContract]
        UserAccount GetUserAccount(string username);
    }
}
