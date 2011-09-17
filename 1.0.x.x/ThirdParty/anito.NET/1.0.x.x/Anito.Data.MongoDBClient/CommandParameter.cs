using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Anito.Data.MongoDBClient
{
    internal class CommandParameter : Anito.Data.ICommandParameter
    {

        private string m_parameterName = string.Empty;
        private BsonDocument m_document = null;

        public string Name 
        {
            get
            {
                return m_parameterName;
            }        
        }

        public object Value 
        {
            get
            {
                return m_document[Name];   
            }
            set
            {
                m_document[Name] = ToCompatibleBsonValue(value);
            }        
        }


        public CommandParameter(ref BsonDocument document, string parameterName)
        {
            m_parameterName = parameterName;
            m_document = document;
            m_document[parameterName] = BsonNull.Value;   
        }

        public CommandParameter(ref BsonDocument document, string parameterName, object value)
        {
            m_parameterName = parameterName;
            m_document = document;
            m_document[parameterName] = value.ToBson();
        }

        public BsonValue ToCompatibleBsonValue(object value)
        {
            if (value == null)
                return BsonNull.Value;

            Type valueType = value.GetType();
            if (valueType == typeof(Decimal))
                return BsonValue.Create(double.Parse(value.ToString()));
            else if (valueType == typeof(Guid))
                return BsonValue.Create(value.ToString());

            return BsonValue.Create(value);
        }

    }
}
