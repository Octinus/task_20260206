using EmergencyContactApi.Models.EmployeeDto;

namespace EmergencyContactApi.Models.Results
{
    public class FailureResult
    {
        public AddDto FailedDto { get; set; }
        public string Reason { get; set; }
    }
}
