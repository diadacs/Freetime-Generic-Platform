using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;
using Freetime.Base.Data.Contracts;

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
