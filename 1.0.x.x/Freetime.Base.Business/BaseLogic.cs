using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Freetime.GlobalHandling;
using Freetime.Authentication;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;
using Freetime.Configuration;
using Freetime.Base.Business.Implementable;

namespace Freetime.Base.Business
{
    public abstract class BaseLogic<TSession> : ILogic, IDisposable
        where TSession : IDataSession
    {
        private TSession m_dataSession;

        private TSession CurrentSession
        {
            get
            {
                if (m_dataSession == null)
                    m_dataSession = DataSessionBuilder.Current.GetDataSession<TSession>(GetDefaultSession());
                return m_dataSession;
            }
        }

        protected abstract TSession GetDefaultSession();

        public virtual FreetimeUser CurrentUser { get; private set; }

        protected BaseLogic(FreetimeUser user)
        {
            CurrentUser = user;
        }

        protected BaseLogic()
        {             
        }

        protected bool HasEvent(string eventName)
        {
            return GlobalEventDispatcher.HasEvent(eventName);
        }

        protected void RaiseEvent(string eventName)
        {
            GlobalEventDispatcher.RaiseEvent(eventName, this, null);
        }

        protected void RaiseEvent(string eventName, EventArgs args)
        {
            GlobalEventDispatcher.RaiseEvent(eventName, this, args);
        }

        protected void RaiseEvent(string eventName, object sender, EventArgs args)
        {
            GlobalEventDispatcher.RaiseEvent(eventName, sender, args);
        }

        public virtual void Dispose()
        {
            CurrentUser = null;
        }
    }
}
