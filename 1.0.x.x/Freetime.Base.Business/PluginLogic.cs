using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;
using Freetime.GlobalHandling;
using Freetime.Base.Business.Implementable;

namespace Freetime.Base.Business
{
    public class PluginLogic : BaseLogic<IDataSession>, IPluginLogic
    {
        #region Variables
        private static IPluginLogic __INSTANCE = null;

        private WebControllerList m_controllerList = null;        
        #endregion

        protected override IDataSession GetDefaultSession()
        {
            throw new NotImplementedException();
        }

        #region EventHandling
        public EventHandlerList GetEventHandlers(string xmlsource)
        {
            Stream stream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EventHandlerList));
                stream = new FileStream(xmlsource, FileMode.Open);
                EventHandlerList list = serializer.Deserialize(stream) as EventHandlerList;
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

        public EventHandlerList GetEventHandlers()
        {
            return default(EventHandlerList);
            //return GetAll<EventHandlerList, Data.Entities.EventHandler>();
        }

        public void LoadEventHandlers(string xmlsource)
        {
            EventHandlerList list = GetEventHandlers(xmlsource);
            GlobalEventDispatcher.LoadEventHandlers(list);
        }

        public void LoadEventHandlers(GlobalEventConfiguration configuration)
        {
            GlobalEventDispatcher.LoadEventHandlers(configuration);
        }

        public void ClearEventHandlers()
        {
            GlobalEventDispatcher.ClearEventHandlers();
        }
        #endregion

        #region Controller Injection
        public WebController GetWebController(string name)
        {
            var controllers = from controller in m_controllerList where controller.Name == name select controller;
            if (controllers.Count() > 0)
                return controllers.ElementAt(0);
            return null;
        }

        public void LoadWebControllers(string xmlsource)
        {
            m_controllerList = GetWebControllers(xmlsource);
        }

        public WebControllerList GetWebControllers(string xmlsource)
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

        public WebControllerList GetWebControllers()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructor
        internal PluginLogic()
        {
        }
        #endregion

        public static void SetPluginLogic(IPluginLogic pluginLogic)
        {
            __INSTANCE = pluginLogic;
        }

        public static IPluginLogic PluginManager
        { 
            get
            {
                if (__INSTANCE == null)
                {
                    //use default
                    __INSTANCE = new PluginLogic();
                }
                return __INSTANCE;
            }
            
        }
    }
}

