using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Anito.Data.Mapping;
using Anito.Data;

namespace Freetime.Base.Data.Entities.Common
{
    [Serializable]
    public abstract class BaseEntity : IDisposable
    {        

        protected BaseEntity()
        { }

        private readonly int? _ID = null;

        [XmlElementAttribute("ID")]
        [DataField(FieldName = "ID", Identity = true)]
        public virtual int? ID
        {
            get
            {
                return _ID;
            }
        }

        public virtual void Dispose()
        {
        }
        
    }
}
