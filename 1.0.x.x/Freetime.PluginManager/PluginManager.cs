using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;
using Freetime.PluginManagement.Configuration;
using Freetime.Configuration;


namespace Freetime.PluginManagement
{    
    public class PluginManager : IPluginManager
    {
        private static IPluginManager __INSTANCE = null;

        private PluginManagerConfiguration Configuration { get; set; }

        public static IPluginManager Current
        {
            get
            {
                if (__INSTANCE == null)
                    __INSTANCE = new PluginManager();
                return __INSTANCE;
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
            string configSection = ConfigurationManager.FreetimeConfig.PluginManagementConfigurationSection;
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
            __INSTANCE = manager;
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
        private WebControllerList m_controllerList = null;

        private static Dictionary<string, Type> m_controllerCache = null;

        private static Dictionary<string, Type> Controllers
        {
            get
            {
                if (m_controllerCache == null)
                    m_controllerCache = new Dictionary<string, Type>();
                return m_controllerCache;
            }
        }

        Type IPluginManager.GetControllerType(string controllerName)
        {
            if (!Controllers.ContainsKey(controllerName))
            {
                var controllers = from controller in m_controllerList where controller.Name == controllerName select controller;
                if (controllers.Count() > 0)
                {
                    WebController webController = controllers.ElementAt(0);
                    if (!webController.IsActive)
                        return null;
                    Type controllerType = Type.GetType(string.Format("{0}, {1}", webController.ControllerType, webController.Assembly));
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

        private WebControllerList GetWebControllers(string xmlsource)
        {
            Stream stream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WebControllerList));
                stream = new FileStream(xmlsource, FileMode.Open);
                WebControllerList list = serializer.Deserialize(stream) as WebControllerList;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        #endregion

        #region Views
        private WebViewList m_viewList = null;

        public WebView GetWebView(string viewName)
        {
            var views = from view in m_viewList where view.Name == viewName && view.IsActive select view;
            if (views.Count() == 0)
                return null;
            return views.ElementAt(0);            
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
                XmlSerializer serializer = new XmlSerializer(typeof(WebViewList));
                stream = new FileStream(xmlsource, FileMode.Open);
                WebViewList list = serializer.Deserialize(stream) as WebViewList;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        #endregion

        #region PartialViews
        private WebPartialViewList m_partialViewList = null;

        public WebPartialView GetPartialView(string partialViewName)
        {
            var partials = from partial in m_partialViewList where partial.Name == partialViewName select partial;
            if (partials.Count() == 0)
                return null;
            return partials.ElementAt(0);
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
                XmlSerializer serializer = new XmlSerializer(typeof(WebPartialViewList));
                stream = new FileStream(xmlsource, FileMode.Open);
                WebPartialViewList list = serializer.Deserialize(stream) as WebPartialViewList;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        #endregion

        #region MasterPages
        private WebMasterPageList m_masterPageList = null;

        public WebMasterPage GetWebMasterPage(string masterPageName)
        {
            var masters = from master in m_masterPageList where master.Name == masterPageName && master.IsActive select master;
            if (masters.Count() == 0)
                return null;
            return masters.ElementAt(0);
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
                XmlSerializer serializer = new XmlSerializer(typeof(WebMasterPageList));
                stream = new FileStream(xmlsource, FileMode.Open);
                WebMasterPageList list = serializer.Deserialize(stream) as WebMasterPageList;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        #endregion

        #region DataSession
        private DataSessionServiceList m_dataServiceList = null;

        private void LoadDataSessionServices(string sourceXml)
        {
            m_dataServiceList = GetDataSessionServiceList(sourceXml);
        }

        private DataSessionServiceList GetDataSessionServiceList(string xmlsource)
        {
            Stream stream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DataSessionServiceList));
                stream = new FileStream(xmlsource, FileMode.Open);
                DataSessionServiceList list = serializer.Deserialize(stream) as DataSessionServiceList;
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
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
