﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [DataContract]
    [XmlRoot("MasterPages",
        Namespace = "http://www.freetime-generic.com",
        IsNullable = true)]
    public class WebMasterPageList : List<Entities.WebMasterPage>
    {
    }
}
