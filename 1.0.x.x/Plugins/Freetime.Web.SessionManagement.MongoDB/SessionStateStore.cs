using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Freetime.Web.SessionManagement.MongoDB
{
    public class SessionStateStore : SessionStateStoreProviderBase, IDisposable
    {
        #region Variables
        private string m_hostName = null;
        private string m_catalogName = null;
        private string m_connectionString = null;
        #endregion

        #region CurrentCollection
        private MongoCollection<BsonDocument> CurrentCollection
        {
            get
            {
                MongoServer server = MongoDBUtil.NewConnection(m_hostName);
                MongoDatabase database = MongoDBUtil.GetSessionStoreDatabase(server, m_catalogName);
                return MongoDBUtil.GetSessionStoreCollection(database, MongoDBUtil.USER_SESSION_COLLECTION);
            }
        }
        #endregion

        #region ApplicationName
        public string ApplicationName
        {
            get;
            private set;
        }
        #endregion

        #region ConnectionString
        public string ConnectionString
        {
            get
            {
                return m_connectionString;
            }
            private set
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

        #region Initialize
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            string connectionStringName = config["connectionStringName"];
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            
            UserSession.RegisterClassMap();
            SessionElement.RegisterClassMap();
            base.Initialize(name, config);
        }
        #endregion

        #region CreateUninitializedItem
        public override void CreateUninitializedItem(System.Web.HttpContext context, string id, int timeout)
        {
            UserSession userSession = new UserSession();
            userSession.SessionID = id;
            userSession.ApplicationName = ApplicationName;
            userSession.Created = DateTime.Now.ToUniversalTime();
            userSession.Expires = DateTime.Now.AddMinutes((Double)timeout).ToUniversalTime();
            userSession.LockDate = DateTime.Now.ToUniversalTime();
            userSession.LockId = 0;
            userSession.Timeout = timeout;
            userSession.Locked = false;
            userSession.Flags = 1;
            
            var collection = new StateItemCollection(ApplicationName, ConnectionString);
            userSession.ChildIdentifier = collection.SessionOwner;
            
            var result = CurrentCollection.Insert<UserSession>(userSession);
            if(result != null)
                if (!result.Ok)
                    throw new Exception("");

        }
        private string SessionID { get; set; }
        #endregion

        #region CreateNewStoreData
        public override SessionStateStoreData CreateNewStoreData(System.Web.HttpContext context, int timeout)
        {           
            
            return new SessionStateStoreData(new StateItemCollection(ApplicationName, ConnectionString),
                    SessionStateUtility.GetSessionStaticObjects(context),
                    timeout);            
        }

        private SessionStateStoreData CreateNewStoreData(System.Web.HttpContext context, int timeout, string identifer)
        {
            var collection = new StateItemCollection(ApplicationName, ConnectionString);
            collection.SessionOwner = identifer;
            return new SessionStateStoreData(collection,
                    SessionStateUtility.GetSessionStaticObjects(context),
                    timeout);
        }
        #endregion

        #region InitializeRequest
        public override void InitializeRequest(System.Web.HttpContext context)
        {                

        }
        #endregion

        #region EndRequest
        public override void EndRequest(System.Web.HttpContext context)
        {
            
        }
        #endregion

        #region GetItem
        public override SessionStateStoreData GetItem(System.Web.HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetSessionStoreItem(false, context, id, out locked,
                out lockAge, out lockId, out actions);
        }
        #endregion

        #region GetItemExclusive
        public override SessionStateStoreData GetItemExclusive(System.Web.HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetSessionStoreItem(true, context, id, out locked,
                out lockAge, out lockId, out actions);
        }

        private SessionStateStoreData GetSessionStoreItem(bool lockRecord,
          System.Web.HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {           
            lockAge = TimeSpan.Zero;
            lockId = null;
            locked = false;
            actionFlags = 0;

  

            IMongoQuery query = null;

            UserSession session = null;

           
            query = Query.And(Query.EQ("SessionID", id), Query.EQ("ApplicationName", ApplicationName));

            session = CurrentCollection.FindOneAs<UserSession>(query);

            if (session != null)
            {
                if (session.Locked)
                    return null;

                if (lockRecord)
                    session.Locked = true;
                else
                    session.Locked = false;

                //Check Expiration
                if (session.Expires < DateTime.Now.ToUniversalTime())
                {
                    //Delete Since expired
                    CurrentCollection.Remove(query);
                    return CreateNewStoreData(context, session.Timeout);
                }

                lockId = (int)lockId + 1;


                lockAge = DateTime.Now.ToUniversalTime().Subtract(session.LockDate);
                actionFlags = (SessionStateActions)session.Flags;
                locked = session.Locked;

                session.Flags = 0;
                session.LockId = (int)lockId;
                CurrentCollection.Save<UserSession>(session);

                return CreateNewStoreData(context, session.Timeout, session.ChildIdentifier);
            }
            return CreateNewStoreData(context, 20);
        }       
        
        #endregion

        #region RemoveItem
        public override void RemoveItem(System.Web.HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            if (item != null)
            {
                //var query = Query.And(Query.EQ("SessionID", id), Query.EQ("ApplicationName", ApplicationName), Query.EQ("LockId", (Int32)lockId));
                //CurrentCollection.Remove(query);
            }
        }
        #endregion

        #region SetAndReleaseItemExclusive
        public override void SetAndReleaseItemExclusive(System.Web.HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            if (lockId == null)
                lockId = 0; //Default

            if (newItem)
            {
                UserSession userSession = new UserSession();
                userSession.SessionID = id;
                userSession.ApplicationName = ApplicationName;
                userSession.Created = DateTime.Now.ToUniversalTime();
                userSession.Expires = DateTime.Now.AddMinutes((Double)item.Timeout).ToUniversalTime();
                userSession.LockDate = DateTime.Now.ToUniversalTime();
                userSession.LockId = 0;
                userSession.Timeout = item.Timeout;
                userSession.Locked = false;
                userSession.Flags = 1;
                if (item == null)
                    item = CreateNewStoreData(context, 20);                  
                userSession.ChildIdentifier = (item.Items as StateItemCollection).SessionOwner;               
                CurrentCollection.Insert<UserSession>(userSession);
            }
            else
            {
                var query = Query.And(Query.EQ("SessionID", id), Query.EQ("ApplicationName", ApplicationName), Query.EQ("LockId", (Int32) lockId));
                UserSession userSession = CurrentCollection.FindOneAs<UserSession>(query);
                if (userSession != null)
                {
                    userSession.Expires = DateTime.Now.AddMinutes((Double)item.Timeout).ToUniversalTime();
                    if (item == null)
                        item = CreateNewStoreData(context, 20);                    
                    userSession.Locked = false;
                    userSession.ChildIdentifier = (item.Items as StateItemCollection).SessionOwner;
                    CurrentCollection.Save<UserSession>(userSession);
                }                               
            }
        }
        #endregion

        #region ReleaseItemExclusive
        public override void ReleaseItemExclusive(System.Web.HttpContext context, string id, object lockId)
        {
            var query = Query.And(Query.EQ("SessionID", id), Query.EQ("ApplicationName", ApplicationName), Query.EQ("LockId", (Int32)lockId));
            UserSession userSession = CurrentCollection.FindOneAs<UserSession>(query);

            if (userSession != null)
            {
                userSession.Locked = false;
                userSession.Expires = DateTime.Now.AddMinutes(20).ToUniversalTime();
                
                
                CurrentCollection.Save<UserSession>(userSession);
            }
        }
        #endregion

        #region ResetItemTimeout
        public override void ResetItemTimeout(System.Web.HttpContext context, string id)
        {
            var query = Query.And(Query.EQ("SessionID", id), Query.EQ("ApplicationName", ApplicationName));
            UserSession userSession = CurrentCollection.FindOneAs<UserSession>(query);
            if (userSession != null)
            {
                userSession.Expires = DateTime.Now.AddMinutes(20).ToUniversalTime();
                CurrentCollection.Save<UserSession>(userSession);
            }
        }
        #endregion

        #region SetItemExpireCallback
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {

        }

        #endregion
    }
}
