/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Serialization;
using Anito.Data.Configuration;
using Anito.Data.Schema;

namespace Anito.Data
{
    public static class ProviderFactory
    {
        #region Variables
        private static Dictionary<string, ProviderSetting> m_providerCache;
        private static Dictionary<Type, Delegate> m_createProviderMethodsCache = null;
        private static ProviderConfiguration m_configuration = null;

        private class ProviderSetting
        {
            public string Name { get; set; }
            public string ProviderTypeName { get; set; }
            public string ConnectionString { get; set; }
            public Type ProviderType { get; set; }
        }
        #endregion

        #region Properties
        private static Dictionary<Type, Delegate> CreateProviderMethodsCache
        {
            get
            {
                if (m_createProviderMethodsCache == null)
                    m_createProviderMethodsCache = new Dictionary<Type, Delegate>();
                return m_createProviderMethodsCache;
            }
        }

        private static Dictionary<string, ProviderSetting> ProviderCache
        {
            get
            {
                return m_providerCache;
            }
        }

        private static ProviderConfiguration Configuration
        {
            get
            {
                if (m_configuration == null)
                    InitConfiguration();              
                return m_configuration;
            }
        }
        #endregion

        #region Constructor

        static ProviderFactory()
        {
            m_providerCache = new Dictionary<string, ProviderSetting>();
            InitConfiguration();
        }

        #endregion

        #region InitConfiguration
        private static void InitConfiguration()
        {
            var section = System.Configuration.ConfigurationManager.GetSection("AnitoProviderConfiguration");
            if (section == null)
                throw new Exception("Default Anito Provider Configuration Section doesn't exist (AnitoProviderConfiguration)");
            m_configuration = section as ProviderConfiguration;

            foreach (ProviderConfigurationElement element in m_configuration.Providers)
            {
                ProviderSetting setting = new ProviderSetting();
                setting.Name = element.Name;
                setting.ConnectionString = element.ConnectionString;
                setting.ProviderTypeName = element.Type;

                ProviderCache.Add(element.Name, setting);

                TypeSchemaSource source = new TypeSchemaSource();
                foreach (SchemaSourceElement schemaElement in element.SchemaSourceCollection)
                {
                    XmlReader reader = new XmlTextReader(schemaElement.SourceFile);
                    (source as IXmlSerializable).ReadXml(reader);
                    reader.Close();
                }
                CacheManager.SchemaCache.Add(element.Name, source);
            }    
        }
        #endregion

        #region GetProvider
        public static IProvider GetProvider()
        {
            return GetProvider(Configuration.DefaultProvider);
        }

        public static IProvider GetProvider(string providerName)
        {
            if (!ProviderCache.ContainsKey(providerName))
                throw new Exception("Unknown Provider. (See Provider Configuration)");

            ProviderSetting setting = ProviderCache[providerName];

            if (setting.ProviderType == null)
            {
                Type type = Type.GetType(setting.ProviderTypeName);
                if (type == null)
                    throw new Exception(string.Format("Unknown Type {0}", setting.ProviderTypeName));
                setting.ProviderType = type;
            }
            IProvider provider = CreateIProviderInstance(setting.ProviderType);

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[setting.ConnectionString].ConnectionString;

            if (!string.IsNullOrEmpty(connectionString))
                provider.ConnectionString = connectionString;
            
            if (typeof(ProviderBase).IsAssignableFrom(provider.GetType()))
                (provider as ProviderBase).SetProviderName(providerName);

            return provider;
        }

        public static ISession GetSession()
        {
            return GetSession(GetProvider());
        }

        public static ISession GetSession(IProvider provider)
        {
            return new DataSession(provider);
        }

        public static ISession GetSession(string providerName)
        {
            return GetSession(GetProvider(providerName));
        }

        public static void SetProviderConfiguration(ProviderConfiguration configuration)
        {
            m_configuration = configuration;
        }

        private static IProvider CreateIProviderInstance(Type type)
        {
            if (!CreateProviderMethodsCache.ContainsKey(type))
            {
                DynamicMethod dm = new DynamicMethod("CreateIProviderInstance", typeof(IProvider), Type.EmptyTypes, type);
                ILGenerator il = dm.GetILGenerator();
                il.DeclareLocal(type);
                il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Ret);
                CreateProviderMethodsCache.Add(type, dm.CreateDelegate(typeof(CreateIProviderDelegate)));
            }
            CreateIProviderDelegate method = CreateProviderMethodsCache[type] as CreateIProviderDelegate;
            return method.Invoke();
        }
        #endregion
    }
}
