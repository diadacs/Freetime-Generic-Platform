using System;
using System.ServiceModel;
using Freetime.Authentication;

namespace Freetime.Base.Data.Contracts
{
    [ServiceContract]
    public interface IDataSession : IDisposable
    {
        [OperationContract]
        void SetFreetimeUser(FreetimeUser user);

        [OperationContract]
        void BeginTransaction();

        [OperationContract]
        void CommitTransaction();

        [OperationContract]
        void RollbackTransaction();

        [OperationContract]
        void CancelTransaction();
    }
}
