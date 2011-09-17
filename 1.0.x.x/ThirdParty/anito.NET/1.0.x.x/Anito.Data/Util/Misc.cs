/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Util
{
    public class Misc
    {
        private readonly static object s_dBNullValueHolder = DBNull.Value;

        public static Object IsNull(Object objectToCheck)
        {
            return IsNull(objectToCheck, null);
        }

        public static Object IsNull(Object objectToCheck, Object objectReturned)
        {
            if (objectToCheck == null)
                return objectReturned;
            return objectToCheck.Equals(DBNull.Value) 
                ? objectReturned
                : objectToCheck;
        }        

        public static T IsDbNull<T>(Object objectToCheck, Object objectReturned)
        {
            if (s_dBNullValueHolder != objectToCheck)
            {
                if (typeof(T) == typeof(float)) 
                    objectToCheck = Convert.ToSingle(objectToCheck); //temporary fix
                return (T)objectToCheck;
            }
            return (T)objectReturned;
        }

        public static bool IsNullableEnum(Type type)
        {
            var enumType = Nullable.GetUnderlyingType(type);
            return enumType != null && enumType.IsEnum;
        }

        public static bool IsNumericType(Type type)
        {
            return IsNumericType(Type.GetTypeCode(type));                          
        }

        public static bool IsNumericType(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
