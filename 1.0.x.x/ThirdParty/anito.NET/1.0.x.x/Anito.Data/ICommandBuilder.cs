/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Linq.Expressions;

namespace Anito.Data
{
    public interface ICommandBuilder
    {
        //Provider Namespace TODO : propose a better solution
        IProvider Provider { get; set; }

        //Read
        ICommand CreateGetObjectByKeyCommand<T>();
        ICommand CreateGetTCommand<T>(Expression expression);
        ICommand CreateGetListCommand<T>();
        ICommand CreateGetListCommand<T>(IFilterCriteria criteria);
        ICommand CreateGetListCommand<T>(Expression expression);
        ICommand CreateGetListByPageCommand<T>();
        ICommand CreateGetListByPageCommand<T>(Expression expression);
        ICommand CreateReadCommand<T>(Expression expression);
        
        //Procedure
        ICommand CreateCommandFromProcedure(Procedure procedure);

        //Create, Update, Delete
        ICommand CreateInsertCommand(object data);
        
        ICommand CreateUpdateCommand(object data);
        ICommand CreateUpdateCommand<T>(Expression expression);
        ICommand CreateUpdateByIdCommand(object data);
        ICommand CreateUpdateByKeyCommand(object data);
        
        ICommand CreateDeleteCommand(object data);
        ICommand CreateDeleteCommand<T>(Expression expression);
        ICommand CreateDeleteByIdCommand(object data);        
        ICommand CreateDeleteByKeyCommand(object data);

        //Count
        ICommand CreateCountCommand(string source);
        ICommand CreateCountCommand<T>();
        ICommand CreateCountCommand<T>(Expression expression);

        //Supply Paramters
        void SupplyGetObjectByKeyCommandParameters(ref ICommand command, params object[] keyValues);
        void SupplyGetListByPageCommandParameters(ref ICommand command, int page, int pageSize);
        
        void SupplyInsertCommandParameters(ref ICommand command, object data);

        void SupplyUpdateCommandParameters(ref ICommand command, IDataObject data);
        void SupplyUpdateCommandParameters(ref ICommand command, object data);
        void SupplyUpdateByKeyCommandParameters(ref ICommand command, IDataObject data);        
        void SupplyUpdateByIdCommandParameters(ref ICommand command, IDataObject data);

        void SupplyUpdateCommandWhereParameters(ref ICommand command, object data);


        void SupplyDeleteCommandParameters(ref ICommand command, IDataObject data);
        void SupplyDeleteCommandParameters(ref ICommand commmand, object data);
        void SupplyDeleteByIdCommandParameters(ref ICommand command, IDataObject data);
        void SupplyDeleteByKeyCommandParameters(ref ICommand command, IDataObject data);

	}
}
