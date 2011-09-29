using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Freetime.Base.Framework.Diagnostics
{
    public static class DebugUtil
    {
        #region Category
        public enum Category
        {
            Message,
            Information,
            Critical
        }
        #endregion

        #region Write

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

        #endregion

        #region WriteLine
        public static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        public static void WriteLine(string message, Category category)
        {
            Debug.WriteLine(message, category.ToString());
        }

        public static void WriteLine(object value)
        {
            Debug.WriteLine(value);
        }

        public static void WriteLine(object value, Category category)
        {
            Debug.WriteLine(value, category.ToString());
        }

        #endregion

        #region WriteIf

        public static void WriteIf(bool condition, string message)
        {
            Debug.WriteIf(condition, message);
        }

        public static void WriteIf(bool condition, string message, Category category)
        {
            Debug.WriteIf(condition, message, category.ToString());
        }

        public static void WriteIf(bool condition, object value)
        {
            Debug.WriteIf(condition, value);
        }

        public static void WriteIf(bool condition, object value, Category category)
        {
            Debug.WriteIf(condition, value, category.ToString());
        }
        
        #endregion

        #region WriteLineIf

        public static void WriteLineIf(bool condition, string message)
        {
            Debug.WriteLineIf(condition, message);
        }

        public static void WriteLineIf(bool condition, string message, Category category)
        {
            Debug.WriteLineIf(condition, message, category.ToString());
        }

        public static void WriteLineIf(bool condition, object value)
        {
            Debug.WriteLineIf(condition, value);
        }

        public static void WriteLineIf(bool condition, object value, Category category)
        {
            Debug.WriteLineIf(condition, value, category.ToString());
        }

        #endregion
    }
}
