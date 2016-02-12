using Funq;
using ServiceStack.Contrib.Features.RedisVFSFeature.Providers;
using ServiceStack.IO;
using ServiceStack.Redis;

namespace ServiceStack.Contrib.Features.RedisVFSFeature
{
    public class RedisVfsFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();

            container.Register(c => appHost);
            container.Register<IRedisClientsManager>(new PooledRedisClientManager("localhost:6379"));
            container.Register(container.Resolve<IRedisClientsManager>().GetClient());

            var redisVirtualPathProvider = new RedisVirtualPathProvider(appHost);

            container.Register(c => redisVirtualPathProvider);
            container.RegisterAutoWired<RedisVirtualPathProvider>().ReusedWithin(ReuseScope.Default);
            //container.RegisterAutoWiredType(typeof(RedisVirtualPathProvider), ReuseScope.Default);
            //container.AutoWire(redisVirtualPathProvider);
            
            
            var clientManager = container.Resolve<IRedisClientsManager>();
            var redisClient = container.Resolve<IRedisClient>();
            var pathProvider = container.Resolve<RedisVirtualPathProvider>();
            
            ;
        }
    }
}
