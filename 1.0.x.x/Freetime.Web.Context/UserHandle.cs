using System.Web;
using Freetime.Authentication;
using Freetime.Configuration;

namespace Freetime.Web.Context
{
    public static class UserHandle
    {
        private const string FREETIME_USER_SESSION_KEY = "0327DA76-6526-455D-8E95-17DA56D9B5E5";

        public static void SetCurrentFreetimeUser(FreetimeUser user)
        {
            HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] = user;
        }

        public static void KillCurrentFreetimeUser()
        {
            HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] = null;
        }

        public static FreetimeUser CurrentUser
        { 
            get
            {
                if (HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] == null)
                {
                    var freetimeUser = new FreetimeUser();
                    freetimeUser.DefaultTheme = ConfigurationManager.FreetimeConfig.DefaultTheme;
                    HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] = freetimeUser;
                }
                return HttpContext.Current.Session[FREETIME_USER_SESSION_KEY] as FreetimeUser;       
            }
        }
    }
}
