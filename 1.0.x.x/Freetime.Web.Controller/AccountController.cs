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

namespace Freetime.Web.Controller
{
    public class AccountController : BaseController<IAuthenticationLogic<UserAccount>>
    {
        protected override IAuthenticationLogic<UserAccount> NewControllerLogic()
        {
            return new AuthenticationLogic<UserAccount>();
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
            else
                return RedirectToAction("Index", "Root");  
        }

        public ActionResult Logoff()
        {
            Logic.WebSignOut(CurrentUser);
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

            FreetimeUser user = Logic.WebSignIn<FreetimeUser>(userName, password, "");

            SetCurrentUser(user);
            return ModelState.IsValid;
        }

        private void SetCurrentUser(FreetimeUser user)
        {
            UserHandle.SetCurrentFreetimeUser(user);
        }

    }
}
