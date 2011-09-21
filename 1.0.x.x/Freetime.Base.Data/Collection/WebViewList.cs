using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [DataContract]
    [XmlRoot("Views",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class WebViewList : List<Entities.WebView>
    {
    }
}
