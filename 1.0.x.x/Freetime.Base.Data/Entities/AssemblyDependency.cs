using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    public class AssemblyDependency : AuditableEntity
    {
        public AssemblyDependency()
            : base()
        { }

        

        [DataField(FieldName = "ID"
            , Identity = true
            , PrimaryKey = true
            )]
        public int ID { get; private set; }


        [DataField(FieldName = "AssemblyID"
            )]
        public int AssemblyID { get; set; }


        [DataField(FieldName = "DependencyID"
            )]
        public int DependencyID { get; set; }
    }
}
