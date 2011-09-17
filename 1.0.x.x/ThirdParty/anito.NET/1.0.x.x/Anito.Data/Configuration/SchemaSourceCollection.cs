/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Configuration;

namespace Anito.Data.Configuration
{
    public class SchemaSourceCollection : ConfigurationElementCollection
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            var schemaElement = element as SchemaSourceElement;
            if (schemaElement == null) throw new Exception("Unable to cast System.Configuration.ConfigurationElement into Anito.Data.Configuration.SchemaSourceElement");
            return schemaElement.Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SchemaSourceElement();
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Add(SchemaSourceElement element)
        {
            BaseAdd(element);
        }

        public void Remove(SchemaSourceElement element)
        {
            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}
