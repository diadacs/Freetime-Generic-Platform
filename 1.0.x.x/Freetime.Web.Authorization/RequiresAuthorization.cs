﻿using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Freetime.Web.Authorization
{
    public class RequiresAuthorization : AuthorizeAttribute
    {
        

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //if (httpContext == null)
            //{
            //    throw new ArgumentNullException("httpContext");
            //}

            //IPrincipal user = httpContext.User;
            //if (!user.Identity.IsAuthenticated)
            //{
            //    return false;
            //}

            //if (_usersSplit.Length > 0 && !_usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            //{
            //    return false;
            //}

            //if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
