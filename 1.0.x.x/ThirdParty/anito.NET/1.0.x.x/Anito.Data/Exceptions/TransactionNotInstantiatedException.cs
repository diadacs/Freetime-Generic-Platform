﻿/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Exceptions
{
    public class TransactionNotInstantiatedException : Exception
    {
        private const string ERROR_MESSAGE = "No transaction has been instantiated";

        public override string Message
        {
            get
            {
                return ERROR_MESSAGE;
            }
        }
    }
}
