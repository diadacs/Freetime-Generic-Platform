using System;
using Freetime.Base.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Freetime.Base.Data.Contracts;

namespace Freetime.Configuration
{
    public static class ConfigurationManager
    {        

        private static FreetimeConfiguration m_freetimeConfig = null;

        public static FreetimeConfiguration FreetimeConfig
        {
            get
            {
                if (m_freetimeConfig == null)
                {
                    var config = System.Configuration.ConfigurationManager.GetSection("Freetime.Configuration");
                    if (config == null)
                        throw new Exception("Freetime.Configuration not implemented");                    
                    m_freetimeConfig = config as FreetimeConfiguration;
                }
                return m_freetimeConfig;
            }
        }

        public static void SetFreetimeConfig(FreetimeConfiguration config)
        {
            m_freetimeConfig = config;
        }
    }
}
