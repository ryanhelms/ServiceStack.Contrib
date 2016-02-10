using System;
using ServiceStack.Contrib.Features.UptimeFeature.ServiceModel;

namespace ServiceStack.Contrib.Features.UptimeFeature.ServiceInterface
{
    public class UptimeService : Service
    {
        public object Any(Uptime request)
        {
            var startTime = ServiceStackHost.Instance.StartedAt;
            var endTime = DateTime.UtcNow.ToUniversalTime();
            var t = TimeSpan.FromMilliseconds(endTime.Subtract(startTime).TotalMilliseconds);

            return new UptimeResponse
            {
                StartedAt = startTime.ToString(),
                CurrentTime = endTime.ToString(),
                Duration = "{0}{1}{2}{3}{4}".Fmt(
                            t.Days == 0 ? "" : string.Concat(t.Days, " Days "),
                            t.Hours == 0 ? "" : string.Concat(t.Hours, " Hours "),
                            t.Minutes == 0 ? "" : string.Concat(t.Minutes, " Minutes "),
                            t.Seconds == 0 ? "" : string.Concat(t.Seconds, " Seconds "),
                            t.Milliseconds == 0 ? "" : string.Concat(t.Milliseconds, " Milliseconds "))
            };
        }
    }
}
