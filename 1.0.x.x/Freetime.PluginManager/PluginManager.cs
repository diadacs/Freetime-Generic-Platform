using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;
using Freetime.PluginManagement.Configuration;
using Freetime.Configuration;


namespace Freetime.PluginManagement
{    
    public class PluginManager : IPluginManager
    {
        private static IPluginManager s_instance;

        private PluginManagerConfiguration Configuration { get; set; }

        public static IPluginManager Current
        {
            get
            {
                s_instance = s_instance ?? new PluginManager();
                return s_instance;
            }
        }

        string IPluginManager.Name
        {
            get
            {
                return "Freetime Default Plugin Manager";
            }
        }

        internal PluginManager()
        {
            var configSection = ConfigurationManager.FreetimeConfiguration.PluginManagementConfigurationSection;
            if (!string.IsNullOrEmpty(configSection))
            {
                var config = System.Configuration.ConfigurationManager.GetSection(configSection);
                if (config == null)
                {
                    //TODO throw proper exception
                    throw new Exception("PluginManagementConfiguration Not Implemented");
                }
                Configuration = config as PluginManagerConfiguration;
            }
            else 
                throw new Exception("PluginManagementConfiguration Not Implemented");
            SetRequiredAttributes();
        }

        public static void SetPluginManager(IPluginManager manager)
        {
            s_instance = manager;
        }

        private void SetRequiredAttributes()
        { 
            foreach (PluginManagerConfigurationAttribute attribute in Configuration.Attributes)
            {
                if (attribute.Key == "WebControllersConfig")
                    LoadWebControllers(attribute.Value);
                else if (attribute.Key == "WebViewsConfig")
                    LoadWebViews(attribute.Value);
                else if(attribute.Key == "WebPartialViewsConfig")
                    LoadWebPartialViews(attribute.Value);
                else if (attribute.Key == "MasterPagesConfig")
                    LoadWebMasterPages(attribute.Value);
                else if (attribute.Key == "DataServiceConfig")
                    LoadDataSessionServices(attribute.Value);                
            }
        }


        #region Controllers
        private WebControllerList m_controllerList;

        private static Dictionary<string, Type> s_controllerCache;

        private static Dictionary<string, Type> Controllers
        {
            get
            {
                s_controllerCache = s_controllerCache ?? new Dictionary<string, Type>();
                return s_controllerCache;
            }
        }

        Type IPluginManager.GetControllerType(string controllerName)
        {
            if (!Controllers.ContainsKey(controllerName))
            {
                var webController = m_controllerList.FirstOrDefault(x => x.Name == controllerName);
                if (webController != null)
                {
                    if (!webController.IsActive)
                        return null;
                    var controllerType = Type.GetType(string.Format("{0}, {1}", webController.ControllerType, webController.Assembly));
                    Controllers.Add(controllerName, controllerType);
                }
                else
                    return null;
            }
            return Controllers[controllerName];
        }

        public void LoadWebControllers(string xmlsource)
        {
            m_controllerList = GetWebControllers(xmlsource);
        }

        private static WebControllerList GetWebControllers(string xmlsource)
        {
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(typeof(WebControllerList));
                stream = new FileStream(xmlsource, FileMode.Open);
                var list = serializer.Deserialize(stream) as WebControllerList;
                return list;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }
        }
        #endregion

        #region Views
        private WebViewList m_viewList;

        public WebView GetWebView(string viewName)
        {
            return m_viewList.FirstOrDefault(x => x.Name == viewName && x.IsActive);
        }

        private void LoadWebViews(string sourceXml)
        {
            m_viewList = GetWebViewList(sourceXml);
        }

        private WebViewList GetWebViewList(string xmlsource)
        {
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(typeof(WebViewList));
                stream = new FileStream(xmlsource, FileMode.Open);
                var list = serializer.Deserialize(stream) as WebViewList;
                return list;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }
        }
        #endregion

        #region PartialViews
        private WebPartialViewList m_partialViewList;

        public WebPartialView GetPartialView(string partialViewName)
        {
            return m_partialViewList.FirstOrDefault(x => x.Name == partialViewName);
        }

        private void LoadWebPartialViews(string sourceXml)
        {
            m_partialViewList = GetWebPartialViewList(sourceXml);
        }

        private WebPartialViewList GetWebPartialViewList(string xmlsource)
        {
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(typeof(WebPartialViewList));
                stream = new FileStream(xmlsource, FileMode.Open);
                var list = serializer.Deserialize(stream) as WebPartialViewList;
                return list;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }
        }
        #endregion

        #region MasterPages
        private WebMasterPageList m_masterPageList;

        public WebMasterPage GetWebMasterPage(string masterPageName)
        {
            return m_masterPageList.FirstOrDefault(x => x.Name == masterPageName && x.IsActive);
        }

        private void LoadWebMasterPages(string sourceXml)
        {
            m_masterPageList = GetMasterPageList(sourceXml);
        }

        private WebMasterPageList GetMasterPageList(string xmlsource)
        {
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(typeof(WebMasterPageList));
                stream = new FileStream(xmlsource, FileMode.Open);
                var list = serializer.Deserialize(stream) as WebMasterPageList;
                return list;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }
        }
        #endregion

        #region DataSession
        private DataSessionServiceList m_dataServiceList;

        private void LoadDataSessionServices(string sourceXml)
        {
            m_dataServiceList = GetDataSessionServiceList(sourceXml);
        }

        private DataSessionServiceList GetDataSessionServiceList(string xmlsource)
        {
            Stream stream = null;
            try
            {
                var serializer = new XmlSerializer(typeof(DataSessionServiceList));
                stream = new FileStream(xmlsource, FileMode.Open);
                var list = serializer.Deserialize(stream) as DataSessionServiceList;
                return list;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }
        }

        DataSessionServiceList IPluginManager.GetDataSessionServices()
        {
            return m_dataServiceList;
        }
        #endregion

    }
}
