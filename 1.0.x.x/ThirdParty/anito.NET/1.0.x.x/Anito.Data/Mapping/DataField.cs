/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;

namespace Anito.Data.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataField : Attribute
    {
        public string FieldName { get; set; }

        public string MemberName { get; set; }

        public bool ViewOnly { get; set; }

        public bool Identity { get; set; }

        public bool PrimaryKey { get; set; }

        public int Size { get; set; }

        public DataField()
        { 
        
        }

        public DataField(string fieldName, string memberName)
        {
            FieldName = fieldName;
            MemberName = memberName;
        }

        public DataField(string fieldName)
        {
            FieldName = fieldName;
            MemberName = null;
        }

        public DataField(string fieldName, bool viewOnly)
        {
            FieldName = fieldName;
            MemberName = null;
            ViewOnly = viewOnly;
        }

        public DataField(string fieldName, string memberName, bool viewOnly)
        {
            FieldName = fieldName;
            MemberName = memberName;
            ViewOnly = viewOnly;
        }
    }
}
