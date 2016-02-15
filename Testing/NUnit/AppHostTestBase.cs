using Funq;
using NUnit.Framework;
using ServiceStack.Testing;

namespace ServiceStack.Contrib.Testing.NUnit
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
            Container = AppHost.Container;
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            AppHost = null;
        }
    }
}
