using System;
using System.Collections.Generic;
using System.Text;
using Anito.Data;
using Moq;

namespace Anito.Test.Mocks
{
    public class ICommandBuilder : Mock<Anito.Data.ICommandBuilder>
    {
        public ICommandBuilder()
            : this(new IProvider())
        {}

        public ICommandBuilder(IProvider provider)
            : this(provider.Object)
        {}

        public ICommandBuilder(Anito.Data.IProvider provider)
        {
            Setup(x => x.Provider).Returns(provider);
        }

    }
}
