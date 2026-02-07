using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Entity;

namespace EmergencyContactApi.Models.Results
{
    public class FailureResult
    {
        public Employee Failed { get; set; }
        public string Reason { get; set; }
    }
}
