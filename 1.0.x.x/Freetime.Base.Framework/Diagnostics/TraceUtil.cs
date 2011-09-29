using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Freetime.Base.Framework.Diagnostics
{
    public static class TraceUtil
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
            Trace.Write(value);
        }

        public static void Write(object value, Category category)
        {
            Trace.Write(value, category.ToString());
        }

        public static void Write(string message)
        {
            Trace.Write(message);
        }

        public static void Write(string message, Category category)
        {
            Trace.Write(message, category.ToString());
        }

        #endregion

        #region WriteLine
        public static void WriteLine(string message)
        {
            Trace.WriteLine(message);
        }

        public static void WriteLine(string message, Category category)
        {
            Trace.WriteLine(message, category.ToString());
        }

        public static void WriteLine(object value)
        {
            Trace.WriteLine(value);
        }

        public static void WriteLine(object value, Category category)
        {
            Trace.WriteLine(value, category.ToString());
        }

        #endregion

        #region WriteIf

        public static void WriteIf(bool condition, string message)
        {
            Trace.WriteIf(condition, message);
        }

        public static void WriteIf(bool condition, string message, Category category)
        {
            Trace.WriteIf(condition, message, category.ToString());
        }

        public static void WriteIf(bool condition, object value)
        {
            Trace.WriteIf(condition, value);
        }

        public static void WriteIf(bool condition, object value, Category category)
        {
            Trace.WriteIf(condition, value, category.ToString());
        }

        #endregion

        #region WriteLineIf

        public static void WriteLineIf(bool condition, string message)
        {
            Trace.WriteLineIf(condition, message);
        }

        public static void WriteLineIf(bool condition, string message, Category category)
        {
            Trace.WriteLineIf(condition, message, category.ToString());
        }

        public static void WriteLineIf(bool condition, object value)
        {
            Trace.WriteLineIf(condition, value);
        }

        public static void WriteLineIf(bool condition, object value, Category category)
        {
            Trace.WriteLineIf(condition, value, category.ToString());
        }

        #endregion
    }
}
