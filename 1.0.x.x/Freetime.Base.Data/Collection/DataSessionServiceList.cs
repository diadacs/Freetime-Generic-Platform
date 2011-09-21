using System;
using System.Collections.Generic;
using Freetime.Base.Data.Entities;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [DataContract]
    [XmlRoot("Services",
        Namespace = "http://www.freeG-businessplatform.com",
        IsNullable = true)]
    public class DataSessionServiceList : List<DataSessionService>
    {
    }
}
