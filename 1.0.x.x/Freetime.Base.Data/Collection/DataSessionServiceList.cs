using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data.Entities;
using System.Xml;
using System.Xml.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [XmlRoot("Services",
        Namespace = "http://www.freetime-businessplatform.com",
        IsNullable = false)]
    public class DataSessionServiceList : List<DataSessionService>
    {
    }
}
