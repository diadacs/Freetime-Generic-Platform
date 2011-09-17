/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Anito.Data.Schema
{
    [XmlRoot("Schema",
        IsNullable = false)]
    public class TypeSchemaSource : Dictionary<Type, TypeTable>, IXmlSerializable
    {

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //TODO write namespace here
            foreach (var entry in this)
            {
                writer.WriteStartElement("DataObject");
                (entry.Value as IXmlSerializable).WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataObject")
                {
                    var table = new TypeTable();
                    (table as IXmlSerializable).ReadXml(reader);
                    if(!ContainsKey(table.MappedObjectType))
                        Add(table.MappedObjectType, table);
                }
                
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Schema")
                {
                    break;
                }
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }
    }
}
