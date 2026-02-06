namespace EmergencyContactApi.Models.Request
{
    public class ImportRequest
    {
        public IFormFile? FormFile { get; set; }
        public string? RawString { get; set; }
    }
}
