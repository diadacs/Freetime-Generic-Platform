using System;
using Freetime.PluginManagement;

namespace Freetime.Web.Controller
{
    public class ControllerFactory : System.Web.Mvc.DefaultControllerFactory
    {

        protected override Type GetControllerType(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {
                var type = PluginManager.Current.GetControllerType(controllerName);
                return type ?? base.GetControllerType(requestContext, controllerName);
            }
            catch (Exception ex)
            { 
                //TODO Log Error
                throw ex;
            }
        } 

    }
}
