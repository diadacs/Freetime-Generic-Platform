using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Freetime.Web.SessionManagement.MongoDB
{    
    internal class SessionElement
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        [BsonElement("_id")]        
        public Guid DocumentId { get; private set; }

        [BsonElement("ApplicationName")]
        public string ApplicationName { get; set; }

        [BsonElement("SessionOwner")]
        public string SessionOwner { get; set; }

        [BsonElement("Key")]
        public string Key { get; set; }

        [BsonElement("Value")]
        public object Value { get; set; }

        public SessionElement()
        {
            //DocumentId = Guid.NewGuid().ToString();
        }


        private static bool IsClassMapRegistered { get; set; }
        
        public static void RegisterClassMap()
        {
            if (!IsClassMapRegistered)
            {
                BsonClassMap.RegisterClassMap<SessionElement>();
                IsClassMapRegistered = true;
            }
        }
    }
}


