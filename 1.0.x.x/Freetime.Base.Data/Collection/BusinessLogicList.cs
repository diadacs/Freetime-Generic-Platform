using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [DataContract]
    [XmlRoot("BusinessLogics",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class BusinessLogicList : List<Entities.BusinessLogic>
    {
    }
}
