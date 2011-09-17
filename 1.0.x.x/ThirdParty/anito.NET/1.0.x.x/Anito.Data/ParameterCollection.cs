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

namespace Anito.Data
{
    public class ParameterCollection: Dictionary<string, ICommandParameter>
    {
        public ICommandParameter this[int index]
        {
            get
            {
                var keys = from key in Keys select key;
                return this[keys.ElementAt(index)];
                
            }
        }
    }
}
