using Funq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Contrib.Features.UptimeFeature.ServiceInterface;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace ServiceStack.Contrib.Features.Uptime.Tests
{
    public class AppHostTestBase
    {
        internal ServiceStackHost AppHost;
        internal Container Container;

        public AppHostTestBase()
        {
            AppHost = new BasicAppHost().Init();
            Container = AppHost.Container;

            Container.Register(new UptimeService());
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            AppHost.Dispose();
        }
    }
}
