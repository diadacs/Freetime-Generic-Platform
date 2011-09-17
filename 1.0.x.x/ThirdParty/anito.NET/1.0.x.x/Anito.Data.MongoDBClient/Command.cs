using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using System.Data;
using System.Data.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Anito.Data.MongoDBClient
{
    internal class Command : ICommand
    {
        private Anito.Data.ParameterCollection m_parameters = null;
        private List<Query> m_queries = null;


        private Connection MongoConnection { get; set; }
        public string CollectionName { get; set; }

        public List<Query> Queries
        {
            get
            {
                if (m_queries == null)
                    m_queries = new List<Query>();
                return m_queries;
            }
            set
            {
                m_queries = value;
            }
        }

        public IDbConnection Connection 
        {
            get
            {
                return MongoConnection;
            }
            set
            {
                MongoConnection = value as Connection;
            }
        }


        public Anito.Data.ParameterCollection Parameters
        {
            get
            {
                if (m_parameters == null)
                    m_parameters = new ParameterCollection();
                return m_parameters;
            }
        }

        public void AddParam(string parameterName)
        {
            BsonDocument document = Queries[0].Document;
            CommandParameter parameter = new CommandParameter(ref document, parameterName);
            Parameters.Add(parameterName, parameter);
        }

        public void AddParamWithValue(string parameterName, object value)
        {
            BsonDocument document = Queries[0].Document;
            CommandParameter parameter = new CommandParameter(ref document, parameterName, value);
            Parameters.Add(parameterName, parameter);
        }

        public DataReader ExecuteReader()
        {
            var cursor = MongoConnection.MongoDatabase.GetCollection(CollectionName).FindAll();            
            Result result = new Result(cursor);
            ResultSet resultSet = new ResultSet();
            resultSet.Add(result);
            DataReader reader = new DataReader(resultSet);
            return reader;
        }

        public int ExecuteNonQuery()
        {
            foreach(Query query in Queries)
            {
                if (query.Statement == StatementType.INSERT)
                {
                    MongoCollection collection = MongoConnection.MongoDatabase.GetCollection(CollectionName);
                    collection.Insert(query.Document);
                }               
            }
            return 0;
        }
    }
}
