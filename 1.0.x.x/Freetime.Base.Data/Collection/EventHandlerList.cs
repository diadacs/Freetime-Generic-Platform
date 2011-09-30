using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [DataContract]
    [XmlRoot("EventHandlers",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class EventHandlerList : List<Entities.EventHandler>
    {
        
    }
}
