/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Anito.Data.Mapping;

namespace Anito.Data.Schema
{
    [Serializable]
    [XmlRoot("DataObject",
        IsNullable = false)]
    public class TypeTable: List<TypeColumn>, IXmlSerializable
    {
        private bool m_isHasIdentitySet;
        private bool m_isHasKeySet;
        private bool m_hasIdentity;
        private bool m_hasKey;
        private string m_columnList;
        private List<TypeRelation> m_relations;
        private string m_assemblyName = string.Empty;
        private TypeColumn m_identityColumn;

        private string TypeName { get; set; }

        public Type MappedObjectType { get; private set; }

        public List<TypeRelation> Associations
        {
            get
            {
                m_relations = m_relations ?? new List<TypeRelation>();
                return m_relations;
            }
        }

        public string ViewSource { get; set; }
        public string UpdateSource { get; set; }

        public string ColumnList
        {
            get
            {
                if (m_columnList == null)
                {
                    var sb = new StringBuilder();
                    foreach (var column in this)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");
                        sb.Append(column.Name);
                    }
                    m_columnList = sb.ToString();
                }
                return m_columnList;
            }
        }

        public TypeTable()
        { 
        
        }

        public TypeTable(Type type)
        {
            ViewSource = type.Name;
            UpdateSource = type.Name;
            TypeName = type.FullName;
            m_assemblyName = type.Assembly.FullName;

            var tattr = type.GetCustomAttributes(true);
            foreach(var attr in tattr.Where(a => a.GetType() == typeof(Source)))
            {
                var sourceAttribute = attr as Source;
                if (sourceAttribute == null) continue;

                var view = sourceAttribute.View;
                var update = sourceAttribute.Update;

                ViewSource = view == string.Empty ? type.Name : view;
                UpdateSource = update == string.Empty ? type.Name : update;

            }

            var minfoArray = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var info in minfoArray.Where(i => i.GetCustomAttributes(typeof(DataField), true).Length > 0))
            {
                //Get DataFields
                var mattr = info.GetCustomAttributes(typeof(DataField), true);

                var dataField = mattr[0] as DataField;
                if (dataField == null) continue; 


                var columnName = dataField.FieldName;
                var memberName = dataField.MemberName ?? info.Name ;

                var isPrimaryKey = dataField.PrimaryKey;
                var isIdentity = dataField.Identity;
                var size = dataField.Size;

                var memberInfo = GetMemberInfo(type, memberName);

                var viewOnly = dataField.ViewOnly;

                switch(memberInfo.MemberType)
                {
                    case MemberTypes.Property:
                        Add(new TypeColumn(columnName, info.Name, (memberInfo as PropertyInfo), viewOnly, isPrimaryKey, isIdentity, size));
                        break;
                    case MemberTypes.Field:
                        Add(new TypeColumn(columnName, info.Name, (memberInfo as FieldInfo), viewOnly, isPrimaryKey, isIdentity, size));
                        break; 
                }            
            }

            //Get Relations
            foreach (var info in minfoArray.Where(i => i.GetCustomAttributes(typeof(Association), true).Length > 0))
            {
                var rattr = info.GetCustomAttributes(typeof(Association), true);

                var association = rattr[0] as Association;
                if (association == null) continue; 

                var memberInfo = GetMemberInfo(type, association.SourceMember);
                TypeRelation relation = null;
   

                var sourceKeyColumn = this.First(col => col.Name == association.SourceKey);

                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Property:
                        var pInfo = memberInfo as PropertyInfo;
                        if (pInfo == null) break;
                        relation = new TypeRelation(association.Relationship,
                        pInfo.PropertyType,
                        association.SourceMember,
                        association.SourceKey,
                        association.ReferenceKey,
                        memberInfo,
                        sourceKeyColumn);
                        break;
                    case MemberTypes.Field:
                        var fInfo = memberInfo as FieldInfo;
                        if(fInfo == null) break;
                        relation = new TypeRelation(association.Relationship,
                        fInfo.FieldType,
                        association.SourceMember,
                        association.SourceKey,
                        association.ReferenceKey,
                        memberInfo,
                        sourceKeyColumn);
                        break;
                }
                Associations.Add(relation);
            }

            MappedObjectType = type;
        }

        private MemberInfo GetMemberInfo(Type type, string memberName)
        {
            var typeInfos = type.GetMember(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (typeInfos.Count() > 0)
                return typeInfos[0];
            return (type.BaseType != null) ? GetMemberInfo(type, memberName)
                : null;
        }

        public bool HasIdentity
        {
            get
            {                
                if (m_isHasIdentitySet)
                    return m_hasIdentity;
                var res = from column in this where column.IsIdentity select column;
                if (res.Count() > 0)
                {
                    m_hasIdentity = true;
                    m_isHasIdentitySet = true;
                }
                return m_hasIdentity;
            }
        }

        public TypeColumn IdentityColumn
        {
            get
            {
                if (!HasIdentity)
                    return null;
                m_identityColumn = m_identityColumn ?? this.First(col => col.IsIdentity);
                return m_identityColumn;
            }
        }

        public bool HasKey
        {
            get
            {
                if (m_isHasKeySet)
                    return m_hasKey;
                var res = from column in this where column.IsIdentity select column;
                if (res.Count() > 0)
                {
                    m_hasKey = true;
                    m_isHasKeySet = true;
                }
                return m_hasKey;
            }
        }

        public string GetDbColumn(string holderName)
        {
            return (from col in this where col.ColumnHolder == holderName select col.Name).First() 
                ?? holderName;
        }

        private static Dictionary<Type, TypeTable> s_typeSchemaCache;

        private static Dictionary<Type, TypeTable> TypeSchemaCache
        {
            get
            {
                s_typeSchemaCache = s_typeSchemaCache ?? new Dictionary<Type, TypeTable>();
                return s_typeSchemaCache;
            }
        }

        internal static TypeTable NewTypeTable(Type type)
        {
            return new TypeTable(type);
        }

        internal static TypeTable GetTypeTableSchema(Type type)
        {
            if (!TypeSchemaCache.ContainsKey(type))
                TypeSchemaCache.Add(type, NewTypeTable(type));
            return TypeSchemaCache[type];
        }


        #region Serialization
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartAttribute("Type");
            writer.WriteValue(TypeName);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("Assembly");
            writer.WriteValue(m_assemblyName);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("View");
            writer.WriteValue(ViewSource);            
            writer.WriteEndAttribute();
            writer.WriteStartAttribute("Update");
            writer.WriteValue(UpdateSource);
            writer.WriteEndAttribute();

            writer.WriteStartElement("Fields");
            foreach (TypeColumn column in this)
            {
                (column as IXmlSerializable).WriteXml(writer);                
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Associations");
            foreach (TypeRelation relation in Associations)
            {
                (relation as IXmlSerializable).WriteXml(writer);
            }
            writer.WriteEndElement();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {

                //Get table attributes
                TypeName = reader.GetAttribute("Type");
                ViewSource = reader.GetAttribute("View");
                UpdateSource = reader.GetAttribute("Update");
                string assembly = reader.GetAttribute("Assembly");

                //TODO find another way not to include assembly
                if ((ViewSource == null && UpdateSource == null) || assembly == null)
                { 
                    //TODO throw exception here
                }

                if (ViewSource == null && UpdateSource != null)
                    ViewSource = UpdateSource;

                if (UpdateSource == null && ViewSource != null)
                    UpdateSource = ViewSource;

               
                //TODO Add try catch here
                var type = Type.GetType(string.Format("{0}, {1}", TypeName, assembly), true);

                if (type == null) throw new Exception(string.Format("Unknown Type {0}.{1}", assembly, TypeName));

   
                MappedObjectType = type;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Fields")
                    {
                        if (reader.IsEmptyElement)
                            continue;
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataField")
                            {
                                var column = new TypeColumn();
                                column.ReadXml(reader, type);
                                Add(column);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Fields")
                                break;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Associations")
                    {
                        if (reader.IsEmptyElement)
                            continue;
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Association")
                            {
                                var relation = new TypeRelation();                                

                                relation.ReadXml(reader, type, this);
                                Associations.Add(relation);
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Associations")
                                break;
                        
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "DataObject")
                        break;                   
                    
                } 
            }

                           
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return (null);
        }
        
        #endregion
    }
}
