namespace EmergencyContactApi.Models.Employees
{
    public class ImportRequest
    {
        public IFormFile? FormFile { get; set; }
        public string? RawString { get; set; }
    }
}
