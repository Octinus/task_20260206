using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.Request;
using EmergencyContactApi.Models.Results;

namespace EmergencyContactApi.Services.Interfaces.Employees
{
    public interface IRegisterService
    {
        ApiResponse<RegisterResult> AddEmployees(ImportRequest request);
    }
}
