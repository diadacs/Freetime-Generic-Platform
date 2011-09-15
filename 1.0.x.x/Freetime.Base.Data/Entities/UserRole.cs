using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    public class UserRole : AuditableEntity
    {
        
        public UserRole()
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
        //        SetField<int>("ID", ref _ID, value);
        //    }
        //}

        //private string _RoleCode;

        //[DataField(FieldName = "RoleCode"
        //    , MemberName = "_RoleCode"
        //    , Size = 30
        //    )]
        //public string RoleCode
        //{
        //    get
        //    {
        //        return _RoleCode;
        //    }
        //    set
        //    {
        //        SetField<string>("RoleCode", ref _RoleCode, value);
        //    }
        //}

        //private string _Description;

        //[DataField(FieldName = "Description"
        //    , MemberName = "_Description"
        //    , Size = 250
        //    )]
        //public string Description
        //{
        //    get
        //    {
        //        return _Description;
        //    }
        //    set
        //    {
        //        SetField<string>("Description", ref _Description, value);
        //    }
        //}

        //private bool _IsDefault;

        //[DataField(FieldName = "IsDefault"
        //    , MemberName = "_IsDefault"
        //    )]
        //public bool IsDefault
        //{
        //    get
        //    {
        //        return _IsDefault;
        //    }
        //    set
        //    {
        //        SetField<bool>("IsDefault", ref _IsDefault, value);
        //    }
        //}

        //private bool _IsActive;

        //[DataField(FieldName = "IsActive"
        //    , MemberName = "_IsActive"
        //    )]
        //public bool IsActive
        //{
        //    get
        //    {
        //        return _IsActive;
        //    }
        //    set
        //    {
        //        SetField<bool>("IsActive", ref _IsActive, value);
        //    }
        //}
    }
}
