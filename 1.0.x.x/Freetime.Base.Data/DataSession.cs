using Anito.Data;
using Freetime.Authentication;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Data
{

    public abstract class DataSession : IDataSession
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

        protected virtual FreetimeUser CurrentUser { get; set;}

        private static ISession GetDefaultSession()
        {
            var provider = ProviderFactory.GetProvider();
            var session = ProviderFactory.GetSession(provider);
            return session;
        }

        public virtual void SetFreetimeUser(FreetimeUser user)
        {
            CurrentUser = user;
        }

        public virtual void Dispose()
        {
            m_anitoSession = null;
        }

        #region Transaction
        public virtual void BeginTransaction()
        {
            CurrentSession.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            CurrentSession.CommitTransaction();
        }

        public virtual void RollbackTransaction()
        {
            CurrentSession.RollBackTransaction();
        }

        public virtual void CancelTransaction()
        {
            CurrentSession.CancelTransaction();
        }
        #endregion

    }
}
