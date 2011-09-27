using Freetime.Base.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Freetime.Base.Framework
{
    /// <summary>
    /// Unit Test for Freetime.Base.Framework.Crypto
    /// </summary>
    /// <Assembly>Freetime.Base.Framework</Assembly>
    /// <Class>Freetime.Base.Framework.Crypto</Class>
    [TestClass]
    public class CryptoTest
    {

        /// <summary>
        /// Expected : Crypto should not be null
        /// </summary>
        [TestMethod]
        public void CryptoNullTest()
        {
            var provider = Crypto.Md5CryptoServiceProvider;
            Assert.IsNotNull(provider);
        }
    }
}
