using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;


namespace Freetime.Base.Data.Entities
{
    public class Assembly : AuditableEntity
    {
        
        public Assembly()
            : base()
        {
        }

        //private int _ID;

        //[DataField(FieldName = "ID"
        //    , MemberName = "_ID"
        //    , Identity = true
        //    , PrimaryKey = true
        //    )]
        //public int ID
        //{
        //    get
        //    {
        //        return _ID;
        //    }
        //    set
        //    {
        //        SetField <int>("ID", ref _ID, value);
        //    }
        //}

        //private string _AssemblyName;

        //[DataField(FieldName = "AssemblyName"
        //    , MemberName = "_AssemblyName"
        //    , Size = 250
        //    )]
        //public string AssemblyName
        //{
        //    get
        //    {
        //        return _AssemblyName;
        //    }
        //    set
        //    {
        //        SetField <string>("AssemblyName", ref _AssemblyName, value);
        //    }
        //}

        //private bool _IsCustom;

        //[DataField(FieldName = "IsCustom"
        //    , MemberName = "_IsCustom"
        //    )]
        //public bool IsCustom
        //{
        //    get
        //    {
        //        return _IsCustom;
        //    }
        //    set
        //    {
        //        SetField <bool>("IsCustom", ref _IsCustom, value);
        //    }
        //}

        //private List<AssemblyDependency> m_dependencies = null;
        //public List<AssemblyDependency> Dependencies
        //{
        //    get
        //    {
        //        if (m_dependencies == null)
        //            m_dependencies = Session.GetList<List<AssemblyDependency>, AssemblyDependency>(dependency => dependency.AssemblyID == ID);
        //        return m_dependencies;
        //    }       
        //}
    }
}
