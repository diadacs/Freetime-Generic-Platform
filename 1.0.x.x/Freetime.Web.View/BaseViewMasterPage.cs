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
using System.Web.Mvc;
using System.Collections.Generic;
using Freetime.Web.Context;
using Freetime.Base.Framework;

namespace Freetime.Web.View
{
    public class BaseViewMasterPage : System.Web.Mvc.ViewMasterPage
    {
        public virtual string ViewName
        {
            get
            {
                try
                {
                    string name =(ViewContext.View as INameable).Name;
                    return name;
                }
                catch (Exception ex)
                { 
                
                }
                    
                return string.Empty;
            }
        }

        public virtual FreetimeUser CurrentUser
        {
            get
            {
                return UserHandle.CurrentUser;
            }
        }
    }
}
