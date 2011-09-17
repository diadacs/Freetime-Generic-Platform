/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Mapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Source : Attribute
    {
        private string m_viewSource = string.Empty;
        private string m_updateSource = string.Empty;

        public string View
        {
            get
            {
                return m_viewSource;
            }
            set
            {
                m_viewSource = value;
            }
        }

        public string Update
        {
            get
            {
                return m_updateSource;
            }
            set
            {
                m_updateSource = value;
            }
        }

        public Source()
        { 
                
        }

        public Source(string tableName)
            : this(tableName, tableName)
        {
        }

        public Source(string viewSource, string updateSource)
        {
            m_viewSource = viewSource;
            m_updateSource = updateSource;
        }

    }
}
