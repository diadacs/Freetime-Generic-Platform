using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Anito.Data.MongoDBClient
{
    internal class Query
    {
        private BsonDocument m_document = null;

        public StatementType Statement { get; set; }

        public BsonDocument Document
        {
            get
            {                
                if (m_document == null)
                    m_document = new BsonDocument();
                return m_document;
            }
            set
            {
                m_document = value;
            }
        }
    }
}
