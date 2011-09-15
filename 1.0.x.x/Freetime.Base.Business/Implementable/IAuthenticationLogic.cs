using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Framework;
using Freetime.Authentication;
using Freetime.Base.Data.Entities;

namespace Freetime.Base.Business.Implementable
{
    public interface IAuthenticationLogic<TUser> : ILogic
        where TUser : UserAccount
    {
        FreetimeUser WebSignIn(string loginName,
            string password,
            string ipAddress);

        CFreetimeUser WebSignIn<CFreetimeUser>(string loginName,
            string password,
            string ipAddress)
            where CFreetimeUser : FreetimeUser, new();

        void WebSignOut(FreetimeUser user);
    }
}
