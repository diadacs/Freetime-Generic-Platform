using Freetime.Base.Component;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Freetime.Base.Component
{
    /// <summary>
    /// Unit Test for Freetime.Base.Component.LanguageManager
    /// </summary>
    /// <Assembly>Freetime.Base.Component</Assembly>
    /// <Class>Freetime.Base.Component.LanguageManager</Class>
    [TestClass]
    public class LanguageManagerTest
    {

        /// <summary>
        /// Expected : Current Lanaguae Manager Not Null
        /// </summary>
        [TestMethod]
        public void LanguageManagerCurrentNotNullTest()
        {
            Assert.IsNotNull(LanguageManager.Current);
        }
    }
}
