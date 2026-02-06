using EmergencyContactApi.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace EmergencyContactApi.Controllers.Employee
{
    /// <summary>
    /// 직원 등록 관련 API를 제공하는 컨트롤러.
    /// </summary>
    [Route("api/employee")]
    [ApiController]
    [Tags("Employee")]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// csv 또는 json 형식의 파일을 업로드하거나,
        /// textarea에서 직접 입력한 데이터를 통해 직원 정보를 등록합니다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEmployee([FromForm] ImportRequest request)
        {
            return null;
        }
    }
}
