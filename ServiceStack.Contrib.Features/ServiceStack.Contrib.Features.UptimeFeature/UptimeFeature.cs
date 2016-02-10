using System.Linq;

namespace ServiceStack.Contrib.Features.UptimeFeature
{
    public class DiagnosticServicesFeature : IPlugin
    {
        public DiagnosticServicesFeature()
        {
            
        }

        public void Register(IAppHost appHost)
        {
            var services = GetType().Assembly.GetTypes().Where(a => a.BaseType == typeof(Service)).ToList();

            services.Each(service =>
            {
                appHost.RegisterService(service);
            });
            
        }
    }
}

