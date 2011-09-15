using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Freetime.Web.SessionManagement.MongoDB
{

    internal class StateItemCollection : ISessionStateItemCollection, ICollection, IEnumerable
    {
        private string m_connectionString = null;
        private string m_hostName = string.Empty;
        private string m_catalogName = string.Empty;
        private bool m_isDirty = false;

        public string SessionOwner { get; set; }

        public string ApplicationName { get; private set; }

        private MongoCollection<BsonDocument> CurrentCollection 
        {
            get
            {
                MongoServer server = MongoDBUtil.NewConnection(m_hostName);
                MongoDatabase database = MongoDBUtil.GetSessionStoreDatabase(server, m_catalogName);
                return MongoDBUtil.GetSessionStoreCollection(database, MongoDBUtil.USER_SESSION_ITEM_COLLECTION);
            }
        }

        #region ConnectionString
        //format: "server=mongodb://localhost;catalog=SessionStorage"
        public string ConnectionString 
        {
            get
            {
                return m_connectionString;    
            }
            set
            {

                m_connectionString = value;    
                ParseConnectionString(m_connectionString);
            }
        }

        private void ParseConnectionString(string connectionString)
        {
            string[] splits = connectionString.Split(';');

            if (splits.Count() > 2)
                throw new Exception("connection string not correctly formed");
            
            m_hostName = GetConnectionValue("server", splits[0]);
            if (m_hostName == string.Empty)
            {
                m_hostName = GetConnectionValue("server", splits[1]);
                m_catalogName = GetConnectionValue("catalog", splits[0]);
                return;
            }
            m_catalogName = GetConnectionValue("catalog", splits[1]);            
        }

        private string GetConnectionValue(string name, string uncleanstring)
        {
            uncleanstring = uncleanstring.Replace("=", string.Empty);
            if (uncleanstring.Contains(name))
            {                
                return uncleanstring.Replace(name, string.Empty).Trim();
            }
            else if (uncleanstring.Contains(name))
            {
                return uncleanstring.Replace(name, string.Empty).Trim();
            }
            return string.Empty;
        }
        #endregion


        #region MongoDB
        
        #endregion

   

        public StateItemCollection(string applicationName, 
            string connectionString)
        {            
            ApplicationName = applicationName;
            SessionOwner = Guid.NewGuid().ToString();
            ConnectionString = connectionString;            
        }



        #region ISessionStateItemCollection

        private object GetValue(string key)
        {
            IMongoQuery query = Query.And(Query.EQ("SessionOwner", SessionOwner), Query.EQ("ApplicationName", ApplicationName), Query.EQ("Key", key));           
            SessionElement doc = CurrentCollection.FindOneAs<SessionElement>(query);
            if (doc == null)
                return null;
            return doc.Value;
        }

        private static Dictionary<Type, bool> m_knownTypes = null;
        private static Dictionary<Type, bool> KnownTypes
        {
            get
            {
                if (m_knownTypes == null)
                    m_knownTypes = new Dictionary<Type, bool>();
                return m_knownTypes;
            }
        }

        private void SetValue(string key, object value)
        {            
            //Check Classmap
            //if (!KnownTypes.ContainsKey(value.GetType()) && value.GetType().IsClass)
            //{
            //    BsonClassMap map = BsonClassMap.LookupClassMap(value.GetType());
            //    BsonClassMap.RegisterClassMap(map);
            //    KnownTypes.Add(value.GetType(), true);
            //}
            

            IMongoQuery query = Query.And(Query.EQ("SessionOwner", SessionOwner), Query.EQ("ApplicationName", ApplicationName), Query.EQ("Key", key));

            int count = CurrentCollection.Count(query);
            if (count == 1)
            {                
                SessionElement document = CurrentCollection.FindOneAs<SessionElement>(query);
                document.Value = value;
                CurrentCollection.Save<SessionElement>(document);
            }
            else if (count == 0)
            { 
                SessionElement document = new SessionElement();
                document.SessionOwner = SessionOwner;
                document.ApplicationName = ApplicationName;
                document.Key = key;
                document.Value = value;
                CurrentCollection.Insert<SessionElement>(document);
            }           
          
        }

      
        public object this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                SetValue(key, value);
            }
        }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void ISessionStateItemCollection.Clear()
        {
            IMongoQuery query = Query.And(Query.EQ("SessionOwner", SessionOwner), Query.EQ("ApplicationName", ApplicationName));
            CurrentCollection.Remove(query);
        }

        void ISessionStateItemCollection.Remove(string key)
        {            
            IMongoQuery query = Query.And(Query.EQ("SessionOwner", SessionOwner), Query.EQ("ApplicationName", ApplicationName), Query.EQ("Key", key));
            CurrentCollection.Remove(query);
        }

        void ISessionStateItemCollection.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        bool ISessionStateItemCollection.Dirty
        {
            get
            {
                return m_isDirty;
            }
            set
            {
                m_isDirty = value;
            }
        }

        System.Collections.Specialized.NameObjectCollectionBase.KeysCollection ISessionStateItemCollection.Keys
        {
            get
            {                
                throw new NotImplementedException();  

            }
        }

        #endregion

        #region ICollection

        int ICollection.Count
        {
            get
            {
                IMongoQuery query = Query.And(Query.EQ("SessionOwner", SessionOwner), Query.EQ("ApplicationName", ApplicationName));
                return CurrentCollection.Count(query);
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
        void ICollection.CopyTo(System.Array array, int length)
        {

        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            IMongoQuery query = Query.And(Query.EQ("SessionOwner", SessionOwner), Query.EQ("ApplicationName", ApplicationName));
            var cursor = CurrentCollection.FindAs<SessionElement>(query);
            return cursor.GetEnumerator();
        }

        #endregion

    }
}
