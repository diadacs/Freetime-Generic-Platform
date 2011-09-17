/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data.Util;

namespace Anito.Data.SqliteClient
{
    public class DbNullConverter
    {

        public static Int16? ToInt16(object value)
        {
            return Misc.IsDbNull<Int16?>(value, null);
        }

        public static Int32? ToInt32(object value)
        {
            return Misc.IsDbNull<Int32?>(value, null);
        }

        public static Int64? ToInt64(object value)
        {
            return Misc.IsDbNull<Int64?>(value, null);
        }

        public static Single? ToSingle(object value)
        {
            return Misc.IsDbNull<Single?>(value, null);
        }

        public static Boolean? ToBoolean(object value)
        {
            if (value == null)
                return default(Boolean);

            if (value.GetType() == typeof(byte[]))
            {
                var arrayByte = value as byte[];

                if (arrayByte != null && arrayByte.Count() > 0)
                {
                    if (arrayByte[0] == 49) return true;
                    return false;
                }
            }

            if (value.ToString() == "1")
                return true;

            return default(Boolean);
        }

        public static String ToString(object value)
        {
            return Misc.IsDbNull<String>(value, null);
        }

        public static DateTime? ToDateTime(object value)
        {
            return Misc.IsDbNull<DateTime?>(value, null);
        }

        public static Decimal? ToDecimal(object value)
        {
            return Misc.IsDbNull<Decimal?>(value, null);
        }

        public static Double? ToDouble(object value)
        {
            return Misc.IsDbNull<Double?>(value, null);
        }

        public static Guid? ToGuid(object value)
        {
            return Misc.IsDbNull<Guid?>(value, null);
        }

        public static Byte ToByte(object value)
        {
            return Misc.IsDbNull<Byte>(value, null);
        }

        public static Byte[] ToBytes(object value)
        {
            return Misc.IsDbNull<Byte[]>(value, null);
        }

    }
}
