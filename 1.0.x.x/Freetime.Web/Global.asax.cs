using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Routing;
using Freetime.GlobalHandling;
using Freetime.Base.Business;

namespace Freetime.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Root",
                "{action}",
                new { controller = "Root", action = "Index" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Root", action = "Index", id = "" }
            );

            
        }

        protected virtual void Application_Start()
        {
            ViewEngines.Engines.Clear();
            Freetime.Web.Routing.ViewEngine engine = new Freetime.Web.Routing.ViewEngine();
            ViewEngines.Engines.Add(engine);
            RegisterRoutes(RouteTable.Routes);          
            
            
            //Load from a config section
            //GlobalEventConfiguration eventConfig = ConfigurationManager.GetSection("globalEventConfig") as GlobalEventConfiguration;
            //PluginLogic.PluginManager.LoadEventHandlers(eventConfig);

            //Load event handlers from an xml file
            //PluginLogic.PluginManager.LoadEventHandlers(@"C:\Documents and Settings\Administrator\Desktop\freetime\1.0.1.x\Freetime.Web\Config\Events.config");

            
            //Load Controllers from an xml file
            //Freetime.PluginManagement.PluginManager.Current.
            //PluginLogic.PluginManager.LoadWebControllers(@"C:\Documents and Settings\Administrator\Desktop\freetime\1.0.1.x\Freetime.Web\Config\Controllers.config");

            //Set Controller Factory
            Freetime.Web.Controller.ControllerFactory factory = new Freetime.Web.Controller.ControllerFactory();
            ControllerBuilder.Current.SetControllerFactory(factory);
        }
    }
}