/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Events
{
    public class FieldValueChangedEventArgs: EventArgs
    {
        public FieldValueChangedEventArgs(string fieldName, object oldValue, object newValue)
        { 
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string FieldName { get; private set; }

        public object OldValue { get; private set; }

        public object NewValue { get; private set; }
    }
}
