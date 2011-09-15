using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Business;
using Freetime.Base.Data.Entities;
using System.Reflection;
using Freetime.PluginManagement;

namespace Freetime.Web.Controller
{
    public class ControllerFactory : System.Web.Mvc.DefaultControllerFactory
    {

        public ControllerFactory()
        { 
        
        }

        protected override Type GetControllerType(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {
                Type type = PluginManager.Current.GetControllerType(controllerName);
                if (type == null)
                    return base.GetControllerType(requestContext, controllerName);
                return type;
            }
            catch (Exception ex)
            { 
                //TODO Log Error
                throw ex;
            }
        } 

    }
}
