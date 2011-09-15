using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.GlobalHandling;

namespace Freetime.Plugin.EventHandling
{
    public class EventHandler : IGlobalEventHandler
    {
        void IGlobalEventHandler.RegisterEventHandlers()
        {
            //Register methods
            
            GlobalEventDispatcher.AddEventHandler("authentication_login_success", Authentication_Login_Success);
            GlobalEventDispatcher.AddEventHandler("authentication_login_failed", Authentication_Login_Failed);
        }

        private void Authentication_Login_Success(object sender, EventArgs args)
        { 
            //If the this event handling plugin is registered it should pass here if you logged in successfully
        }

        private void Authentication_Login_Failed(object sender, EventArgs args)
        {
            //If the this event handling plugin is registered it should pass here if a log in fails
        }
    }
}
