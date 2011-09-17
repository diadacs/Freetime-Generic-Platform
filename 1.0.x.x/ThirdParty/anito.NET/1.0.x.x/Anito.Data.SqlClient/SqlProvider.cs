﻿/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Anito.Data.SqlClient
{
    public class SqlProvider : Anito.Data.ProviderBase
    {
        #region Variables
        private Mapper m_mapper = null;
        private CommandBuilder m_builder = null;
        private CommandExecutor m_executor = null;
        #endregion


        private const string PROVIDER_NAME = "Anito.SqlClient";

        #region Properties

        #region ConnectionString
        public override string ConnectionString
        {
            get
            {
                return m_executor.ConnectionString;
            }
            set
            {
                m_executor.ConnectionString = value;
            }
        }
        #endregion

        #region CommandBuilder
        public override ICommandBuilder CommandBuilder
        {
            get
            {
                return m_builder;
            }
        }
        #endregion

        #region Mapper
        public override IMapper Mapper
        {
            get
            {
                return m_mapper;
            }
        }
        #endregion

        #region CommandExecutor
        public override ICommandExecutor CommandExecutor
        {
            get
            {
                return m_executor;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region Constructor
        public SqlProvider() 
            : base(PROVIDER_NAME)
        {
            m_mapper = new Mapper(this);
            m_builder = new CommandBuilder(this);           
            m_executor = new CommandExecutor();
        }
        #endregion

        #region Dispose
        public virtual void Dispose()
        {

        }
        #endregion

        #endregion
    }
}
