﻿using System;
using System.Text;
using Freetime.Authentication;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Framework;
using Freetime.Base.Business.Events.Global;
using Freetime.Base.Business.Implementable;

namespace Freetime.Base.Business
{
    public class AuthenticationLogic: BaseLogic<IAuthenticationSession>, IAuthenticationLogic
    {
        private AuthenticationSession m_authenticationSession;

        protected override IAuthenticationSession DefaultSession
        {
            get
            {
                m_authenticationSession = m_authenticationSession ?? new AuthenticationSession();
                return m_authenticationSession;
            }

        }

        public virtual bool SignInUser(string loginName,
            string password)
        {
            FreetimeUser user = null;
            return SignInUser(loginName, password, ref user);
        }

        public virtual bool SignInUser(string loginName,
            string password,
            string ipAddress)
        {
            FreetimeUser user = null;
            return SignInUser(loginName, password, ipAddress, ref user);
        }

        public virtual bool SignInUser(string loginName,
            string password,
            ref FreetimeUser user)
        {
            user = AuthenticateUser(loginName, password, string.Empty);
            return user != null;
        }

        public virtual bool SignInUser(string loginName,
            string password,
            string ipAddress,
            ref FreetimeUser user)
        {
            user = AuthenticateUser(loginName, password, ipAddress);
            return user != null;
        }

        protected virtual FreetimeUser AuthenticateUser(string loginName,
            string password,
            string ipAddress)
        {

            var account = CurrentSession.GetUserAccount(loginName);
            UserAuthenticationEventArgs args;

            if(account == null)
            {
                args = new UserAuthenticationEventArgs(string.Empty) //TODO Supply Localized Message
                           {
                               LoginName =  loginName,
                               Password =  password,
                               IPAddress = ipAddress
                           };
                RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_FAILED, args);
                return default(FreetimeUser);
            }

            var encoder = new UTF8Encoding();
            var encryptedPassword = Crypto.Md5CryptoServiceProvider.ComputeHash(encoder.GetBytes(password));

            if (account.Password != Convert.ToBase64String(encryptedPassword))
            {
                args = new UserAuthenticationEventArgs(string.Empty) //TODO Supply Localized Message
                           {
                               LoginName = loginName,
                               Password = password,
                               IPAddress = ipAddress
                           };

                RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_FAILED, args);
                return default(FreetimeUser);
            }

            args = new UserAuthenticationEventArgs(string.Empty); //TODO Supply Localized Message
            var user = new FreetimeUser(
                (int) account.ID,
                1,
                account.Name,
                true,
                "FreetimeBlue"
                );
                              
                           
            RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_SUCCESS, args);
            return user;
        }


        public virtual void SignOutUser(FreetimeUser user)
        { 
        
        }
    }
}

