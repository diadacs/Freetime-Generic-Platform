/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Exceptions
{
    public class DuplicateConfigurationException : Exception
    {
        private const string ERROR_MESSAGE = "A configuration named {0} already exists.";

        private readonly string m_message = string.Empty;

        public override string Message
        {
            get
            {
                return m_message;
                
            }
        }

        public DuplicateConfigurationException(string configName)
        {
            m_message = string.Format(ERROR_MESSAGE, configName);
        }
    }
}
