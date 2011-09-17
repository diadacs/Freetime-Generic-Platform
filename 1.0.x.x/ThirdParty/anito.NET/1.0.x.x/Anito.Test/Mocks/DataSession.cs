using Moq;

namespace Anito.Test.Mocks
{
    public class DataSession : Mock<Data.DataSession>
    {
        private readonly IProvider m_provider;

        public IProvider Provider
        {
            get
            {
                return m_provider;
            }
        }

        public DataSession()
            : this(new IProvider())
        {}

        public DataSession(IProvider provider)
        {
                m_provider = provider;
                Setup(x => x.Provider).Returns(provider.Object);
        }
    }
}
