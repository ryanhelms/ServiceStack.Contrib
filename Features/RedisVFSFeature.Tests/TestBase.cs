using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;

namespace Test
{
    public class AppHostTestBase
    {
        public ServiceStackHost AppHost;
        public Container Container;

        public AppHostTestBase()
        {

        }

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            AppHost = new BasicAppHost().Init();
            Container = AppHost.Container;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            AppHost.Dispose();
        }
    }
}
