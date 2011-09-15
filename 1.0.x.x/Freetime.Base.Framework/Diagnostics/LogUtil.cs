using System.Diagnostics;

namespace Freetime.Base.Framework.Diagnostics
{
    public static class LogUtil
    {
        public enum Category
        { 
            Message,
            Information,
            Critical
        }

        public static void Write(object value)
        {       
            Debug.Write(value);
        }

        public static void Write(object value, Category category)
        {            
            Debug.Write(value, category.ToString());
        }

        public static void Write(string message)
        {
            Debug.Write(message);
        }

        public static void Write(string message, Category category)
        {            
            Debug.Write(message, category.ToString());
        }
    }
}
