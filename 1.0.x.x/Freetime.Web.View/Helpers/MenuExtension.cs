using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Freetime.Authentication;

namespace Freetime.Web.View.Helpers
{
    public static class MenuExtension
    {
        public static string UserMenu(this HtmlHelper helper, FreetimeUser user)
        {
            return string.Empty;
        }

        public static string UserMenu(this HtmlHelper helper)
        {
            return string.Empty;
        }
    }
}
