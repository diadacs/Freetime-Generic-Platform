using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using Freetime.PluginManagement;
using Freetime.Web.Context;

namespace Freetime.Web.Routing
{
    public class ViewEngine : BaseViewEngine
    {
       
        protected override string GetViewPath(string[] locations, string viewName, string theme,
            string controllerName, out string[] searchedLocations)
        {
            var view = PluginManager.Current.GetWebView(viewName);
            searchedLocations = null;
            if (view == null || !view.IsActive)
            {
                string path = null;

                searchedLocations = new string[locations.Length];

                for (int i = 0; i < locations.Length; i++)
                {
                    path = string.Format(CultureInfo.InvariantCulture, locations[i],
                                         new object[] { viewName, controllerName });
                    if (VirtualPathProvider.FileExists(path))
                    {
                        searchedLocations = new string[0];
                        return path;
                    }
                    searchedLocations[i] = path;
                }
                return null;
            }

            if (view.IsThemed)
                return string.Format("~/Themes/{0}/{1}", theme, view.File);
            
            return view.File;
        }


        protected override string GetMasterPath(string[] locations, string mastername, string viewName,
            string controllerName, string theme, out string[] searchedLocations)
        {
            var master = PluginManager.Current.GetWebMasterPage(mastername);
            searchedLocations = null;
            if (master == null || !master.IsActive)
            {
                string path = null;

                searchedLocations = new string[locations.Length];

                for (int i = 0; i < locations.Length; i++)
                {
                    path = string.Format(CultureInfo.InvariantCulture, locations[i],
                                         new object[] { viewName, controllerName, theme });
                    if (this.VirtualPathProvider.FileExists(path))
                    {
                        searchedLocations = new string[0];
                        return path;
                    }
                    searchedLocations[i] = path;
                }
                return null;
            }

            if(master.IsThemed)
                return string.Format("~/Themes/{0}/{1}", theme, master.File);
            return master.File;
        }

        protected override string GetPartialPath(string[] locations, string partialViewName, string theme, 
            string controllerName, out string[] searchedLocations)
        {
            var partialView = PluginManager.Current.GetPartialView(partialViewName);
            searchedLocations = null;
            if (partialView == null || !partialView.IsActive)
            {
                string path = null;

                searchedLocations = new string[locations.Length];

                for (int i = 0; i < locations.Length; i++)
                {
                    path = string.Format(CultureInfo.InvariantCulture, locations[i],
                                         new object[] { partialViewName, controllerName });
                    if (VirtualPathProvider.FileExists(path))
                    {
                        searchedLocations = new string[0];
                        return path;
                    }
                    searchedLocations[i] = path;
                }
                return null;
            }
            if (partialView.IsThemed)
                return string.Format("~/Themes/{0}/{1}", theme, partialView.File);
            return partialView.File;
        }

    }
}
