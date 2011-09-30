using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Freetime.Authentication;
using Freetime.GlobalHandling;
using Freetime.Base.Business;
using Freetime.Base.Business.Implementable;
using Freetime.Web.Context;
using Freetime.Web.Controller.Implementable;

namespace Freetime.Web.Controller
{    
    public abstract class BaseController<TLogic> : System.Web.Mvc.Controller, IFreetimeController where TLogic : ILogic
    {
        private FreetimeUser m_freetimeUser = null;
        private TLogic m_currentLogic = default(TLogic);

        protected virtual bool IsReplaceLogic
        {
            get
            {
                return false;
            }
        }

        protected virtual FreetimeUser CurrentUser
        {
            get
            {
                return UserHandle.CurrentUser;
            }
        }

        public string Theme
        {
            get
            {
                return m_freetimeUser.DefaultTheme;
            }
            set
            {
                m_freetimeUser.DefaultTheme = value;
            }
        }

        public BaseController()
        {
            
        }

        protected bool HasEvent(string eventName)
        {
            return GlobalEventDispatcher.HasEvent(eventName);
        }

        protected void RaiseEvent(string eventName, EventArgs args)
        {
            GlobalEventDispatcher.RaiseEvent(eventName, this, args);
        }

        protected void RaiseEvent(string eventName, object sender, EventArgs args)
        {
            GlobalEventDispatcher.RaiseEvent(eventName, sender, args);
        }

        protected abstract TLogic NewControllerLogic();

        protected virtual TLogic Logic
        {
            get
            {
                return GetOrReplaceLogic();
            }
        }

        protected virtual TLogic GetOrReplaceLogic()
        {
            if (m_currentLogic == null)
            {
                if (!IsReplaceLogic)
                    m_currentLogic = NewControllerLogic();
                else
                    m_currentLogic = GetReplacementLogic();
            }
            return m_currentLogic;        
        }

        protected virtual TLogic GetReplacementLogic()
        {
            return NewControllerLogic();
        }

        private bool IsLogicAssignableFrom(Type logicType)
        {
            return typeof(TLogic).IsAssignableFrom(logicType);
        }

    }

    public abstract class BaseController : BaseController<ISharedLogic>
    {

        protected override ISharedLogic NewControllerLogic()
        {
            return new SharedLogic();
        }
    }
}
