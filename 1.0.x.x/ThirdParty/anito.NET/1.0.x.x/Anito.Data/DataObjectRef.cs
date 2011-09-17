/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

namespace Anito.Data
{

    public class DataObjectRef<T> : SessionContainer
        ,ISingleChild
        where T : class
    {
        private T m_dataObject;
       

        public T DataObject
        { 
            get
            {
                if(m_dataObject == null)
                    Load();
                return m_dataObject;
            }
            set
            {
                m_dataObject = value;
            }
        }

        internal DataObjectRef(ISession session)
           : base(session)
        {
             
        }

        protected override void Load()
        {
            if (DataSession == null || KeyValue == null)
                m_dataObject = null;
            else
                m_dataObject = DataSession.GetT<T>(KeyValue);
        }

        object ISingleChild.Child
        {
            get
            {
                return DataObject;
            }
        }
    }
}
