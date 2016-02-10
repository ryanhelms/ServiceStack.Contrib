using System.Linq;

namespace ServiceStack.Contrib.Features.UptimeFeature
{
    public class DiagnosticServicesFeature : IPlugin
    {
        public DiagnosticServicesFeature() { }

        public void Register(IAppHost appHost)
        {
            // Get all of the services in this assembly that inherit from ServiceStackService
            GetType().Assembly.GetTypes().Where(a => a.BaseType == typeof(Service)).ToList()
                // Register the Service
                .Each(service => appHost.RegisterService(service));
        }
    }
}

