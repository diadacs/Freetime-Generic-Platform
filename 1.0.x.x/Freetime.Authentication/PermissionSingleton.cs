using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.Authentication
{
    public static class PermissionSingleton
    {
        private static Dictionary<string, PermissionCollection> s_userPermissions;

        private static Dictionary<string, PermissionCollection> UserPermissions
        {
            get
            {
                s_userPermissions = s_userPermissions ?? new Dictionary<string, PermissionCollection>();
                return s_userPermissions;
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
