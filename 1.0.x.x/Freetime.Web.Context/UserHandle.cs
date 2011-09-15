using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Freetime.Authentication;

namespace Freetime.Web.Context
{
    public static class UserHandle
    {
        private const string FREETIME_USER_SESSION_KEY = "0327DA76-6526-455D-8E95-17DA56D9B5E5";

        public static void SetCurrentFreetimeUser(FreetimeUser user)
        {
            System.Web.HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] = user;
        }

        public static void KillCurrentFreetimeUser()
        {
            System.Web.HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] = null;
        }

        public static FreetimeUser CurrentUser
        { 
            get
            {
                if (System.Web.HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] == null)
                    System.Web.HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] = new FreetimeUser();
                return System.Web.HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] as FreetimeUser;       
            }
        }
    }
}
