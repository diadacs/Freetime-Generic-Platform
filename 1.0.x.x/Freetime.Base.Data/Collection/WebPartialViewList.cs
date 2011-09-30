using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [DataContract]
    [XmlRoot("PartialViews",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class WebPartialViewList : List<Entities.WebPartialView>
    {
    }
}
