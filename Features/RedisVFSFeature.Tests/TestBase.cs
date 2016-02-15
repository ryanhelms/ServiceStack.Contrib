using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;

namespace Test
{
    [SetUpFixture]
    public class AppHostTestBase
    {
        public ServiceStackHost AppHost;
        public Container Container;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            AppHost = new BasicAppHost().Init();
            var container = AppHost.Container;
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            AppHost = null;
        }
    }
}
