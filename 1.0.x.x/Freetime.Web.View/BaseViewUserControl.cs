using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Freetime.Authentication;
using Freetime.Web.Context;

namespace Freetime.Web.View
{
    public class BaseViewUserControl : System.Web.Mvc.ViewUserControl
    {
        private FreetimeUser m_freetimeUser = null;

        public virtual FreetimeUser CurrentUser
        {
            get
            {
                return UserHandle.CurrentUser;
            }
        }
    }

    public class BaseViewUserControl<T> : System.Web.Mvc.ViewUserControl<T>
    {
        private FreetimeUser m_freetimeUser = null;

        public virtual FreetimeUser CurrentUser
        {
            get
            {
                return UserHandle.CurrentUser;
            }
        }
    }
}
