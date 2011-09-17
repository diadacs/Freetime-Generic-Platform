using System;
using System.Collections.Generic;
using System.Text;
using Anito.Data;
using Moq;

namespace Anito.Test.Mocks
{
    public class IMapper : Mock<Anito.Data.IMapper>
    {
        public IMapper()
            : this(new IProvider())
        { }

        public IMapper(IProvider provider)
            : this(provider.Object)
        { }
        
        public IMapper(Anito.Data.IProvider provider)
        { 
        
        }


    }
}
