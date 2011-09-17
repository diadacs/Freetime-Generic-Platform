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
    public class Association : Attribute
    {      

        private string m_sourceMember = string.Empty;
        private string m_sourceKey = string.Empty;
        private string m_referenceKey = string.Empty;

        public string SourceMember
        {
            get
            {
                return m_sourceMember;
            }
            set
            {
                m_sourceMember = value;
            }
        }

        public string SourceKey
        {
            get
            {
                return m_sourceKey;
            }
            set
            {
                m_sourceKey = value;
            }
        }

        public string ReferenceKey
        {
            get
            {
                return m_referenceKey;
            }
            set
            {
                m_referenceKey = value;
            }
        }

        public bool ViewOnly { get; set; }

        public Relation Relationship { get; set; }

        public Association(Relation relation, string member, string sourceKey, string referenceKey, bool viewOnly)
        {
            Relationship = relation;
            m_sourceMember = member;
            m_sourceKey = sourceKey;
            m_referenceKey = referenceKey;
            ViewOnly = viewOnly;
        }

        public Association()
        { 
        
        }
    }
}
