/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;


namespace Anito.Data.Exceptions
{
    public class TypeNoKeyException: Exception
    {
        private const string ERROR_MESSAGE = "No defined key attribute for type {0}";

        private readonly string m_message = string.Empty;

        public override string Message
        {
            get
            {
                return m_message;
            }
        }

        public TypeNoKeyException(Type type)
        {
            m_message = string.Format(ERROR_MESSAGE, type.FullName);
        }
    }
}
