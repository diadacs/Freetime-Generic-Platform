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
    public class Database : IDatabase
    {
        private string m_databaseName = string.Empty;
        private Dictionary<string, ITable> m_databaseTables;

        public string DatabaseName 
        {
            get { return m_databaseName; }
            set { m_databaseName = value; }
        }

        


    }
}
