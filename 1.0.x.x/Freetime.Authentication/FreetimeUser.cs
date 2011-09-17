using System.Runtime.Serialization;


namespace Freetime.Authentication
{
    [DataContract]
    public class FreetimeUser
    {
        private string m_defaultTheme = string.Empty;
        private readonly int m_userId = default(int);
        private readonly int m_userRole = default(int);
        private readonly bool m_isAuthorized = default(bool);
        private readonly string m_name = string.Empty;

        [DataMember]
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
        
        public FreetimeUser(int userId, 
            int userRole,
            string name,
            bool isAuthorized,
            string theme)
            : this()
        {
            m_userId = userId;
            m_userRole = userRole;
            m_name = name;
            m_isAuthorized = isAuthorized;
            DefaultTheme = theme;
        }

        public FreetimeUser()
        {
        }

        [DataMember]
        public int UserId
        {
            get
            {
                return m_userId;
            }
        }

        [DataMember]
        public int UserRole
        {
            get
            {
                return m_userRole;
            }
        }

        [DataMember]
        public bool IsAuthorized
        {
            get
            {
                return m_isAuthorized;
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return m_name;
            }
        }

    }
}
