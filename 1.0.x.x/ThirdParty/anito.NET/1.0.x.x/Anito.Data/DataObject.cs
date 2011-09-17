/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using Anito.Data.Attributes;
using Anito.Data.Exceptions;
using System.Xml;
using System.IO;

namespace Anito.Data
{
    [Serializable]
    public abstract class DataObject: IDataObject
    {
        #region Variables
        private DataObjectState m_objectState;
        private readonly ISession m_session;
        private bool m_isSaveRequired;
        private Dictionary<string, object> m_keyValues;
        private Dictionary<string, object> m_idValues;
        private static Dictionary<Type, IEnumerable<Schema.TypeColumn>> s_keyCache;
        private static Dictionary<Type, IEnumerable<Schema.TypeColumn>> s_idCache;
        #endregion

        #region Events
        public event Events.FieldValueChangingDelegate OnFieldValueChanging = null;
        public event Events.FieldValueChangedDelegate OnFieldValueChanged = null;
        public event Events.DataObjectOnSaveDelegate OnSave = null;
        public event Events.DataObjectAfterSaveDelegate AfterSave = null;
        public event Events.DataObjectOnDeleteDelegate OnDelete = null;
        public event Events.DataObjectAfterDeleteDelegate AfterDelete = null;
        #endregion

        #region Properties

        #region KeyTypeColumnCache
        [DataObjectDefault]
        [System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.No)]
        private static Dictionary<Type, IEnumerable<Schema.TypeColumn>> KeyTypeColumnCache
        {
            get
            {
                s_keyCache = s_keyCache ?? new Dictionary<Type, IEnumerable<Schema.TypeColumn>>();
                return s_keyCache;
            }
        }
        #endregion

        #region IDTypeColumnCache
        [DataObjectDefault]
        [System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.No)]
        private static Dictionary<Type, IEnumerable<Schema.TypeColumn>> IdTypeColumnCache
        {
            get
            {
                s_idCache = s_idCache ?? new Dictionary<Type, IEnumerable<Schema.TypeColumn>>();
                return s_idCache;
            }
        }
        #endregion

        #region IsSaveRequired
        [DataObjectDefault]
        [System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.No)]
        protected virtual bool IsSaveRequired
        {
            get
            {
                return m_isSaveRequired;
            }
        }
        #endregion

        #region Session
        [DataObjectDefault]
        [System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.No)]
        protected virtual ISession Session
        {
            get
            {
                return m_session;
            }
        }
        #endregion

        #region KeyValues
        [DataObjectDefault]
        protected Dictionary<string, object> KeyValues
        {
            get
            {
                if (m_keyValues == null)
                    PopulateOriginalValues();
                return m_keyValues;
            }
        }
        #endregion

        #region IDValues
        [DataObjectDefault]
        protected Dictionary<string, object> IdValues
        {
            get
            {
                if (m_idValues == null)
                    PopulateOriginalValues();
                return m_idValues;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        protected DataObject()
        {
            m_objectState = DataObjectState.New;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        protected DataObject(ISession session)
        {
            m_objectState = DataObjectState.New;
            m_session = session;
        }
        #endregion        

        #region Save
        /// <summary>
        /// Saves Data
        /// </summary>
        [DataObjectDefault]
        public virtual void Save()
        {
            if (Session == null)
                throw new SessionNotInstantiatedException();
            if (!IsSaveRequired)
                return;

            if (OnSave != null)
                OnSave.Invoke(this, new Events.DataObjectOnSaveEventArgs());
            Session.Save(this);

            if (AfterSave != null)
                AfterSave.Invoke(this, new Events.DataObjectAfterSaveArgs());
            
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Data
        /// </summary>
        [DataObjectDefault]
        public virtual void Delete()
        {
            if (Session == null)
                throw new SessionNotInstantiatedException();

            if (OnDelete != null)
                OnDelete.Invoke(this, new Events.DataObjectOnDeleteEventArgs());
            m_objectState = DataObjectState.Deleted;
            Session.Delete(this);

            if (AfterDelete != null)
                AfterDelete.Invoke(this, new Events.DataObjectAfterDeleteEventArgs());
        }
        #endregion

        #region AcceptChanges
        [DataObjectDefault]
        public void AcceptChanges()
        {
            m_objectState = DataObjectState.Unchanged;
            m_isSaveRequired = false;            
        }
        #endregion

        #region SetField
        [DataObjectDefault]
        protected void SetField<T>(string fieldName, ref T valueHolder, T value)
        {
            var changingArgs = new Events.FieldValueChangingEventArgs(
                fieldName, valueHolder, value);

            if (OnFieldValueChanging != null)
                OnFieldValueChanging.Invoke(this, changingArgs);

            //populating original values should come first than setting values
            if (!m_isSaveRequired)
                PopulateOriginalValues();
            
            valueHolder = (T)changingArgs.NewValue;            

            m_isSaveRequired = true;
            if(m_objectState == DataObjectState.Unchanged)
                m_objectState = DataObjectState.Modified;

            if (OnFieldValueChanged != null)
                OnFieldValueChanged.Invoke(this, new Events.FieldValueChangedEventArgs(fieldName,
                    valueHolder, changingArgs.NewValue));
        }
        #endregion 
   
        #region PopulateOriginalPrimaryKeyValues
        [DataObjectDefault]
        private void PopulateOriginalValues()
        {
            if (!KeyTypeColumnCache.ContainsKey(GetType()) || !IdTypeColumnCache.ContainsKey(GetType()))
            {
                var table = (Session != null) 
                    ? Session.Provider.GetSchema(GetType())
                    : new Schema.TypeTable(GetType());

                var keyList = from column in table where column.IsPrimaryKey select column;
                var idList = from column in table where column.IsIdentity select column;
                KeyTypeColumnCache.Add(GetType(), keyList);
                IdTypeColumnCache.Add(GetType(), idList);
            }
            var keyColumns = KeyTypeColumnCache[GetType()];

            if (m_keyValues == null)
                m_keyValues = new Dictionary<string, object>();
            foreach (var column in keyColumns)
            {
                if (!m_keyValues.ContainsKey(column.Name))
                    m_keyValues.Add(column.Name, column.GetFieldValue(this));
                else
                    m_keyValues[column.Name] = column.GetFieldValue(this);
            }

            var idColumns = IdTypeColumnCache[GetType()];
            if (m_idValues == null)
                m_idValues = new Dictionary<string, object>();
            foreach (var column in idColumns)
            {
                if (!m_idValues.ContainsKey(column.Name))
                    m_idValues.Add(column.Name, column.GetFieldValue(this));
                else
                    m_idValues[column.Name] = column.GetFieldValue(this);
            }

        }
        #endregion

        #region GetOriginalKeyValue
        [DataObjectDefault]
        object IDataObject.GetOriginalKeyValue(string keyField)
        {
            return KeyValues[keyField];
        }
        #endregion

        #region GetOriginalIdValue
        [DataObjectDefault]
        object IDataObject.GetOriginalIdValue(string idField)
        {
            return IdValues[idField];
        }
        #endregion

        #region GetObjectState
        [DataObjectDefault]
        DataObjectState IDataObject.GetObjectState()
        {
            return m_objectState;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            m_keyValues = null;
            m_idValues = null;
        }
        #endregion

        #endregion

        #region Serialization
        public void ToXml(string path)
        { 
        
        }

        public void ToXml(Stream stream)
        { 
        
        }
        
        public void ToXml(XmlWriter writer)
        { 
        
        }
        #endregion

    }
}
