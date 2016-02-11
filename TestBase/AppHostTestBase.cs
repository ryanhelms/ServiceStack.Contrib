using Funq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Testing;

namespace ServiceStack.Contrib.TestBase
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
        
        [TestCleanup]
        public void TestCleanup()
        {
            AppHost.Dispose();
        }
    }
}
