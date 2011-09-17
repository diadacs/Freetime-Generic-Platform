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
using Anito.Data.Schema;

namespace Anito.Data
{
    public interface IProvider
    {
        string ProviderName { get; }
        string ConnectionString { get; set; }
        ICommandBuilder CommandBuilder { get; }
        IMapper Mapper { get; }
        ICommandExecutor CommandExecutor { get; }
        TypeTable GetSchema(Type type);
        EntityStatus GetEntityStatus(object item);
    }
}
