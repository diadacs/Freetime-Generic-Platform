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
    public class Procedure: IProcedure
    {
        private string m_procedureName = string.Empty;

        private List<ProcedureParameter> m_parameters = null;

        public List<ProcedureParameter> Parameters
        {
            get
            {
                if (m_parameters == null)
                    m_parameters = new List<ProcedureParameter>();
                return m_parameters;
            }
        }

        public string ProcedureName
        {
            get
            {
                return m_procedureName;
            }
        }

        public Procedure(string name)
        {
            m_procedureName = name;
        }

        public void AddParameter(ProcedureParameter parameter)
        {
            Parameters.Add(parameter);
        }

        public void AddParameter(string parameterName, ParameterType parameterType)
        {
            ProcedureParameter parameter = new ProcedureParameter(parameterName, parameterType);
            Parameters.Add(parameter);
        }

        public void AddParameter(string parameterName, ParameterType parameterType, int size)
        {
            ProcedureParameter parameter = new ProcedureParameter(parameterName, parameterType, null, size);
            Parameters.Add(parameter);
        }

        public void AddParameter(string parameterName, ParameterType parameterType, int size, object value)
        {
            ProcedureParameter parameter = new ProcedureParameter(parameterName, parameterType, value, size);
            Parameters.Add(parameter);
        }

        public void AddParameter(string parameterName, ParameterType parameterType, ParameterDirection direction)
        {
            ProcedureParameter parameter = new ProcedureParameter(parameterName, parameterType, direction);
            Parameters.Add(parameter);
        }

        public void AddParameter(string parameterName, ParameterType parameterType, ParameterDirection direction, object value)
        {
            ProcedureParameter parameter = new ProcedureParameter(parameterName, parameterType, direction, value);
            Parameters.Add(parameter);
        }

        public void AddParameter(string parameterName, ParameterType parameterType, ParameterDirection direction, object value, int size)
        {
            ProcedureParameter parameter = new ProcedureParameter(parameterName, parameterType, direction, value, size);
            Parameters.Add(parameter);
        }

    }
}
