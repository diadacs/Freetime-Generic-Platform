using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
    /// Unit Test for Freetime.Base.Data.LocalizationSession
    /// </summary>
    /// <Assembly>Freetime.Base.Data</Assembly>
    /// <Class>Freetime.Base.Data.LocalizationSession</Class>
    [TestClass]
    public class LocalizationSessionTest
    {        
        /// <summary>
        /// Expected : Language Entity
        /// </summary>
        [TestMethod]
        public void GetLanguage()
        {
            Mock<LocalizationSession> localizationSession = new Mock<LocalizationSession> { CallBase = true };

            Mock<ISession> anitoSession = new Mock<ISession> { CallBase= true };
            
            string languageCode = "English - United States";

            Language language = new Language
            {
                LanguageCode = languageCode,
                DisplayName = languageCode,
                IsActive = true
            };

            anitoSession.Setup(x => x.GetT<Language>(It.IsAny<Expression<Func<Language, bool>>>())).Returns(language);

            localizationSession.Protected().Setup<ISession>("CurrentSession").Returns(anitoSession.Object);
                      
            var actual = localizationSession.Object.GetLanguage(languageCode);

            Assert.AreEqual(language, actual);
        }

        /// <summary>
        /// Expected : ArgumentNullException
        /// </summary>
        [TestMethod]
        public void GetLanguageThrowsArgumentNullException()
        {
            LocalizationSession target = new LocalizationSession();

            Exception exception = null;

            try
            {
                target.GetLanguage(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);

            Assert.AreEqual<Type>(
               typeof(ArgumentNullException), exception.GetType());

        }

    }
}
