using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Freetime.Web.SessionManagement.MongoDB
{
    internal class UserSession
    {
        [BsonId]
        public string DocumentID { get; set; }

        [BsonElement("SessionID")]
        public string SessionID { get; set; }


        [BsonElement("ApplicationName")]
        public string ApplicationName { get; set; }

        [BsonElement("Created")]
        public DateTime Created { get; set; }

        [BsonElement("Expires")]
        public DateTime Expires { get; set; }

        [BsonElement("LockDate")]
        public DateTime LockDate { get; set; }

        [BsonElement("LockId")]
        public int LockId { get; set; }

        [BsonElement("Timeout")]
        public int Timeout { get; set; }

        [BsonElement("Locked")]
        public bool Locked { get; set; }

        [BsonElement("Flags")]
        public int Flags { get; set; }

        //[BsonElement("DataStore")]
        //public System.Web.SessionState.SessionStateStoreData DataStore { get; set; }

        [BsonElement("ChildIdentifier")]
        public string ChildIdentifier { get; set; }

        public UserSession()
        {
            DocumentID = Guid.NewGuid().ToString();
        }

        private static bool IsClassMapRegistered { get; set; }

        public static void RegisterClassMap()
        {
            if (!IsClassMapRegistered)
            {
                BsonClassMap.RegisterClassMap<UserSession>();
                IsClassMapRegistered = true;
            }
        }
    }
}
