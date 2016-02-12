using Funq;
using NUnit.Framework;
using ServiceStack.Testing;

namespace ServiceStack.Contrib.TestBase.NUnit
{
    public class AppHostTestBase
    {
        public ServiceStackHost AppHost;
        public Container Container;

        public AppHostTestBase()
        {
            AppHost = new BasicAppHost().Init();
            Container = AppHost.Container;
        }
        
        [OneTimeTearDown]
        public void TestCleanup()
        {
            AppHost.Dispose();
        }
    }
}
