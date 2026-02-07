using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Services.Interfaces.Employees;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Xml.Linq;

namespace EmergencyContactApi.Services.ServiceImpls.Employees
{
    public class SearchImpl : ISearchService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public SearchImpl(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public ApiResponse<List<DetailInformationDto>> GetEmployeeByName(string name)
        {
            List<Employee> result = _employeeStorage.GetEmployeeByName(name);
            
            var dto = result.Select(e => new DetailInformationDto(e.Name, e.Email, e.Tel, e.Joined)).ToList();

            return new ApiResponse<List<DetailInformationDto>>
            {
                Success = true,
                Result = dto,
                Error = null
            };
        }

        public ApiResponse<PagedResult<DetailInformationDto>> GetEmployeePagedList(int page, int pageSize)
        {
            var result = _employeeStorage.GetEmployeePagedList(page, pageSize);

            var dto = new PagedResult<DetailInformationDto>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Items = result.Items.Select(e => new DetailInformationDto(e.Name, e.Email, e.Tel, e.Joined)).ToList()
            };


            return new ApiResponse<PagedResult<DetailInformationDto>>
            {
                Success = true,
                Result = dto,
                Error = null
            };
        }
    }
}
