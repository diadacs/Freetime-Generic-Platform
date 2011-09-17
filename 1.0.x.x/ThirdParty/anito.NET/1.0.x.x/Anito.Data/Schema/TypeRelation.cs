/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using System.Reflection;
using Anito.Data.Mapping;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Anito.Data.Schema
{
    public class TypeRelation : IXmlSerializable
    {

        public Relation Relation { get; private set;}
        public Type ObjectType { get; private set; }
        public string MemberName { get; private set; }
        public bool ViewOnly { get; private set; }
        public string SourceKey { get; private set; }
        public string ReferenceKey { get; private set; }
        public TypeColumn SourceKeyColumn { get; private set; }

        private MemberInfo m_memberInfo;

        public TypeRelation()
        { 
        
        }
        
        internal TypeRelation(Relation relation, 
            Type objectType, 
            string memberName,
            string sourceKey,
            string referenceKey, MemberInfo memberInfo, TypeColumn sourceKeyColumn)
        {
            Relation = relation;
            ObjectType = objectType;
            MemberName = memberName;
            SourceKey = sourceKey;
            ReferenceKey = referenceKey;
            SourceKeyColumn = sourceKeyColumn;
            m_memberInfo = memberInfo;
        }

        public void SetFieldValue(object item, object value)
        {
            switch (m_memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    var fInfo = m_memberInfo as FieldInfo;
                    if(fInfo == null) break;
                    fInfo.SetValue(item, value);
                    break;
                case MemberTypes.Property:
                    var pInfo = m_memberInfo as PropertyInfo;
                    if(pInfo == null) break;
                    pInfo.SetValue(item, value, null);
                    break;
            } 
        }

        public object GetFieldValue(object item)
        {
            switch (m_memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    var fInfo = m_memberInfo as FieldInfo;
                    if (fInfo == null) break;
                    return fInfo.GetValue(item);
                case MemberTypes.Property:
                    var pInfo = m_memberInfo as PropertyInfo;
                    if (pInfo == null) break;
                    return pInfo.GetValue(item, null);
            }
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var relation = Relation.OneToOne;
            var storage = string.Empty;
            var sourcekey = string.Empty;
            var refkey = string.Empty;
            var viewOnly = false;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Relation":
                        relation = (reader.Value == "OneToOne") ? Relation.OneToOne : Relation.OneToMany;
                        break;
                    case "Storage":
                        storage = reader.Value;
                        break;
                    case "SourceKey":
                        sourcekey = reader.Value;
                        break;
                    case "ReferenceKey":
                        refkey = reader.Value;
                        break;
                    case "ViewOnly":
                        viewOnly = bool.Parse(reader.Value);
                        break;
                } 
            }
            Relation = relation;
            MemberName = storage;
            SourceKey = sourcekey;
            ReferenceKey = refkey;
            ViewOnly = viewOnly;
        }

        internal void ReadXml(XmlReader reader, Type type, TypeTable table)
        {
            var relation = Relation.OneToOne;
            var storage = string.Empty;
            var sourcekey = string.Empty;
            var refkey = string.Empty;
            var viewOnly = false;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Relation":
                        relation = (reader.Value == "OneToOne") ? Relation.OneToOne : Relation.OneToMany;
                        break;
                    case "Storage":
                        storage = reader.Value;
                        break;
                    case "SourceKey":
                        sourcekey = reader.Value;
                        break;
                    case "ReferenceKey":
                        refkey = reader.Value;
                        break;
                    case "ViewOnly":
                        viewOnly = bool.Parse(reader.Value);
                        break;
                }  
            }

            var memberInfo = GetMemberInfo(type, storage);

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Property:
                    var pInfo = memberInfo as PropertyInfo;
                    if(pInfo == null) break;
                    ObjectType = pInfo.PropertyType;
                    break;
                case MemberTypes.Field:
                    var fInfo = memberInfo as FieldInfo;
                    if(fInfo == null) break;
                    ObjectType = fInfo.FieldType;
                    break;
            }
            
                    
            

            Relation = relation;
            MemberName = storage;
            SourceKey = sourcekey;
            ReferenceKey = refkey;
            ViewOnly = viewOnly;
            m_memberInfo = memberInfo;
            
            var column = table.First(col => col.Name == SourceKey);
            if (column != null) SourceKeyColumn = column;
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Association");

            writer.WriteStartAttribute("Relation");
            writer.WriteValue((Relation == Relation.OneToOne) ? "OneToOne" : "OneToMany");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("Storage");
            writer.WriteValue(MemberName);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("SourceKey");
            writer.WriteValue(SourceKey);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("ReferenceKey");
            writer.WriteValue(ReferenceKey);
            writer.WriteEndAttribute();

            if (ViewOnly)
            {
                writer.WriteStartAttribute("ViewOnly");
                writer.WriteValue(ViewOnly);
                writer.WriteEndAttribute();
            }

            writer.WriteEndElement();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        private static MemberInfo GetMemberInfo(Type type, string memberName)
        {
            var typeInfos = type.GetMember(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            if (typeInfos.Count() > 0) return typeInfos[0];
            return (type.BaseType != null) ? GetMemberInfo(type, memberName)
                : null;
        }
    }
}
