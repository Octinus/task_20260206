using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.Request;
using EmergencyContactApi.Models.Results;
using EmergencyContactApi.Services.Interfaces.Employees;

namespace EmergencyContactApi.Services.ServiceImpls.Employees
{
    public class RegisterImpl : IRegisterService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public RegisterImpl(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public ApiResponse<RegisterResult> AddEmployees(ImportRequest request)
        {
            bool hasFile = request.FormFile is not null;
            bool hasRawString = request.RawString is not null;
            if(hasFile && hasRawString)
            {
                return new ApiResponse<RegisterResult>
                {
                    Success = false,
                    Result = new RegisterResult(false, 0, "파일업로드, 직접입력 둘중 하나만 입력해주세요."),
                    Error = "Too much data provided"
                };
            }
            else if(hasFile && !hasRawString)
            {
                return null;
            }
            else if(hasRawString && !hasFile)
            {
                return null;
            }
            else
            {
                return new ApiResponse<RegisterResult>
                {
                    Success = false,
                    Result = new RegisterResult(false,0,"직원등록에 필요한 정보가 없습니다."),
                    Error = "No data provided"
                };
            }
        }
    }
}
