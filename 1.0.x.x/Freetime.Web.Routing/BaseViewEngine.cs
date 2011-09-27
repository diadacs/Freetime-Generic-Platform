using System;
using System.Web.Mvc;
using Freetime.Web.Context;
using Freetime.Configuration;

namespace Freetime.Web.Routing
{
    public abstract class BaseViewEngine : WebFormViewEngine
    {
        protected BaseViewEngine()
        {
            ViewLocationFormats = new[] {
            "~/{0}.aspx",
            "~/{0}.ascx",
            "~/{1}/{0}.aspx",
            "~/{1}/{0}.ascx"
            };

            PartialViewLocationFormats = new[]{
            "~/Controls/{0}.aspx",
            "~/Controls/{0}.ascx"
            };

            MasterLocationFormats = new[] {
            "~/{0}.master"
            };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //Site.Master as default
            if (string.IsNullOrEmpty(masterName))
                masterName = ConfigurationManager.FreetimeConfiguration.DefaultMasterPage;

            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("Value is required.", "viewName");


            string[] searchedViewLocations;
            string[] searchedMasterLocations;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            
            string viewPath = GetViewPath(ViewLocationFormats, viewName, UserHandle.CurrentUser.DefaultTheme,
                              controllerName, out searchedViewLocations);

            
            string masterPath = GetMasterPath(MasterLocationFormats, masterName, viewName, controllerName,
                UserHandle.CurrentUser.DefaultTheme, out searchedMasterLocations);

            if (!(string.IsNullOrEmpty(viewPath)) &&
               ((masterPath != string.Empty) || string.IsNullOrEmpty(masterName)))
                return new ViewEngineResult(
                    (CreateView(controllerContext, viewPath, masterPath)), this);            
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }


        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            string[] searchedLocations;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string partialViewPath = GetPartialPath(PartialViewLocationFormats, partialViewName, UserHandle.CurrentUser.DefaultTheme,
                controllerName, out searchedLocations);

            if (!string.IsNullOrEmpty(partialViewPath))
                return new ViewEngineResult(
                    CreatePartialView(controllerContext, partialViewPath), this);           
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }

        protected abstract string GetViewPath(string[] locations, string viewName, string theme,
            string controllerName, out string[] searchedLocations);

        protected abstract string GetMasterPath(string[] locations, string mastername, string viewName,
            string controllerName, string theme, out string[] searchedLocations);

        protected abstract string GetPartialPath(string[] locations, string partialViewName, string theme,
            string controllerName, out string[] searchedLocations);

    }
}
