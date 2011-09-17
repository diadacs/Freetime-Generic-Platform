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
    public class ProcedureParameter
    {
        private string m_name = string.Empty;
        private ParameterType m_type;
        private object m_value = null;
        private int m_size = 0;
        private ParameterDirection m_direction = ParameterDirection.Input;

        public string Name 
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        
        }

        public ParameterType Type 
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        
        }

        public object Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }

        public int Size
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
            }
        }

        public ParameterDirection Direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
            }
        }

        public ProcedureParameter(string name)
        {
            Name = name;
        }

        public ProcedureParameter(string name, ParameterType type)
        {
            Name = name;
            Type = type;
        }

        public ProcedureParameter(string name, ParameterType type, object value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public ProcedureParameter(string name, ParameterType type, object value, int size)
        {
            Name = name;
            Type = type;
            Value = value;
            Size = size;
        }

        public ProcedureParameter(string name, ParameterType type, ParameterDirection direction)
        {
            Name = name;
            Type = type;
            Direction = direction;
        }

        public ProcedureParameter(string name, ParameterType type, ParameterDirection direction, object value)
        {
            Name = name;
            Type = type;
            Direction = direction;
            Value = value;
        }

        public ProcedureParameter(string name, ParameterType type, ParameterDirection direction, object value, int size)
        {
            Name = name;
            Type = type;
            Direction = direction;
            Value = value;
            Size = size;
        }
    }
}
