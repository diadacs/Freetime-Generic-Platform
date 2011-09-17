/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Data;

namespace Anito.Data.Common
{
    public abstract class AdoCommandBase : ICommand
    {
        private ParameterCollection m_parameters;
        private IDbCommand m_command;

        public virtual IDbConnection Connection { get; set; }

        public virtual ParameterCollection Parameters
        {
            get
            {
                m_parameters = m_parameters ?? new ParameterCollection();
                return m_parameters;
            }
        }

        public virtual IDbCommand SqlCommand
        {
            get
            {
                return m_command;
            }
        }
            
        protected AdoCommandBase(IDbCommand command)
        {
            m_command = command;
        }

        protected abstract ICommandParameter NewCommandParameter(ref IDbCommand command, string parameterName);
        protected abstract ICommandParameter NewCommandParameter(ref IDbCommand command, string parameterName, object value);

        public virtual void AddParam(string parameterName)
        {
            ICommandParameter parameter = NewCommandParameter(ref m_command, parameterName);
            Parameters.Add(parameter.Name, parameter);
        }

        public virtual void AddParamWithValue(string parameterName, object value)
        {
            ICommandParameter parameter = NewCommandParameter(ref m_command, parameterName, value);
            Parameters.Add(parameter.Name, parameter);
        }

    }
}
