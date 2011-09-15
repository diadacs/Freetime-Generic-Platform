using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    public class GeneratedDocumentCode
    {
        [DataField(FieldName = "TransactionType")]
        public string TransactionType { get; set; }
        
        [DataField(FieldName = "Extension")]
        public string Extension { get; set; }

        [DataField(FieldName = "IsSuffix")]
        public bool IsSuffix { get; set; }

        [DataField(FieldName = "Count")]
        public int Count { get; set; }

        [DataField(FieldName = "Separator")]
        public string Separator { get; set; }
    }
}
