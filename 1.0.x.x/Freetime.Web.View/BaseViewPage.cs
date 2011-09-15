using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Authentication;
using System.Web.Mvc;
using Freetime.Web.Context;
using Freetime.Base.Framework;

namespace Freetime.Web.View
{
    public abstract class BaseViewPage : System.Web.Mvc.ViewPage, IEditable, INameable
    {
        public virtual string Name
        {
            get
            {
                return ToString();
            }
        }

        public virtual FreetimeUser CurrentUser
        {
            get
            {
                return UserHandle.CurrentUser;
            }
        }


        public bool IsEdit
        {
            get 
            {
                return false;
            }
        }
    }

    public abstract class BaseViewPage<T> : System.Web.Mvc.ViewPage<T>, IEditable, INameable
    {
        public virtual string Name
        {
            get
            {
                return ToString();
            }
        }

        public virtual FreetimeUser CurrentUser
        {
            get
            {
                return UserHandle.CurrentUser;
            }
        }

        public bool IsEdit 
        {
            get
            {
                return false;
            }
        }
    }
}
