/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using System.Reflection;
using Anito.Data.Util;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Anito.Data.Schema
{
    [Serializable]
    [XmlRoot("Column", 
        Namespace = "http://www.anito-project.com",
        IsNullable = false)]
    public class TypeColumn : IXmlSerializable
    {
        [XmlElementAttribute(ElementName = "IsPrimaryKey")]
        public bool IsPrimaryKey { get; set; }

        [XmlElementAttribute(ElementName = "IsIdentity")]
        public bool IsIdentity { get; set; }

        [XmlElementAttribute(ElementName = "Size")]
        public int Size { get; set; }

        [XmlElementAttribute(ElementName = "Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public Type Type { get; set; }

        [XmlElementAttribute(ElementName = "IsNumeric")]
        public bool IsNumeric { get; set; }

        [XmlElementAttribute(ElementName = "MemberName")]
        public string MemberName { get; set; }

        [XmlElementAttribute(ElementName = "ColumnHolder")]
        public string ColumnHolder { get; set; }

        [XmlElementAttribute(ElementName = "IsNullable")]
        public bool IsNullable { get; set; }

        private bool m_viewOnly;
        private FieldInfo ColumnInfo { get; set; }
        private PropertyInfo PropertyInfo { get; set; }
        private ColumnStructureType m_columnStructureType = ColumnStructureType.Field;

        [XmlIgnore]
        public ColumnStructureType StructureStructureType
        {
            get
            {
                return m_columnStructureType;
            }
        }

        public enum ColumnStructureType
        {
            Property,
            Field,
            Method            
        }

        [XmlElementAttribute(ElementName = "m_viewOnly")]
        public bool ViewOnly
        {
            get
            {
                return m_viewOnly;
            }
            set
            {
                m_viewOnly = value;
            }
        }

        public TypeColumn()
        { }

        public TypeColumn(string columnName, string identifierName, PropertyInfo info, bool viewOnly, 
            bool isPrimaryKey, bool isIdentity, int size)
        {            
            IsPrimaryKey = isPrimaryKey;
            IsIdentity = isIdentity;
            Size = size;
            Name = columnName;
            if (info.PropertyType.Name.ToUpper() == "NULLABLE`1")
            {
                Type = Nullable.GetUnderlyingType(info.PropertyType);
                IsNullable = true;
            }
            else
                Type = info.PropertyType;

            IsNumeric = Misc.IsNumericType(Type);
            PropertyInfo = info;
            MemberName = info.Name;
            ColumnHolder = identifierName;
            m_columnStructureType = ColumnStructureType.Property;
            m_viewOnly = viewOnly;
            
        }

        public TypeColumn(string columnName, string identifierName, FieldInfo info, bool viewOnly,
            bool isPrimaryKey, bool isIdentity, int size)
        {
            IsPrimaryKey = isPrimaryKey;
            IsIdentity = isIdentity;
            Size = size;

            Name = columnName;
            if (info.FieldType.Name.ToUpper() == "NULLABLE`1")
            {
                Type = Nullable.GetUnderlyingType(info.FieldType);
                IsNullable = true;
            }
            else
                Type = info.FieldType;
            IsNumeric = Misc.IsNumericType(Type);
            ColumnInfo = info;
            MemberName = info.Name;
            ColumnHolder = identifierName;
            m_columnStructureType = ColumnStructureType.Field;
            m_viewOnly = viewOnly;
            
        }

        public void SetFieldValue(object item, object value)
        {
            switch (m_columnStructureType)
            {
                case ColumnStructureType.Field:
                    ColumnInfo.SetValue(item, value);
                    break;
                case ColumnStructureType.Property:
                    PropertyInfo.SetValue(item, value, null);
                    break;
            }
        }

        public object GetFieldValue(object item)
        {
            if (m_columnStructureType == ColumnStructureType.Field) return ColumnInfo.GetValue(item);

            return m_columnStructureType == ColumnStructureType.Property ? PropertyInfo.GetValue(item, null)
                : null;
        }

        #region Serializable
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {            
            
            writer.WriteStartElement("DataField");

            writer.WriteStartAttribute("Name");
            writer.WriteValue(Name);
            writer.WriteEndAttribute();

            if (IsPrimaryKey)
            {
                writer.WriteStartAttribute("IsPrimaryKey");
                writer.WriteValue(IsPrimaryKey);
                writer.WriteEndAttribute();
            }

            if (IsIdentity)
            {
                writer.WriteStartAttribute("IsIdentity");
                writer.WriteValue(IsIdentity);
                writer.WriteEndAttribute();
            }

            if (!string.IsNullOrEmpty(ColumnHolder))
            {
                writer.WriteStartAttribute("Property");
                writer.WriteValue(ColumnHolder);
                writer.WriteEndAttribute();
            }

            if (ColumnHolder != MemberName)
            {
                writer.WriteStartAttribute("Storage");
                writer.WriteValue(MemberName);
                writer.WriteEndAttribute();
            }

            if (Size != 0)
            {
                writer.WriteStartAttribute("Size");
                writer.WriteValue(Size);
                writer.WriteEndAttribute();
            }

            if (ViewOnly)
            {
                writer.WriteStartAttribute("m_viewOnly");
                writer.WriteValue(ViewOnly);
                writer.WriteEndAttribute();
            }
            

            writer.WriteEndElement();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {

            var columnName = string.Empty;
            var storage = string.Empty;
            var property = string.Empty;
            var size = 0;
            var isPrimaryKey = false;
            var isIdentity = false;
            var isViewOnly = false;
            while (reader.MoveToNextAttribute())
            {
                switch(reader.Name)
                {
                    case "Name":
                        columnName = reader.Value;
                        break;
                    case "IsPrimaryKey":
                        isPrimaryKey = bool.Parse(reader.Value);
                        break; 
                    case "Storage":
                        storage = reader.Value;
                        break; 
                    case "IsIdentity":
                        isIdentity = bool.Parse(reader.Value);
                        break; 
                    case "Property":
                        property = reader.Value;
                        break; 
                    case "m_viewOnly":
                        isViewOnly = bool.Parse(reader.Value);
                        break;
                    case "Size":
                        size = int.Parse(reader.Value);
                        break;
                }
            }

            if (storage == string.Empty)
                storage = property;

            if (property == string.Empty && storage == string.Empty) throw new Exception("No Place Holder Specified");

            Name = columnName;
            IsPrimaryKey = isPrimaryKey;
            IsIdentity = isIdentity;
            MemberName = storage;
            ColumnHolder = property;
            ViewOnly = isViewOnly;
            Size = size;            
        }

        internal void ReadXml(XmlReader reader, Type type)
        {

            var columnName = string.Empty;
            var storage = string.Empty;
            var property = string.Empty;
            var size = 0;
            var isPrimaryKey = false;
            var isIdentity = false;
            var isViewOnly = false;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Name":
                        columnName = reader.Value;
                        break;
                    case "IsPrimaryKey":
                        isPrimaryKey = bool.Parse(reader.Value);
                        break;
                    case "Storage":
                        storage = reader.Value;
                        break;
                    case "IsIdentity":
                        isIdentity = bool.Parse(reader.Value);
                        break;
                    case "Property":
                        property = reader.Value;
                        break;
                    case "m_viewOnly":
                        isViewOnly = bool.Parse(reader.Value);
                        break;
                    case "Size":
                        size = int.Parse(reader.Value);
                        break;
                }
            }

            if (storage == string.Empty)
                storage = property;

            if (property == string.Empty && storage == string.Empty) throw new Exception("No Place Holder Specified");
                 
                
            IsPrimaryKey = isPrimaryKey;
            IsIdentity = isIdentity;
            Size = size;
            Name = columnName;
            var info = GetMemberInfo(type, storage);
            switch (info.MemberType)
            {
                case MemberTypes.Property:
                    var pInfo = info as PropertyInfo;

                    if (pInfo == null) break; 

                    if (pInfo.PropertyType.Name.ToUpper() == "NULLABLE`1")
                    {
                        Type = Nullable.GetUnderlyingType(pInfo.PropertyType);
                        IsNullable = true;
                    }
                    else
                        Type = pInfo.PropertyType;

                    PropertyInfo = pInfo;
                    m_columnStructureType = ColumnStructureType.Property;

                    break; 
                case MemberTypes.Field:
                    var fInfo = info as FieldInfo;

                    if (fInfo == null) break;

                    if (fInfo.FieldType.Name.ToUpper() == "NULLABLE`1")
                    {
                        Type = Nullable.GetUnderlyingType(fInfo.FieldType);
                        IsNullable = true;
                    }
                    else
                        Type = fInfo.FieldType;

                    ColumnInfo = fInfo;
                    m_columnStructureType = ColumnStructureType.Field;

                    break;
            }
            
            IsNumeric = Misc.IsNumericType(Type);
            MemberName = info.Name;
            ColumnHolder = property;
            ViewOnly = isViewOnly;           
        }


        XmlSchema IXmlSerializable.GetSchema()
        {
            return (null);
        }

        private MemberInfo GetMemberInfo(Type type, string memberName)
        {
            var typeInfos = type.GetMember(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            if (typeInfos.Count() > 0) return typeInfos[0];

            return (type.BaseType != null) ? GetMemberInfo(type, memberName)
                : null; 
        }
        #endregion
    }
}
