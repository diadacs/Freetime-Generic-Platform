using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anito.Data;
using Anito.Data.Mapping;
using Freetime.Base.Data.Entities.Common;

namespace Freetime.Base.Data.Entities
{
    [Source(View = "LogicRegistry",
        Update = "LogicRegistry")]
    public class BusinessLogic : BaseEntity
    {
        
        public BusinessLogic()
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

        //private string _Logic;

        //[DataField(FieldName = "Logic"
        //    , MemberName = "_Logic"
        //    , Size = 250
        //    )]
        //public string Logic
        //{
        //    get
        //    {
        //        return _Logic;
        //    }
        //    set
        //    {
        //        SetField<string>("Logic", ref _Logic, value);
        //    }
        //}

        //private string _LogicAssembly;

        //[DataField(FieldName = "LogicAssembly"
        //    , MemberName = "_LogicAssembly"
        //    , Size = 250
        //    )]
        //public string LogicAssembly
        //{
        //    get
        //    {
        //        return _LogicAssembly;
        //    }
        //    set
        //    {
        //        SetField<string>("LogicAssembly", ref _LogicAssembly, value);
        //    }
        //}

        //private string _BaseLogicType;

        //[DataField(FieldName = "BaseLogicType"
        //    , MemberName = "_BaseLogicType"
        //    , Size = 250
        //    )]
        //public string BaseLogicType
        //{
        //    get
        //    {
        //        return _BaseLogicType;
        //    }
        //    set
        //    {
        //        SetField<string>("BaseLogicType", ref _BaseLogicType, value);
        //    }
        //}

        //private string _BaseLogicTypeAssembly;

        //[DataField(FieldName = "BaseLogicTypeAssembly"
        //    , MemberName = "_BaseLogicTypeAssembly"
        //    , Size = 250
        //    )]
        //public string BaseLogicTypeAssembly
        //{
        //    get
        //    {
        //        return _BaseLogicTypeAssembly;
        //    }
        //    set
        //    {
        //        SetField<string>("BaseLogicTypeAssembly", ref _BaseLogicTypeAssembly, value);
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
        //        SetField<bool>("IsCustom", ref _IsCustom, value);
        //    }
        //}
    }
}
