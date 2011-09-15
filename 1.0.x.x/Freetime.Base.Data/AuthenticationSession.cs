using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;

namespace Freetime.Base.Data
{
    public class AuthenticationSession : DataSession, IAuthenticationSession
    {
        public UserAccount GetUserAccount(string username)
        {
            return default(UserAccount);
        }
    }
}
