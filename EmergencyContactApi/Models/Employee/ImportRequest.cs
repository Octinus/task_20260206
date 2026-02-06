namespace EmergencyContactApi.Models.Employee
{
    public class ImportRequest
    {
        public IFormFile? FormFile { get; set; }
        public string? RawString { get; set; }
    }
}
