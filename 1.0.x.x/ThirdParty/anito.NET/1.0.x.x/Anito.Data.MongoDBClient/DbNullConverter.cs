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

namespace Anito.Data.MongoDBClient
{
    public class DbNullConverter
    {
        public static System.Int16? ToInt16(object value)
        {
            return Misc.IsDbNull<System.Int16?>(value, null);
        }

        public static System.Int32? ToInt32(object value)
        {
            return Misc.IsDbNull<System.Int32?>(value, null);
        }

        public static System.Int64? ToInt64(object value)
        {
            return Misc.IsDbNull<System.Int64?>(value, null);
        }

        public static System.Single? ToSingle(object value)
        {
            return Misc.IsDbNull<System.Single?>(value, null);
        }

        public static System.Boolean? ToBoolean(object value)
        {
            return Misc.IsDbNull<System.Boolean?>(value, null);
        }

        public static System.String ToString(object value)
        {
            return Misc.IsDbNull<System.String>(value, null);
        }

        public static System.DateTime? ToDateTime(object value)
        {
            return Misc.IsDbNull<System.DateTime?>(value, null);
        }

        public static System.Decimal? ToDecimal(object value)
        {
            if (value == DBNull.Value)
                return null;
            return Decimal.Parse(value.ToString());
            
            //return Misc.IsDbNull<System.Decimal?>(temp, null);
        }

        public static System.Double? ToDouble(object value)
        {
            return Misc.IsDbNull<System.Double?>(value, null);
        }

        public static System.Guid? ToGuid(object value)
        {
            return new Guid(value.ToString());
            //return Misc.IsDbNull<System.Guid?>(value, null);
        }

        public static System.Byte ToByte(object value)
        {
            return Misc.IsDbNull<System.Byte>(value, null);
        }

        public static System.Byte[] ToBytes(object value)
        {
            return Misc.IsDbNull<System.Byte[]>(value, null);
        }

    }
}
