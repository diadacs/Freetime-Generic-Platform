using System;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;

namespace Freetime.Base.Data
{
    public class AuthenticationSession : DataSession, IAuthenticationSession
    {
        public UserAccount GetUserAccount(string username)
        {
            if (Equals(username, null)) 
                throw new ArgumentNullException("username");
                
            return CurrentSession.GetT<UserAccount>( u => u.LoginName == username);
        }
    }
}
