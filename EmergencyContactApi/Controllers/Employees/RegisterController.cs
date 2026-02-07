using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.Request;
using EmergencyContactApi.Models.Results;
using EmergencyContactApi.Services.Interfaces.Employees;
using Microsoft.AspNetCore.Mvc;

namespace EmergencyContactApi.Controllers.Employees
{
    /// <summary>
    /// 직원 등록 관련 API를 제공하는 컨트롤러.
    /// </summary>
    [Route("api/employee")]
    [ApiController]
    [Tags("Employee")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        /// <summary>
        /// csv 또는 json 형식의 파일을 업로드하거나,
        /// textarea에서 직접 입력한 데이터를 통해 직원 정보를 등록합니다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEmployees([FromForm] ImportRequest request)
        {
            //_ = request.RawString;
            ApiResponse<RegisterResult> apiResponse = _registerService.AddEmployees(request);
            if(apiResponse.Success)
                return new ObjectResult(apiResponse) { StatusCode = 201 };
            else
                return new ObjectResult(apiResponse) { StatusCode = 400 };
        }
    }
}
