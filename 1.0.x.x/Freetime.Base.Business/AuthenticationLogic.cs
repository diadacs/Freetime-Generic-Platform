using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Freetime.Authentication;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;
using Freetime.Base.Framework;
using Freetime.Base.Business.Events;
using Freetime.Base.Business.Events.Global;
using Freetime.Base.Business.Implementable;

namespace Freetime.Base.Business
{
    public class AuthenticationLogic<TUser> : BaseLogic<IAuthenticationSession>, IAuthenticationLogic<TUser>
        where TUser : UserAccount, new()
    {
        protected override IAuthenticationSession GetDefaultSession()
        {
            return new AuthenticationSession();
        }

        public virtual FreetimeUser WebSignIn(string loginName,
            string password,
            string ipAddress)
        {
            return default(FreetimeUser);
            //TUser account = GetEntity<TUser>(u => u.LoginName == loginName);

            //UserAuthenticationEventArgs args = null;
            
            //if (account == null)
            //{
            //    args = new UserAuthenticationEventArgs("");
            //    args.LoginName = loginName;
            //    args.Password = password;
            //    args.IPAddress = ipAddress;  

            //    RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_FAILED, args);
            //    return default(FreetimeUser);
            //}

            //UTF8Encoding encoder = new UTF8Encoding();

            //byte[] encryptedPassword = Crypto.MD5ServiceProvider.ComputeHash(encoder.GetBytes(password));

            //if (account.Password != encryptedPassword.ToString())
            //{
            //    args = new UserAuthenticationEventArgs("");
            //    args.LoginName = loginName;
            //    args.Password = password;
            //    args.IPAddress = ipAddress;  

            //    RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_FAILED, args);
            //    return default(FreetimeUser);
            //}

            //args = new UserAuthenticationEventArgs("");
            //args.LoginName = loginName;
            //args.Password = password;
            //args.IPAddress = ipAddress;  

            //FreetimeUser user = new FreetimeUser(account, true);
            //RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_SUCCESS, args);
            //return user;
        }

        public virtual CFreetimeUser WebSignIn<CFreetimeUser>(string loginName,
            string password,
            string ipAddress)
            where CFreetimeUser : FreetimeUser, new()
        {
            return default(CFreetimeUser);
            //TUser account = GetEntity<TUser>(u => u.LoginName == loginName);

            //UserAuthenticationEventArgs args = null;

            //if (account == null)
            //{
            //    args = new UserAuthenticationEventArgs("");

            //    args.LoginName = loginName;
            //    args.Password = password;
            //    args.IPAddress = ipAddress;

            //    RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_FAILED, args);
            //    return default(CFreetimeUser);
            //}          

            //byte[] passwordBytes = Encoding.UTF8.GetBytes(password);         

            //if (account.Password != Convert.ToBase64String(Freetime.Base.Framework.Crypto.MD5ServiceProvider.ComputeHash(passwordBytes)))
            //{
            //    args = new UserAuthenticationEventArgs("");
            //    args.LoginName = loginName;
            //    args.Password = password;
            //    args.IPAddress = ipAddress;

            //    RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_FAILED, args);
            //    return default(CFreetimeUser);
            //}

            //args = new UserAuthenticationEventArgs("");
            //args.LoginName = loginName;
            //args.Password = password;
            //args.IPAddress = ipAddress;

            //CFreetimeUser user = CreateCFreetimeUser<CFreetimeUser>(account, true) as CFreetimeUser;
            //RaiseEvent(GlobalEventConstants.AUTHENTICATION_LOGIN_SUCCESS, args);
            //return user;
        }

        private delegate object CreateCFreetimeDelegate(TUser account, bool isAuthorized);

        private static Dictionary<Type, Delegate> m_CFreetimeDelegateCache = null;

        private static Dictionary<Type, Delegate> CreateCFreetimeDelegateCache
        {
            get
            {
                if (m_CFreetimeDelegateCache == null)
                    m_CFreetimeDelegateCache = new Dictionary<Type, Delegate>();
                return m_CFreetimeDelegateCache;
            }
        }

        private static object CreateCFreetimeUser<CFreetimeUser>(TUser account, bool isAuthorized)
        {
            Type type = typeof(CFreetimeUser);
            if (!CreateCFreetimeDelegateCache.ContainsKey(type))
            {
                Type[] methodArgs = { typeof(TUser), typeof(bool) };
                DynamicMethod dm = new DynamicMethod("CreateCFreetimeUserInstance", typeof(object), methodArgs, type);
                ILGenerator il = dm.GetILGenerator();
                il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, methodArgs, null));
                il.Emit(OpCodes.Ret);
                CreateCFreetimeDelegateCache.Add(type, dm.CreateDelegate(typeof(CreateCFreetimeDelegate)));
            }
            return (CreateCFreetimeDelegateCache[type] as CreateCFreetimeDelegate)(account, isAuthorized);
        }

        public virtual void WebSignOut(FreetimeUser user)
        { 
        
        }
    }
}

