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
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;
using Freetime.Web.Context;

namespace Freetime.Web.Controller
{
    public class AccountController : BaseController<IAuthenticationLogic>
    {
        protected override IAuthenticationLogic NewControllerLogic()
        {
            return new AuthenticationLogic();
        }

        public ActionResult Logon()
        {
            return View("Logon");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {
            if (!ValidateLogOn(userName, password))
            {
                return View();
            }
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Root");  
        }

        public ActionResult Logoff()
        {
            Logic.SignOutUser(CurrentUser);
            string theme = UserHandle.CurrentUser.DefaultTheme;
            SetCurrentUser(null);
            UserHandle.CurrentUser.DefaultTheme = theme;
            return View("Logoff");
        }

        public ActionResult Register()
        {
            return View();
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }

            FreetimeUser user = null;
            bool isAuthorized = Logic.SignInUser(userName, password, "", ref user);

            SetCurrentUser(user);
            return isAuthorized;
            //return ModelState.IsValid;
        }

        private void SetCurrentUser(FreetimeUser user)
        {
            UserHandle.SetCurrentFreetimeUser(user);
        }

    }
}
