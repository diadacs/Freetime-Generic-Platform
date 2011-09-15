using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data;
using Freetime.Base.Data.Entities;
using Freetime.Base.Framework;
using Freetime.Configuration;

namespace Freetime.Authentication
{
    public class FreetimeUser
    {
        private string m_defaultTheme = string.Empty;
        private int m_userId = default(int);
        private int m_userRole = default(int);
        private bool m_isAuthorized = default(bool);
        private string m_name = string.Empty;

        public string DefaultTheme
        {
            get
            {
                return m_defaultTheme;    
            }
            set
            {
                m_defaultTheme = value;
            }
        }
        
        public FreetimeUser(UserAccount userAccount, bool isAuthorized)
            : this()
        {
            m_userId = userAccount.ID;
            m_userRole = userAccount.UserRole;
            m_isAuthorized = isAuthorized;
            if(!string.IsNullOrEmpty(userAccount.Theme))
                m_defaultTheme = userAccount.Theme;
            m_name = userAccount.Name;
        }

        public FreetimeUser()
        {
            m_defaultTheme = ConfigurationManager.FreetimeConfig.DefaultTheme;
        }

        public int UserId
        {
            get
            {
                return m_userId;
            }
        }

        public int UserRole
        {
            get
            {
                return m_userRole;
            }
        }

        public bool IsAuthorized
        {
            get
            {
                return m_isAuthorized;
            }
        }

        public bool IsPermitted(string permissionCode)
        {
            return default(bool);
        }

        public string Name
        {
            get
            {
                return m_name;
            }
        }



    }
}
