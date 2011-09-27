using Moq;
using Freetime.PluginManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Freetime.PluginManagement
{
    /// <summary>
    /// Unit Test for Freetime.PluginManagement.PluginManager
    /// </summary>
    /// <Assembly>Freetime.PluginManagement.</Assembly>
    /// <Class>Freetime.PluginManagement.PluginManager</Class>
    [TestClass]
    public class PluginManagerTest
    {
        /// <summary>
        /// Expected : PluginManager Not Null
        /// </summary>
        [TestMethod]
        public void PluginManagerNotNullTest()
        {
            var pluginManager = new Mock<IPluginManager>();
            PluginManager.SetPluginManager(pluginManager.Object);
            Assert.IsTrue(PluginManager.Current != null);
        }
    }
}
