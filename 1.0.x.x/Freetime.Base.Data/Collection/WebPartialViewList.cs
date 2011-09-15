﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Freetime.Base.Data.Collection
{
    [Serializable]
    [XmlRoot("PartialViews",
        Namespace = "http://www.freetime-businessplatform.com",
        IsNullable = false)]
    public class WebPartialViewList : List<Entities.WebPartialView>
    {
    }
}
