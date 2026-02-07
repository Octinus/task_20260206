using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmergencyContactApi.Services.Interfaces.Employees
{
    public interface ISearchService
    {
        ApiResponse<PagedResult<DetailInformationDto>> GetEmployeePagedList(int page, int pageSize);
        ApiResponse<List<DetailInformationDto>> GetEmployeeByName(string name);
    }
}
