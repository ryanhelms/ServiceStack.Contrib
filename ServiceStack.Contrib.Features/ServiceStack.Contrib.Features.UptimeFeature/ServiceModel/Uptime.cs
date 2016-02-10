namespace ServiceStack.Contrib.Features.UptimeFeature.ServiceModel
{
    [Route("/System/Uptime", Verbs = "GET")]
    public class Uptime { }

    public class UptimeResponse
    {
        public string StartedAt { get; set; }
        public string CurrentTime { get; set; }
        public string Duration { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
