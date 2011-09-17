/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using Anito.Data.Util;

namespace Anito.Data.SqlClient
{
    public class Converter
    {
        public static Int16 ToInt16(object value)
        {
            return Misc.IsDbNull<Int16>(value, default(Int16));
        }

        public static Int32 ToInt32(object value)
        {
            return Misc.IsDbNull<Int32>(value, default(Int32));
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
            return Misc.IsDbNull<Boolean>(value, default(Boolean));
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
