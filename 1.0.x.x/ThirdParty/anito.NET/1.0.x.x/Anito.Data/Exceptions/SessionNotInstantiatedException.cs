/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Exceptions
{
    public class SessionNotInstantiatedException : Exception
    {
        private const string ERROR_MESSAGE = "Session reference not set to an instance of an object";

        public override string Message
        {
            get
            {
                return ERROR_MESSAGE;
                
            }
        }
    }
}
