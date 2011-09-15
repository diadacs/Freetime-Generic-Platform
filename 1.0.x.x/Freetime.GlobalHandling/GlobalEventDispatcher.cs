using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Freetime.Base.Data;
using Freetime.Base.Data.Collection;
using Freetime.Configuration;

namespace Freetime.GlobalHandling
{
    public static class GlobalEventDispatcher
    {
                
        private static Dictionary<string, EventList> m_eventContainer = null;
        
        private static Dictionary<string, EventList> EventContainer
        {
            get
            {
                return m_eventContainer;
            }
        }

        
        static GlobalEventDispatcher()
        {
            m_eventContainer = new Dictionary<string, EventList>();
            if (!string.IsNullOrEmpty(ConfigurationManager.FreetimeConfig.GlobalEventConfigurationSection))
                LoadDefaultEventHandlers();

        }

        public static void AddEventHandler(string eventName, GlobalEvent handler)
        {
            if (!EventContainer.ContainsKey(eventName))
            {
                EventList list = new EventList(eventName);
                list.Add(handler);
                EventContainer.Add(eventName, list);
            }
            else
                EventContainer[eventName].Add(handler);
        }

        public static bool HasEvent(string eventName)
        {
            return EventContainer.ContainsKey(eventName);
        }

        public static void RaiseEvent(string eventName, object sender, EventArgs args)
        {
            if (EventContainer.ContainsKey(eventName))
                EventContainer[eventName].Invoke(sender, args);
        }

        public static void ClearEventHandlers()
        {
            EventContainer.Clear();
        }

        public static void RemoveHandlers(string eventName)
        {
            if(HasEvent(eventName))
                EventContainer.Remove(eventName);   
        }

        //Load Event Handlers from a section in a config file
        private static void LoadDefaultEventHandlers()
        {
            var config = System.Configuration.ConfigurationManager.GetSection(ConfigurationManager.FreetimeConfig.GlobalEventConfigurationSection);
            if (config == null)
                throw new Exception(string.Format("Config Section {0} not implemented", ConfigurationManager.FreetimeConfig.GlobalEventConfigurationSection));
            LoadEventHandlers(config as GlobalEventConfiguration);
        }

        public static void LoadEventHandlers(GlobalEventConfiguration configuration)
        {
            foreach (EventHandlerElement element in configuration.EventHandlerCollection)
            {
                if (!element.IsActive)
                    continue;
                try
                {                    
                    Assembly assembly = Assembly.LoadFile(string.Format("{0}\\{1}.dll", element.AssemblyLocation, element.Assembly));
                    Type eventHandlerType = assembly.GetType(element.Handler);
                    
                    //TODO Modify Activator
                    if (typeof(IGlobalEventHandler).IsAssignableFrom(eventHandlerType))
                        RegisterGlobalHandler(Activator.CreateInstance(eventHandlerType) as IGlobalEventHandler);
                    else
                    { 
                        //TODO Log Error
                    }
                }
                catch (Exception ex)
                { 
                    //TODO Log Error
                }
            }
        }

        public static void LoadEventHandlers(EventHandlerList eventList)
        {
            foreach (Base.Data.Entities.EventHandler handler in eventList)
            {
                if (!handler.IsActive)
                    continue;
                try
                {
                    Assembly assembly = Assembly.LoadFile(string.Format("{0}\\{1}.dll", handler.AssemblyLocation, handler.Assembly));
                    Type eventHandlerType = assembly.GetType(handler.EventHandlerClass);

                    //TODO Modify Activator
                    if (typeof(IGlobalEventHandler).IsAssignableFrom(eventHandlerType))
                        RegisterGlobalHandler(Activator.CreateInstance(eventHandlerType) as IGlobalEventHandler);
                    else
                    {
                        //TODO Log Error
                    }
                }
                catch (Exception ex)
                {
                    //TODO Log Error
                }
            }
        } 

        public static void RegisterGlobalHandler(IGlobalEventHandler handler)
        {
            handler.RegisterEventHandlers();
        }

    }
}
