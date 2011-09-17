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
    public class Converter
    {
        public static System.Int16 ToInt16(object value)
        {
            return Misc.IsDbNull<System.Int16>(value, default(System.Int16));
        }

        public static System.Int32 ToInt32(object value)
        {
            return Misc.IsDbNull<System.Int32>(value, default(System.Int32));
        }

        public static System.Int64 ToInt64(object value)
        {
            return Misc.IsDbNull<System.Int64>(value, default(System.Int64));
        }

        public static System.Single ToSingle(object value)
        {
            return Misc.IsDbNull<System.Single>(value, default(System.Single));
        }

        public static System.Boolean ToBoolean(object value)
        {
            return Misc.IsDbNull<System.Boolean>(value, default(System.Boolean));
        }

        public static System.String ToString(object value)
        {
            return Misc.IsDbNull<System.String>(value, default(System.String));
        }

        public static System.DateTime ToDateTime(object value)
        {
            return Misc.IsDbNull<System.DateTime>(value, default(System.DateTime));
        }

        public static System.Decimal ToDecimal(object value)
        {
            return Misc.IsDbNull<System.Decimal>(value, default(System.Decimal));
        }

        public static System.Double ToDouble(object value)
        {
            return Misc.IsDbNull<System.Double>(value, default(System.Double));
        }

        public static System.Guid ToGuid(object value)
        {
            return Misc.IsDbNull<System.Guid>(value, default(Guid));
        }

        public static System.Byte ToByte(object value)
        {
            return Misc.IsDbNull<System.Byte>(value, default(System.Byte));
        }

        public static System.Byte[] ToBytes(object value)
        {
            return Misc.IsDbNull<System.Byte[]>(value, default(System.Byte[]));
        }
        
    }
}
