/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Anito.Data.Util;
using Anito.Data.Schema;
using Anito.Data.Common;

namespace Anito.Data.SqlClient
{
    public class CommandBuilder: AdoCommandBuilderBase
    {
        #region Variables
        private static Dictionary<string, Command> m_commandCache;
        private static Dictionary<Type, string> m_columnSelectCache;

        private const string SELECT_ALL_FROM_TABLE = "SELECT * FROM {0}";
        private const string SELECT = "SELECT";
        private const string FROM = "FROM";
        private const string WHERE = "WHERE";
        private const string INSERT = "INSERT";
        private const string INTO = "INTO";
        private const string VALUES = "VALUES";
        private const string UPDATE = "UPDATE";
        private const string DELETE = "DELETE";
        private const string ALL = "*";
        private const string SET = "SET";
        private const string COUNT = "COUNT";
        private const string AS = "AS";

        private const string AND = "AND";
        private const string OR = "OR";
        private const string CACHE_KEY = "{0}_{1}";
        private const string PAGE = "Page";
        private const string PAGE_SIZE = "PageSize";
        private const string OPEN_PARENTHESES = "(";
        private const string CLOSE_PARENTHESES = ")";
        private const string SEMI_COLON = ";";
        private const string SHARP = "#";
        private const string DOT = ".";
        private const string AT = "@";
        private const string PARAM_IDENTIFIER = "@";
        private const string PARAM_IDENTIFIER_ORIGINAL = "@Original_";
        private const string SPACE = " ";
        private const string COMMA = ",";
        private const string EQUALS = "=";
        private const string ASTERISK = "*";
        /*
         * 0 - Keys
         * 1 - Keys
         * 2 - Temp Table
         * 3 - Table
         * 4 - Columns
         * 5 - Table
         * 6 - Temp Table
         * 7 - Join Clause
         * 8 - Temp Table
         * 9 - Temp Table
        */
        private const string GET_TABLE_BY_PAGE = @"     
              IF EXISTS(
                SELECT 1 FROM tempdb.dbo.sysobjects WHERE
                xtype = 'U' AND id = object_id(N'tempdb..{0}')
              )
              BEGIN
                DROP TABLE {1}
              END        
              
              SELECT 
                ROW_NUMBER() OVER(ORDER BY {2}) AS _RowNum,
                {3}
              INTO
                {4}
              FROM
                {5}

              SELECT
                {6}
              FROM
                {7}
              INNER JOIN
                {8}
              ON
                {9}
              WHERE
                {10}._RowNum BETWEEN(@Page - 1) * @PageSize + 1 AND @Page * @PageSize

              DROP TABLE {11};
            ";

        private const string GET_TABLE_BY_PAGE_WHERE = @"     
              IF EXISTS(
                SELECT 1 FROM tempdb.dbo.sysobjects WHERE
                xtype = 'U' AND id = object_id(N'tempdb..{0}')
              )
              BEGIN
                DROP TABLE {1}
              END        
              
              SELECT 
                ROW_NUMBER() OVER(ORDER BY {2}) AS _RowNum,
                {3}
              INTO
                {4}
              FROM
                {5}
              WHERE
                {6}

              SELECT
                {7}
              FROM
                {8}
              INNER JOIN
                {9}
              ON
                {10}
              WHERE
                {11}._RowNum BETWEEN(@Page - 1) * @PageSize + 1 AND @Page * @PageSize

              DROP TABLE {12};
            ";


        #endregion

        #region Properties

        #region CommandCache
        private static Dictionary<string, Command> CommandCache
        {
            get
            {
                m_commandCache = m_commandCache ?? new Dictionary<string, Command>();
                return m_commandCache;
            }
        }
        #endregion

        #region ColumnSelectCache
        private static Dictionary<Type, string> ColumnSelectCache
        {
            get
            {
                m_columnSelectCache = m_columnSelectCache ?? new Dictionary<Type, string>();
                return m_columnSelectCache;
            }
        }
        #endregion

        #endregion

        #region Constructor
        public CommandBuilder(IProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Methods

        #region CreateCommands

        #region CreateGetTCommand
        public override ICommand CreateGetTCommand<T>(Expression expression)
        {
            Type typeT = typeof(T);
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT); 

            Command sqlCommand = new Command();

            Translator translator = new Translator(Provider);
            string whereText = translator.Translate(expression);

            sqlCommand.SqlCommand.CommandText = sqlCommand.SqlCommand.CommandText =
                    string.Format("{0} {1} {2} {3} {4} {5}",
                    SELECT, SelectColumnsStatement(typeT), FROM, schemaTable.ViewSource, WHERE, whereText);            

            return sqlCommand;
        }
        #endregion

        #region CreateGetObjectByKeyCommand
        public override ICommand CreateGetObjectByKeyCommand<T>()
        {
            Type typeT = typeof(T);
            string uniqueId = "4F9C6FBE-2971-46A8-A367-B28AB5F65895";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = new Command();

                var keyList = from column in schemaTable where column.IsPrimaryKey select column;

                if (keyList.Count() < 1)
                    throw new Exceptions.TypeNoKeyException(typeof(T));
                
                StringBuilder paramBuilder = new StringBuilder();
                foreach (Anito.Data.Schema.TypeColumn column in keyList)
                {
                    if (paramBuilder.Length > 0)
                        paramBuilder.Append(string.Format("{0}{1}{2}", SPACE, AND, SPACE));
                    paramBuilder.Append(string.Format("{0} = {1}{2}", column.Name, PARAM_IDENTIFIER, column.Name));
                    sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name));
                }

                sqlCommand.SqlCommand.CommandText =
                    string.Format("{0} {1} {2} {3} {4} {5}",
                    SELECT, SelectColumnsStatement(typeT), FROM, schemaTable.ViewSource, WHERE, paramBuilder.ToString());
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }
        #endregion

        #region CreateGetListCommand
        public override ICommand CreateGetListCommand<T>()
        {
            Type typeT = typeof(T);
            string uniqueId = "232FDED0-6C97-40B4-930F-FC190124D181";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId , typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = new Command();

                sqlCommand.SqlCommand.CommandText = string.Format(
                    "{0} {1} {2} {3}",
                    SELECT, SelectColumnsStatement(typeT), FROM, schemaTable.ViewSource
                    );
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateGetListCommand<T>(IFilterCriteria criteria)
        {
            Type typeT = typeof(T);
            string uniqueId = "232FDED0-6C97-40B4-930F-FC190124D181";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = new Command();                

                sqlCommand.SqlCommand.CommandText = string.Format(
                    "{0} {1} {2} {3}",
                    SELECT, SelectColumnsStatement(typeT), FROM, schemaTable.ViewSource
                    );
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateGetListCommand<T>(Expression expression)
        {
            Type typeT = typeof(T);
        
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

            Translator translator = new Translator(Provider);
            string whereText = translator.Translate(expression);

            Command sqlCommand = new Command();

            sqlCommand.SqlCommand.CommandText = string.Format(
                "{0} {1} {2} {3} {4} {5}",
                SELECT, SelectColumnsStatement(typeT), FROM, schemaTable.ViewSource, WHERE, whereText
                );
            return sqlCommand;
        }
        #endregion

        #region CreateGetListByPageCommand
        public override ICommand CreateGetListByPageCommand<T>()
        {           
            Type typeT = typeof(T);
            string uniqueId = "D89646AE-F93B-4D59-9A0C-7AF5A6B79F5D";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = new Command();

                string tempTable = SHARP + "D89646AE" + schemaTable.ViewSource;

                StringBuilder columnBuilder = new StringBuilder();
                StringBuilder keyBuilder = new StringBuilder();
                StringBuilder keyCompareBuilder = new StringBuilder();
                
                foreach (Anito.Data.Schema.TypeColumn column in schemaTable)
                {
                    if (columnBuilder.Length > 0)
                        columnBuilder.Append(COMMA + SPACE);
                    columnBuilder.Append(schemaTable.ViewSource + DOT + column.Name);

                    if (column.IsPrimaryKey)
                    {
                        if(keyBuilder.Length > 0)
                            keyBuilder.Append(COMMA + SPACE);
                        keyBuilder.Append(column.Name);

                        if (keyCompareBuilder.Length > 0)
                            keyCompareBuilder.Append(SPACE + AND + SPACE);
                        keyCompareBuilder.Append(tempTable + DOT + column.Name);
                        keyCompareBuilder.Append(SPACE + EQUALS + SPACE);
                        keyCompareBuilder.Append(schemaTable.ViewSource + DOT + column.Name);
                    }
                }

                sqlCommand.SqlCommand.CommandText = string.Format(GET_TABLE_BY_PAGE, 
                    tempTable,
                    tempTable,
                    keyBuilder.ToString(), 
                    keyBuilder.ToString(), 
                    tempTable,
                    schemaTable.ViewSource,
                    SelectColumnsStatement(typeT),
                    schemaTable.ViewSource,
                    tempTable,
                    keyCompareBuilder.ToString(),
                    tempTable,
                    tempTable);

                sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, PAGE));
                sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, PAGE_SIZE));

                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateGetListByPageCommand<T>(Expression expression)
        {
            Type typeT = typeof(T);            
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

            Command sqlCommand = new Command();

            string tempTable = SHARP + "D89646AM" + schemaTable.ViewSource;

            StringBuilder columnBuilder = new StringBuilder();
            StringBuilder keyBuilder = new StringBuilder();
            StringBuilder keyCompareBuilder = new StringBuilder();

            foreach (Anito.Data.Schema.TypeColumn column in schemaTable)
            {
                if (columnBuilder.Length > 0)
                    columnBuilder.Append(COMMA + SPACE);
                columnBuilder.Append(schemaTable.ViewSource + DOT + column.Name);

                if (column.IsPrimaryKey)
                {
                    if (keyBuilder.Length > 0)
                        keyBuilder.Append(COMMA + SPACE);
                    keyBuilder.Append(column.Name);

                    if (keyCompareBuilder.Length > 0)
                        keyCompareBuilder.Append(SPACE + AND + SPACE);
                    keyCompareBuilder.Append(tempTable + DOT + column.Name);
                    keyCompareBuilder.Append(SPACE + EQUALS + SPACE);
                    keyCompareBuilder.Append(schemaTable.ViewSource + DOT + column.Name);
                }
            }

            Translator translator = new Translator(Provider);
            string whereText = translator.Translate(expression);

            sqlCommand.SqlCommand.CommandText = string.Format(GET_TABLE_BY_PAGE_WHERE,
                tempTable,
                tempTable,
                keyBuilder.ToString(),
                keyBuilder.ToString(),
                tempTable,
                schemaTable.ViewSource,
                whereText,
                SelectColumnsStatement(typeT),
                schemaTable.ViewSource,
                tempTable,
                keyCompareBuilder.ToString(),
                tempTable,
                tempTable);

            sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, PAGE));
            sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, PAGE_SIZE));

            return sqlCommand;
        }
        #endregion

        #region CreateCommandFromProcedure
        public override ICommand CreateCommandFromProcedure(Procedure procedure)
        {            
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(procedure.ProcedureName);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            for (int i = 0; i < procedure.Parameters.Count; i++)
            {                

                System.Data.Common.DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = PARAM_IDENTIFIER + procedure.Parameters[i].Name;
                parameter.Direction = GetParameterDirection(procedure.Parameters[i].Direction);
                parameter.Value = procedure.Parameters[i].Value;
                parameter.DbType = GetSqlDbType(procedure.Parameters[i].Type);
                parameter.Size = procedure.Parameters[i].Size;
                command.Parameters.Add(parameter);
            }
            Command sqlCommand = new Command(command);
            return sqlCommand;
        }
        #endregion

        #region Read
        public override ICommand CreateReadCommand<T>(Expression expression)
        {
            Translator translator = new Translator(Provider);
            string commandText = translator.Translate(expression);
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(commandText);
            return new Command(command);
        }
        #endregion

        #region Insert

        #region CreateInsertCommand
        public override ICommand CreateInsertCommand(object data)
        {
            Type typeT = data.GetType();
            string uniqueId = "26ABBE10-3D2A-40be-ABAC-F92EF9040608";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = new Command();

                StringBuilder commandTextBuilder = new StringBuilder();
                StringBuilder paramBuilder = new StringBuilder();
                StringBuilder columnBuilder = new StringBuilder();

                TypeColumn identityColumn = null;
                foreach (Schema.TypeColumn column in (from col in schemaTable where !col.ViewOnly select col))
                {
                    if (column.IsIdentity)
                    {
                        identityColumn = column;
                        continue;
                    }

                    if (sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                        continue;

                    if (columnBuilder.Length > 0 && paramBuilder.Length > 0)
                    {
                        columnBuilder.Append(COMMA);
                        paramBuilder.Append(COMMA);
                    }
                    columnBuilder.Append(column.Name);
                    paramBuilder.Append(PARAM_IDENTIFIER + column.Name);
                    if(!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                        sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name));
                }
                commandTextBuilder.Append(string.Format("{0} {1} {2}",
                    INSERT, INTO, schemaTable.UpdateSource));
                commandTextBuilder.Append(OPEN_PARENTHESES);
                commandTextBuilder.Append(columnBuilder.ToString());
                commandTextBuilder.Append(CLOSE_PARENTHESES);
                commandTextBuilder.Append(SPACE);
                commandTextBuilder.Append(VALUES + OPEN_PARENTHESES);
                commandTextBuilder.Append(paramBuilder.ToString());
                commandTextBuilder.Append(CLOSE_PARENTHESES + SEMI_COLON);

                if(identityColumn != null)
                    commandTextBuilder.Append(string.Format("SELECT {0} FROM {1} WHERE {2} = (SELECT IDENT_CURRENT('{3}'));"
                        , schemaTable.ColumnList
                        , schemaTable.ViewSource
                        , identityColumn.Name
                        , schemaTable.ViewSource));
                
                sqlCommand.SqlCommand.CommandText = commandTextBuilder.ToString();               
                
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }
        #endregion

        #endregion

        #region Update

        #region CreateUpdateCommand
        public override ICommand CreateUpdateCommand(object data)
        {
            Type typeT = data.GetType();

            TypeTable schemaTable = Provider.GetSchema(typeT);

            if (schemaTable.HasIdentity)
                return CreateUpdateByIdCommand(data, typeT);
            else if (schemaTable.HasKey)
                return CreateUpdateByKeyCommand(data, typeT);
            return null;
        }

        public override ICommand CreateUpdateCommand<T>(Expression expression)
        {
            Type typeT = typeof(T);

            TypeTable schemaTable = Provider.GetSchema(typeT);

            Command sqlCommand = new Command();

            StringBuilder commandTextBuilder = new StringBuilder();
            StringBuilder paramBuilder = new StringBuilder();

            foreach (Schema.TypeColumn column in (from col in schemaTable where !col.ViewOnly && !col.IsIdentity select col))
            {
                if (sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                    continue;
                if (paramBuilder.Length > 0)
                    paramBuilder.Append(COMMA + SPACE);
                paramBuilder.Append(column.Name + SPACE + EQUALS + SPACE);
                paramBuilder.Append(PARAM_IDENTIFIER + column.Name);
                if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                    sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name));               
            }

            Translator translator = new Translator(Provider);
            string whereClause = translator.Translate(expression);

            commandTextBuilder.Append(string.Format("{0} {1} {2} ",
                UPDATE, schemaTable.UpdateSource, SET
                )
                );
            commandTextBuilder.Append(paramBuilder.ToString());
            commandTextBuilder.Append(SPACE + WHERE + SPACE);
            commandTextBuilder.Append(whereClause);

            sqlCommand.SqlCommand.CommandText = commandTextBuilder.ToString();
            return sqlCommand;
        }
        #endregion

        #region CreateUpdateByKeyCommand
        public override ICommand CreateUpdateByKeyCommand(object data)
        {
            Type typeT = data.GetType();
            string uniqueId = "34FD7357-8114-4f36-825E-77C3F819B39F";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Command sqlCommand = _CreateUpdateByKeyCommand(data, typeT);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateUpdateByKeyCommand(object data, Type type)
        {
            string uniqueId = "34FD7357-8114-4f36-825E-77C3F819B39F";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, type.FullName)))
            {
                Command sqlCommand = _CreateUpdateByKeyCommand(data, type);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, type.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, type.FullName)];
        }

        private Command _CreateUpdateByKeyCommand(object data, Type type)
        {
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(type);

            Command sqlCommand = new Command();

            StringBuilder commandTextBuilder = new StringBuilder();
            StringBuilder paramBuilder = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            foreach (Schema.TypeColumn column in (from col in schemaTable where !col.ViewOnly select col))
            {
                if (column.IsIdentity || sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                    continue;
                if (paramBuilder.Length > 0)
                    paramBuilder.Append(COMMA + SPACE);
                paramBuilder.Append(column.Name + SPACE + EQUALS + SPACE);
                paramBuilder.Append(PARAM_IDENTIFIER + column.Name);
                if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                    sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name));

                if (column.IsPrimaryKey)
                {
                    if (whereBuilder.Length > 0)
                        whereBuilder.Append(SPACE + AND + SPACE);
                    whereBuilder.Append(column.Name);
                    whereBuilder.Append(SPACE);
                    whereBuilder.Append(EQUALS);
                    whereBuilder.Append(SPACE);
                    whereBuilder.Append(PARAM_IDENTIFIER_ORIGINAL + column.Name);
                    if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name)))
                        sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name));
                }
            }
            commandTextBuilder.Append(string.Format("{0} {1} {2}",
                UPDATE, schemaTable.UpdateSource, SET));
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(paramBuilder.ToString());
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(WHERE);
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(whereBuilder.ToString());
            commandTextBuilder.Append(SEMI_COLON);

            sqlCommand.SqlCommand.CommandText = commandTextBuilder.ToString();
            return sqlCommand;
        }
        #endregion

        #region CreateUpdateByIdCommand
        public override ICommand CreateUpdateByIdCommand(object data)
        { 
            Type typeT = data.GetType();
            string uniqueId = "80065C07-24CD-40E8-9642-A46085892480";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Command sqlCommand = _CreateUpdateByIdCommand(data, typeT);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateUpdateByIdCommand(object data, Type type)
        {
            string uniqueId = "80065C07-24CD-40E8-9642-A46085892480";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, type.FullName)))
            {
                Command sqlCommand = _CreateUpdateByIdCommand(data, type);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, type.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, type.FullName)];
        }
        private Command _CreateUpdateByIdCommand(object data, Type type)
        {
            Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(type);

            Command sqlCommand = new Command();

            StringBuilder commandTextBuilder = new StringBuilder();
            StringBuilder paramBuilder = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            foreach (Schema.TypeColumn column in (from col in schemaTable where !col.ViewOnly select col))
            {
                if (sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                    continue;

                if (!column.IsIdentity)
                {
                    if (paramBuilder.Length > 0)
                        paramBuilder.Append(COMMA + SPACE);
                    paramBuilder.Append(column.Name + SPACE + EQUALS + SPACE);
                    paramBuilder.Append(PARAM_IDENTIFIER + column.Name);
                    if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name)))
                        sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER, column.Name));
                }
                else if (column.IsIdentity)
                {
                    if (whereBuilder.Length > 0)
                        whereBuilder.Append(SPACE + AND + SPACE);
                    whereBuilder.Append(column.Name);
                    whereBuilder.Append(SPACE);
                    whereBuilder.Append(EQUALS);
                    whereBuilder.Append(SPACE);
                    whereBuilder.Append(PARAM_IDENTIFIER_ORIGINAL + column.Name);
                    if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name)))
                        sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name));
                }
            }
            commandTextBuilder.Append(string.Format("{0} {1} {2}",
                UPDATE, schemaTable.UpdateSource, SET));
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(paramBuilder.ToString());
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(WHERE);
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(whereBuilder.ToString());
            commandTextBuilder.Append(SEMI_COLON);

            sqlCommand.SqlCommand.CommandText = commandTextBuilder.ToString();
            return sqlCommand;
        }
        #endregion

        #endregion

        #region Delete

        #region CreateDeleteCommand
        public override ICommand CreateDeleteCommand(object data)
        {
            Type typeT = data.GetType();
            TypeTable schemaTable = Provider.GetSchema(typeT);

            if (schemaTable.HasIdentity)
                return CreateDeleteByIdCommand(data, typeT);
            else if (schemaTable.HasKey)
                return CreateDeleteByKeyCommand(data, typeT);
            return null;
        }

        public override ICommand CreateDeleteCommand<T>(Expression expression)
        {
            Type typeT = typeof(T);
            TypeTable schemaTable = Provider.GetSchema(typeT);
            Translator translator = new Translator(Provider);
            string whereClause = translator.Translate(expression);
            string commandText = string.Format("{0} {1} {2} {3} {4}",
                DELETE,
                FROM,
                schemaTable.UpdateSource,
                WHERE,
                whereClause);
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(commandText);
            return new Command(sqlCommand);
        }
        #endregion

        #region CreateDeleteByIdCommand
        public override ICommand CreateDeleteByIdCommand(object data)
        {
            Type typeT = data.GetType();
            string uniqueId = "F58B3FAE-6E43-4B39-9850-8E2D4EC63419";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = _CreateDeletebyIdCommand(data, schemaTable);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateDeleteByIdCommand(object data, Type type)
        {
            string uniqueId = "F58B3FAE-6E43-4B39-9850-8E2D4EC63419";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, type.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(type);

                Command sqlCommand = _CreateDeletebyIdCommand(data, schemaTable);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, type.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, type.FullName)];
        }

        private Command _CreateDeletebyIdCommand(object data, Schema.TypeTable schemaTable)
        {
            Command sqlCommand = new Command();

            StringBuilder commandTextBuilder = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            foreach (Schema.TypeColumn column in (from col in schemaTable where col.IsIdentity && !col.ViewOnly select col))
            {
                if (whereBuilder.Length > 0)
                    whereBuilder.Append(SPACE + AND + SPACE);
                whereBuilder.Append(column.Name);
                whereBuilder.Append(SPACE);
                whereBuilder.Append(EQUALS);
                whereBuilder.Append(SPACE);
                whereBuilder.Append(PARAM_IDENTIFIER_ORIGINAL + column.Name);
                if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name)))
                    sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name));
            }

            commandTextBuilder.Append(string.Format("{0} {1} {2}",
                DELETE, schemaTable.UpdateSource, WHERE));
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(whereBuilder.ToString());
            commandTextBuilder.Append(SEMI_COLON);

            sqlCommand.SqlCommand.CommandText = commandTextBuilder.ToString();
            return sqlCommand;
        }
        #endregion

        #region CreateDeleteByKeyCommand
        public override ICommand CreateDeleteByKeyCommand(object data)
        {
            Type typeT = data.GetType();
            string uniqueId = "92C92A43-D467-4931-8C12-F26AA936E7D7";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);

                Command sqlCommand = _CreateDeletebyKeyCommand(data, schemaTable);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateDeleteByKeyCommand(object data, Type type)
        {
            string uniqueId = "92C92A43-D467-4931-8C12-F26AA936E7D7";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, type.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(type);

                Command sqlCommand = _CreateDeletebyKeyCommand(data, schemaTable);
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, type.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, type.FullName)];
        }

        private Command _CreateDeletebyKeyCommand(object data, Schema.TypeTable schemaTable)
        {
            Command sqlCommand = new Command();

            StringBuilder commandTextBuilder = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            foreach (Schema.TypeColumn column in (from col in schemaTable where col.IsPrimaryKey && !col.ViewOnly select col))
            {
                if (whereBuilder.Length > 0)
                    whereBuilder.Append(SPACE + AND + SPACE);
                whereBuilder.Append(column.Name);
                whereBuilder.Append(SPACE);
                whereBuilder.Append(EQUALS);
                whereBuilder.Append(SPACE);
                whereBuilder.Append(PARAM_IDENTIFIER_ORIGINAL + column.Name);
                if (!sqlCommand.Parameters.ContainsKey(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name)))
                    sqlCommand.AddParam(string.Format("{0}{1}", PARAM_IDENTIFIER_ORIGINAL, column.Name));
            }

            commandTextBuilder.Append(string.Format("{0} {1} {2}",
                DELETE, schemaTable.UpdateSource, WHERE));
            commandTextBuilder.Append(SPACE);
            commandTextBuilder.Append(whereBuilder.ToString());
            commandTextBuilder.Append(SEMI_COLON);

            sqlCommand.SqlCommand.CommandText = commandTextBuilder.ToString();
            return sqlCommand;
        }
        #endregion

        #endregion

        #region Count
        public override ICommand CreateCountCommand(string source)
        {            
            Command sqlCommand = new Command();
            sqlCommand.SqlCommand.CommandText = string.Format("{0} {1}(1) {2} {3}", SELECT, COUNT, FROM, source);
            return sqlCommand;
        }

        public override ICommand CreateCountCommand<T>()
        {
            Type typeT = typeof(T);
            string uniqueId = "1801F51F-A4EC-4A78-8AE8-BCB30402E4B5";
            if (!CommandCache.ContainsKey(string.Format(CACHE_KEY, uniqueId, typeT.FullName)))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(typeT);
                Command sqlCommand = (Command) CreateCountCommand(schemaTable.ViewSource);                
                CommandCache.Add(string.Format(CACHE_KEY, uniqueId, typeT.FullName), sqlCommand);
            }
            return CommandCache[string.Format(CACHE_KEY, uniqueId, typeT.FullName)];
        }

        public override ICommand CreateCountCommand<T>(Expression expression)
        {
            Translator translator = new Translator(Provider);
            Type typeT = typeof(T);
            TypeTable schemaTable = Provider.GetSchema(typeT);
            string commandText = string.Format("{0} {1}(1) {2} {3} {4} {5}", SELECT, 
                COUNT,
                FROM,
                schemaTable.ViewSource,
                WHERE,
                translator.Translate(expression));
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(commandText);
            return new Command(command);
        }
        #endregion

        #endregion

        #region Misc

        private string SelectColumnsStatement(Type type)
        {
            if (!ColumnSelectCache.ContainsKey(type))
            {
                Anito.Data.Schema.TypeTable schemaTable = Provider.GetSchema(type);
                StringBuilder columnSb = new StringBuilder();
                foreach (TypeColumn column in schemaTable)
                {
                    if (columnSb.Length > 0) columnSb.Append(", ");

                    if (column.IsIdentity)
                    {
                        columnSb.Append(string.Format("{0}.{1} AS {2}", schemaTable.ViewSource, column.Name, column.Name));
                    }
                    else
                    {
                        if (!column.IsNullable)
                        {
                            switch (column.Type.Name.ToUpper())
                            {
                                case "INT16":
                                case "INT32":
                                case "INT64":
                                case "SINGLE":
                                case "DATETIME":
                                case "DECIMAL":
                                case "DOUBLE":
                                case "BOOLEAN":
                                    columnSb.Append(string.Format("ISNULL({0}.{1},{2}) AS {3}", schemaTable.ViewSource, column.Name, 0, column.Name));
                                    break;
                                case "STRING":
                                case "BYTE[]":
                                case "BYTE":
                                    columnSb.Append(string.Format("{0}.{1} AS {2}", schemaTable.ViewSource, column.Name, column.Name));
                                    break;
                                case "GUID":
                                    columnSb.Append(string.Format("ISNULL({0}.{1},'{2}') AS {3}", schemaTable.ViewSource, column.Name, default(Guid), column.Name));
                                    break;
                            }
                        }
                        else
                            columnSb.Append(string.Format("{0}.{1} AS {2}", schemaTable.ViewSource, column.Name, column.Name));

                    }
                }
                ColumnSelectCache.Add(type, columnSb.ToString());
            }
            return ColumnSelectCache[type];
        }     

        #endregion

        #endregion
    }
}
