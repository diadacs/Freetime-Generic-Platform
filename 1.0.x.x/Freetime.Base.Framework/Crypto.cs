using System.Security.Cryptography;

namespace Freetime.Base.Framework
{
    public class Crypto
    {
        private static MD5CryptoServiceProvider s_md5ServiceProvider;
        
        public static MD5CryptoServiceProvider Md5CryptoServiceProvider
        {
            get
            {
                if (s_md5ServiceProvider == null)
                    Md5CryptoServiceProvider = new MD5CryptoServiceProvider();
                return s_md5ServiceProvider;
            }
            private set
            {                
                s_md5ServiceProvider = value;
            }
        }
    }
}
