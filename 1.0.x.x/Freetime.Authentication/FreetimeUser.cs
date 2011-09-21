using System.Runtime.Serialization;


namespace Freetime.Authentication
{
    [DataContract]
    public class FreetimeUser
    {
        private string m_defaultTheme = string.Empty;
        
        [DataMember]
        private readonly int m_userId = default(int);

        [DataMember]
        private readonly int m_userRole = default(int);

        [DataMember]
        private readonly bool m_isAuthorized = default(bool);

        [DataMember]
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

        public string Name
        {
            get
            {
                return m_name;
            }
        }

    }
}
