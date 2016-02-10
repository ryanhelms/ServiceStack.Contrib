using Funq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Testing;

namespace ServiceStack.Contrib.TestBase
{
    public class AppHostTestBase
    {
        protected internal ServiceStackHost AppHost;
        protected internal Container Container;

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
