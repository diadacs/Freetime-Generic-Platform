using System;
using System.Collections.Generic;
using System.Text;
using Anito.Data;
using Moq;

namespace Anito.Test.Mocks
{
    public class IProvider : Mock<Anito.Data.IProvider>
    {
        private ICommandBuilder m_commandBuilder;
        private ICommandExecutor m_commandExecutor;
        private IMapper m_mapper;

        public ICommandExecutor CommandExecutor
        {
            get { return m_commandExecutor; }
        }

        public ICommandBuilder CommandBuilder
        {
            get { return m_commandBuilder; }
        }

        public IMapper Mapper
        {
            get { return m_mapper; }
        }

        public IProvider()
        {
            m_commandExecutor = new ICommandExecutor();
            m_commandBuilder = new ICommandBuilder(Object);
            m_mapper = new IMapper(Object);            
            
            Setup(x => x.CommandBuilder).Returns(m_commandBuilder.Object);
            Setup(x => x.CommandExecutor).Returns(m_commandExecutor.Object);
            Setup(x => x.Mapper).Returns(m_mapper.Object);
        }
    }
}
