using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Business;
using Freetime.Authentication;

namespace Test.Freetime.Base.Business
{
    /// <summary>
    /// Unit Test for Freetime.Base.Business.AuthenticationLogic
    /// </summary>
    /// <Assembly>Freetime.Base.Business</Assembly>
    /// <Class>Freetime.Base.Business.AuthenticationLogic</Class>
    [TestClass]
    public class AuthenticationLogicTest
    {
       

        [TestMethod]
        public void TestSignInUserByUserPassword()
        {
            var logic = GetLogic();

            var actual = logic.SignInUser("admin", "password");
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestSignInUserByUserPasswordIp()
        {
            var logic = GetLogic();

            var actual = logic.SignInUser("admin", "password", "192.168.175.190");
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestSignInUserByUserPasswordRefUser()
        {
            var logic = GetLogic();
            FreetimeUser user = null;

            var actual = logic.SignInUser("admin", "password", ref user);
            Assert.IsTrue(actual);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestSignInUserByUserPasswordIpRefUser()
        {
            var logic = GetLogic();
            FreetimeUser user = null;

            var actual = logic.SignInUser("admin", "password", "192.168.175.190", ref user);
            Assert.IsTrue(actual);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestSignInUserUserNullUserNameThrowsArgumentNullException()
        {
            var logic = new AuthenticationLogic();

            Exception exception = null;
            try
            {
                logic.SignInUser(null, "password");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);

            Assert.AreEqual(
               typeof(ArgumentNullException), exception.GetType());
        }

        [TestMethod]
        public void TestSignInUserUserNullPasswordThrowsArgumentNullException()
        {
            var logic = new AuthenticationLogic();

            Exception exception = null;
            try
            {
                logic.SignInUser("admin", null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);

            Assert.AreEqual(
               typeof(ArgumentNullException), exception.GetType());
        }

        private AuthenticationLogic GetLogic()
        {
            var authenticationSession = new Mock<IAuthenticationSession>();

            var userAccount = new UserAccount
            {
                LoginName = "admin",
                Password = "X03MO1qnZdYdgyfeuILPmQ==",
                Name = "Freetime-G Administrator",
                UserProfile = 1,
                WebTheme = 1,
                Theme = 1,
                IsActive = true
            };

            authenticationSession.Setup(x => x.GetUserAccount("admin")).Returns(userAccount);


            var authenticationLogic = new Mock<AuthenticationLogic> { CallBase = true };
            authenticationLogic.Protected().Setup<IAuthenticationSession>("CurrentSession").Returns(authenticationSession.Object);

            return authenticationLogic.Object;
        }
    }
}
