using Anito.Data;
using Freetime.Base.Data.Functions;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Data
{

    public class DataSession : IDataSession
    {
        private ISession m_anitoSession;

        protected virtual ISession CurrentSession 
        {
            get
            {
                m_anitoSession = m_anitoSession ?? GetDefaultSession();
                return m_anitoSession;
            }
        }

        private static ISession GetDefaultSession()
        {
            var provider = ProviderFactory.GetProvider();
            var session = ProviderFactory.GetSession(provider);
            return session;
        }

        public virtual string GetDocumentCode(string transaction)
        {
            var generatedDocCode = CurrentSession.GetT<Entities.GeneratedDocumentCode>(Procedures.GenerateDocumentCode(transaction).Procedure);
            
            return (!generatedDocCode.IsSuffix) ?
                string.Format("{0}{1}{2}", generatedDocCode.Extension, generatedDocCode.Separator, generatedDocCode.Count)
                    : string.Format("{0}{1}{2}", generatedDocCode.Count, generatedDocCode.Separator, generatedDocCode.Extension);
        }

        public virtual void Dispose()
        {
            m_anitoSession = null;
        }
    }
}
