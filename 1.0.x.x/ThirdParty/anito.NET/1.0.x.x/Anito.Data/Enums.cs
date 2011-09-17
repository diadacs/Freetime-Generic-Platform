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

namespace Anito.Data
{
    /// <summary>
    /// Determines the record state
    /// </summary>
    public enum DataObjectState
    { 
        New,
        Modified,
        Deleted,
        Unchanged
    }

    public enum EntityStatus
    { 
        Insert,
        Update,
        Delete,
        Unchanged
    }

    public enum Page
    { 
        First,
        Middle,
        Last
    }

    //public enum ProviderConnectionState
    //{
    //    Open,
    //    Closed
    //}

    public enum ParameterType
    {
        Binary,
        Boolean,
        Byte,
        Currency,
        Int16,
        Int32,
        Int64,
        DateTime,
        Decimal,
        Double,
        Single,
        String,
        UId         
    }

    public enum ParameterDirection
    { 
        Input,
        Output,
        InputOutput,
        Returned
    }

}
