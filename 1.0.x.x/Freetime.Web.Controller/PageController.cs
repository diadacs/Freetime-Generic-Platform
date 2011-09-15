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
    public class PageController : BaseController
    {
        public ActionResult Edit(string returnUrl)
        {            
            return RedirectAndEdit(returnUrl);           
        }

        protected virtual RedirectResult RedirectAndEdit(string stringUrl)
        {
            //TODO manipulate URL here add params that enables editing
            if (stringUrl.Contains("edit=1"))
                return Redirect(stringUrl);

            if (stringUrl.Contains("edit=0"))
                return Redirect(stringUrl.Replace("edit=0", "edit=1"));

            return Redirect(stringUrl);
        }        
    }
}
