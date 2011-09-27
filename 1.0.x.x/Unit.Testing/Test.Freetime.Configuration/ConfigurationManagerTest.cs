using Freetime.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Test.Freetime.Configuration
{
    /// <summary>
    /// Unit Test for Freetime.Configuration.ConfigurationManager
    /// </summary>
    /// <Assembly>Freetime.Configuration</Assembly>
    /// <Class>Freetime.Configuration.ConfigurationManager</Class>
    [TestClass]
    public class ConfigurationManagerTest
    {

        /// <summary>
        /// Expected : ConfigurationManager.FreetimeConfiguration not null
        /// </summary>
        [TestMethod]
        public void FreetimeConfigNotNullTest()
        {
            var freetimeConfig = new Mock<FreetimeConfiguration>();
            ConfigurationManager.SetFreetimeConfig(freetimeConfig.Object);
            Assert.IsNotNull(ConfigurationManager.FreetimeConfiguration);
        }
    }
}
