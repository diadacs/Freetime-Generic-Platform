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
using System.Data;
using System.Data.Common;

namespace Anito.Data
{
    public delegate T ToTDelegate<T>(DbDataReader reader);
    public delegate object ToObjectDelegate(DbDataReader reader);

    public delegate T ToDataObjectDelegate<T>(DbDataReader reader, ISession session);
    public delegate IDataObject ToDataObjectDelegate(DbDataReader reader, ISession session);

    internal delegate object CreateISessionContainerDelegate(ISession session);
    internal delegate IProvider CreateIProviderDelegate();
}
