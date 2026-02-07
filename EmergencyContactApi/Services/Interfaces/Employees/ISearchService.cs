using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;

namespace EmergencyContactApi.Services.Interfaces.Employees
{
    public interface ISearchService
    {
        ApiResponse<PagedListDto> GetEmployeePagedList(int page, int pageSize);
        ApiResponse<DetailInformationDto> GetEmployeeByName(string name);
    }
}
