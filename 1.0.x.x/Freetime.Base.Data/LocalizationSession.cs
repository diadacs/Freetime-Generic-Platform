using System;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Entities;

namespace Freetime.Base.Data
{
    public class LocalizationSession : DataSession, ILocalizationSession
    {
        public Language GetLanguage(string languageCode)
        {
            if (Equals(languageCode, null))
                throw new ArgumentNullException("languageCode");

            return CurrentSession.GetT<Language>(l => l.LanguageCode == languageCode);
        }
    }
}
