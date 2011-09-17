/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

namespace Anito.Data.Events
{
    public delegate void FieldValueChangingDelegate(object sender, FieldValueChangingEventArgs args);
    public delegate void FieldValueChangedDelegate(object sender, FieldValueChangedEventArgs args);
    public delegate void DataObjectOnSaveDelegate(object sender, DataObjectOnSaveEventArgs args);
    public delegate void DataObjectAfterSaveDelegate(object sender, DataObjectAfterSaveArgs args);
    public delegate void DataObjectOnDeleteDelegate(object sender, DataObjectOnDeleteEventArgs args);
    public delegate void DataObjectAfterDeleteDelegate(object sender, DataObjectAfterDeleteEventArgs args);
}
