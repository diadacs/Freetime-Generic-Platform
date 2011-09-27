using Freetime.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Freetime.Authentication
{
    /// <summary>
    /// Unit Test for Freetime.Base.Framework.Crypto
    /// </summary>
    /// <Assembly>Freetime.Base.Framework</Assembly>
    /// <Class>Freetime.Base.Framework.Crypto</Class>
    [TestClass]
    public class FreetimeUserTest
    {

        /// <summary>
        /// Expected : Initialized FreetimeUser
        /// </summary>
        [TestMethod]
        public void InitiateFreetimeUserTest()
        {
            var user = new FreetimeUser(1, 1, "Test User", true, "DefaultTheme");
            Assert.IsNotNull(user);
        }
    }
}
