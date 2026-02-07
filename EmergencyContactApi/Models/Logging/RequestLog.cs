namespace EmergencyContactApi.Models.Logging
{
    public class RequestLog
    {
        public string ClientIp { get; set; } = string.Empty;
        public string RequestHost { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string HttpMethod { get; set; } = string.Empty;
        public string UrlParameters { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;

    }
}
