/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using Anito.Data.Util;

namespace Anito.Data.SqliteClient
{
    public class Converter
    {        
        public static Int16 ToInt16(object value)
        {
            if (value.GetType() != typeof(Int16))
                return short.Parse(Misc.IsNull(value, "0").ToString());
            return (Int16) Misc.IsNull(value, default(Int16));
        }

        public static Int32 ToInt32(object value)
        {
            if(value.GetType() != typeof(Int32))
                return int.Parse(Misc.IsNull(value, "0").ToString());
            return (Int32) Misc.IsNull(value, default(Int32));
        }

        public static Int64 ToInt64(object value)
        {
            return Misc.IsDbNull<Int64>(value, default(Int64));
        }

        public static Single ToSingle(object value)
        {
            return Misc.IsDbNull<Single>(value, default(Single));
        }

        public static Boolean ToBoolean(object value)
        {
            if (value == null)
                return default(Boolean);

            if (value.ToString() == "1")
                return true;
            return default(Boolean);
        }

        public static String ToString(object value)
        {
            return Misc.IsDbNull<String>(value, default(String));
        }

        public static DateTime ToDateTime(object value)
        {
            return Misc.IsDbNull<DateTime>(value, default(DateTime));
        }

        public static Decimal ToDecimal(object value)
        {
            return Misc.IsDbNull<Decimal>(value, default(Decimal));
        }

        public static Double ToDouble(object value)
        {
            return Misc.IsDbNull<Double>(value, default(Double));
        }

        public static Guid ToGuid(object value)
        {
            return Misc.IsDbNull<Guid>(value, default(Guid));
        }

        public static Byte ToByte(object value)
        {
            return Misc.IsDbNull<Byte>(value, default(Byte));
        }

        public static Byte[] ToBytes(object value)
        {
            return Misc.IsDbNull<Byte[]>(value, default(Byte[]));
        }
        
    }
}
