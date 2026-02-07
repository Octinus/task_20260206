using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Results;
using EmergencyContactApi.Services.Interfaces.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EmergencyContactApi.Controllers.Employees
{
    /// <summary>
    /// 직원 조회 관련 API를 제공하는 컨트롤러.
    /// </summary>
    [Route("api/employee")]
    [ApiController]
    [Tags("Employee")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// 페이징이 가능하도록 전체 직원 데이터를 조회.
        /// </summary>
        /// <param name="page">조회할 페이지 번호(최소1)</param>
        /// <param name="pageSize">페이지당 조회할 직원수(최소1)</param>
        /// <returns>페이지정보와 요청 목록를 포함한 결과</returns>
        [HttpGet]
        public IActionResult GetEmployeePagedList([FromQuery, Required, Range(1, int.MaxValue, ErrorMessage = "page는 최소 1이상입니다.")] int page,
                                                  [FromQuery, Required, Range(1, int.MaxValue, ErrorMessage = "pageSize는 최소 1이상입니다.")] int pageSize) {

            ApiResponse<PagedResult<DetailInformationDto>> apiResponse = _searchService.GetEmployeePagedList(page, pageSize);
            if (apiResponse.Success)
                return new ObjectResult(apiResponse) { StatusCode = 200 };
            else
                return new ObjectResult(apiResponse) { StatusCode = 400 };
        }

        /// <summary>
        /// 직원의 이름을 기준으로 상세 연락 정보를 조회.
        /// 동일한 이름을 가진 직원이 여러 명인 경우 목록으로 반환.
        /// </summary>
        /// <param name="name">조회할 직원의 이름</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public IActionResult GetEmployeeByName([FromRoute,
                                                RegularExpression("^[a-zA-Z가-힣]+$", ErrorMessage = "이름은 한글, 영문만 입력 가능합니다."),
                                                MinLength(2, ErrorMessage = "이름은 최소 2자 이상이어야 합니다.")]
                                                string name) {

            ApiResponse<List<DetailInformationDto>> apiResponse = _searchService.GetEmployeeByName(name);
            if (apiResponse.Success)
                return new ObjectResult(apiResponse) { StatusCode = 200 };
            else
                return new ObjectResult(apiResponse) { StatusCode = 400 };
        }
    }
}
