using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.Authentication
{
    public static class PermissionSingleton
    {
        private static Dictionary<string, PermissionCollection> m_userPermissions;

        private static Dictionary<string, PermissionCollection> UserPermissions
        {
            get
            {
                if (m_userPermissions == null)
                    m_userPermissions = new Dictionary<string, PermissionCollection>();
                return m_userPermissions;
            }
        }

        private static PermissionCollection GetPermission(string userCode)
        {
            if (!UserPermissions.ContainsKey(userCode))
            { 
                //Get User Permission
            }
            return UserPermissions[userCode];
        }

        public static void Clear()
        {
            UserPermissions.Clear();
        }
    }
}
