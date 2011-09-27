using System;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;

namespace Freetime.PluginManagement
{
    public interface IPluginManager
    {
        string Name { get; }
        
        Type GetControllerType(string controllerName);
        WebView GetWebView(string viewName);
        WebPartialView GetPartialView(string partialViewName);
        WebMasterPage GetWebMasterPage(string masterPageName);

        DataSessionServiceList GetDataSessionServices();

    }
}
