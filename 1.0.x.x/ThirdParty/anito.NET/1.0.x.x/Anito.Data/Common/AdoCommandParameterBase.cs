/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Data;

namespace Anito.Data.Common
{
    public abstract class AdoCommandParameterBase : ICommandParameter
    {
        private IDbDataParameter m_parameter;

        public virtual string Name { get; private set; }

        private readonly object m_tempValue;

        public virtual object Value
        {
            get
            {
                return SqlParameter.Value;
            }
            set
            {
                SqlParameter.Value = value;
            }
        }

        public IDbDataParameter SqlParameter
        {
            get
            {
                m_parameter = m_parameter ?? NewSqlParameter(Name, m_tempValue);
                return m_parameter;
            }
        }

        protected abstract IDbDataParameter NewSqlParameter(string parameterName, object value);

        protected AdoCommandParameterBase(ref IDbCommand command, string parameterName)
            : this(ref command, parameterName, null)
        {
        }

        protected AdoCommandParameterBase(ref IDbCommand command, string parameterName, object value)
        {
            Name = parameterName;
            m_tempValue = value;
            command.Parameters.Add(SqlParameter);
        }
    }
}
