using System;
using System.ServiceModel;
using Freetime.Base.Data.Entities;

namespace Freetime.Base.Data.Contracts
{
    [ServiceContract]
    public interface ILocalizationSession : IDataSession
    {
        [OperationContract]
        Language GetLanguage(string languageCode);
    }
}
