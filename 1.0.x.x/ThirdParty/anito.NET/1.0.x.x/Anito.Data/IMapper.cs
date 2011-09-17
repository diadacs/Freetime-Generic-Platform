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
    public interface IMapper
    {
        IProvider Provider { get; set; }
        ToDataObjectDelegate<T> GetDataObjectMappingMethod<T>();
        ToDataObjectDelegate GetDataObjectMappingMethod(Type type);

        ToTDelegate<T> GetTMappingMethod<T>();

        ToObjectDelegate GetObjectMappingMethod(Type type);
    }
}
