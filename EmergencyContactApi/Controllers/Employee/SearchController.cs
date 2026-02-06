using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmergencyContactApi.Controllers.Employee
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/employee")]
    [ApiController]
    [Tags("Employee")]
    public class SearchController : ControllerBase
    {
        /// <summary>
        /// 페이징이 가능하도록 전체 직원 데이터를 조회.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees([FromQuery, Required] int page,
                                          [FromQuery, Required] int pageSize) {

            return null;
        }

        /// <summary>
        /// 직원의 이름으로 상세 연락정보를 조회.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public IActionResult GetEmployeeByName([FromRoute,
                                                RegularExpression("^[a-zA-Z가-힣]+$", ErrorMessage = "이름은 한글, 영문만 입력 가능합니다."),
                                                MinLength(2, ErrorMessage = "이름은 최소 2자 이상이어야 합니다.")]
                                                string name) {

            return null;
        }
    }
}
