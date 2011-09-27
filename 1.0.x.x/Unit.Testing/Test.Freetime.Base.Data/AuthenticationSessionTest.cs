using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Freetime.Base.Data;
using Freetime.Base.Data.Entities;
using Anito.Data;

namespace Test.Freetime.Base.Data
{
    /// <summary>
    /// Unit Test for Freetime.Base.Data.AuthenticationSession
    /// </summary>
    /// <Assembly>Freetime.Base.Data</Assembly>
    /// <Class>Freetime.Base.Data.AuthenticationSession</Class>
    [TestClass]
    public class AuthenticationSessionTest
    {

        /// <summary>
        /// Expected : UserAccount Entity
        /// </summary>
        [TestMethod]
        public void GetUserAccount()
        {
            var userAccount = new UserAccount { 
                LoginName = "freetime@freetime-G.com",
                Password = "password",
                Name = "Freetime Admin",
                UserProfile = 0,
                WebTheme = 0,
                Theme = 0,
                IsActive = true                    
            };
            var authenticationSession = new Mock<AuthenticationSession> { CallBase = true };

            var anitoSession = new Mock<ISession> { CallBase = true };

            anitoSession.Setup(x => x.GetT(It.IsAny<Expression<Func<UserAccount, bool>>>())).Returns(userAccount);

            authenticationSession.Protected().Setup<ISession>("CurrentSession").Returns(anitoSession.Object);

            var actual = authenticationSession.Object.GetUserAccount("freetime@freetime-G.com");
            
            Assert.AreEqual(userAccount, actual);
        }

        /// <summary>
        /// Expected : ArgumentNullException
        /// </summary>
        [TestMethod]
        public void GetUserAccountThrowsArgumentNullException()
        {
            var target = new AuthenticationSession();
            Exception exception = null;

            try
            {
                target.GetUserAccount(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);

            Assert.AreEqual(
               typeof(ArgumentNullException), exception.GetType());

        }

    }
}
