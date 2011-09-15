using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Freetime.Web.SessionManagement.MongoDB
{
    internal static class MongoDBUtil
    {
        public const string USER_SESSION_ITEM_COLLECTION = "UserSessionItem";
        public const string USER_SESSION_COLLECTION = "UserSession";

        public static MongoServer NewConnection(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
                throw new Exception("Connection string might have not been initialized.");
            MongoServer server = null;
            try
            {
                server = MongoServer.Create(hostName);
            }
            catch (Exception ex)
            {
                if (server.State != MongoServerState.Disconnected)
                    server.Disconnect();
                throw ex;
            }
            return server;
        }

        public static MongoDatabase GetSessionStoreDatabase(MongoServer server, string catalogName)
        {
            MongoDatabase database = null;
            try
            {
                if (server.State == MongoServerState.Disconnected || server.State == MongoServerState.None)
                    server.Connect();
                database = server.GetDatabase(catalogName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return database;
        }

        public static MongoCollection<BsonDocument> GetSessionStoreCollection(MongoDatabase database, string collectionName)
        {
            MongoCollection<BsonDocument> collection = null;
            try
            {
                collection = database.GetCollection<BsonDocument>(collectionName);//"SessionStoreCollection"
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return collection;
        }
        
    }
}
