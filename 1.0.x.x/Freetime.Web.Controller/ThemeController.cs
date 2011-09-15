using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Freetime.Authentication;
using Freetime.Base.Business;
using Freetime.Base.Business.Implementable;
using Freetime.Base.Data;
using Freetime.Base.Data.Entities;
using Freetime.Web.Context;
using System.Text;

namespace Freetime.Web.Controller
{
    public class ThemeController : BaseController
    {
        public ActionResult ChangeTheme(string theme, string returnUrl)
        {
            UserHandle.CurrentUser.DefaultTheme = theme;
            if (!String.IsNullOrEmpty(returnUrl))    
                return Redirect(returnUrl);           
            else
                return RedirectToAction("Index", "Root");
        }
    }
}
