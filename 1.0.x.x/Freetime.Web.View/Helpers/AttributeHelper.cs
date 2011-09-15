using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;

namespace Freetime.Web.View.Helpers
{
    internal class AttributeHelper
    {       
        public static IEnumerable<PropertyValue> GetProperties(object o)
        {
            if (o != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(o);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(o);
                    if (val != null)
                    {
                        yield return new PropertyValue { Name = prop.Name, Value = val };
                    }
                }
            }
        }

        public static string ToString(object o)
        {
            StringBuilder sb = new StringBuilder();
            var attributes = GetProperties(o);
            foreach (PropertyValue value in attributes)
            {
                sb.Append(string.Format("{0}=\"{1}\" ", value.Name, value.Value.ToString()));
            }
            return sb.ToString();
        }

        public static void MergeAttribute(TagBuilder builder, object htmlAttributes)
        {
            var attributes = GetProperties(htmlAttributes);
            foreach (PropertyValue value in attributes)
            {
                builder.MergeAttribute(value.Name, value.Value.ToString());
            }
        }

        public sealed class PropertyValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}
