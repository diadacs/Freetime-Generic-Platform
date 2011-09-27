using System;

namespace Freetime.Configuration
{
    public static class ConfigurationManager
    {        

        private static FreetimeConfiguration s_freetimeConfig;

        public static FreetimeConfiguration FreetimeConfiguration
        {
            get
            {
                if (s_freetimeConfig == null)
                {
                    var config = System.Configuration.ConfigurationManager.GetSection("Freetime.Configuration");
                    if (config == null)
                        throw new Exception("Freetime.Configuration not implemented");                    
                    s_freetimeConfig = config as FreetimeConfiguration;
                }
                return s_freetimeConfig;
            }
        }

        public static void SetFreetimeConfig(FreetimeConfiguration config)
        {
            s_freetimeConfig = config;
        }
    }
}
