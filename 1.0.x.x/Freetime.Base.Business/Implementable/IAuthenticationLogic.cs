using Freetime.Authentication;

namespace Freetime.Base.Business.Implementable
{
    public interface IAuthenticationLogic : ILogic
    {
        bool SignInUser(string loginName,
            string password);

        bool SignInUser(string loginName,
            string password,
            string ipAddress);

        bool SignInUser(string loginName,
            string password, 
            ref FreetimeUser user);

        bool SignInUser(string loginName,
            string password,
            string ipAddress,
            ref FreetimeUser user);
        

        void SignOutUser(FreetimeUser user);
    }
}
