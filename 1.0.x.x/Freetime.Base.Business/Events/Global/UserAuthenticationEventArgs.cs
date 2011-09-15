using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.GlobalHandling;
using Freetime.Base.Data;

namespace Freetime.Base.Business.Events.Global
{
    public class UserAuthenticationEventArgs : EventArgs
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public bool LoginSuccessful { get; set; }
        public string Message { get; private set; }

        public UserAuthenticationEventArgs(string message)
        {
            Message = message;
        }
    }
}
