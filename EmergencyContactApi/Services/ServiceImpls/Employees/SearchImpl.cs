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

        public ApiResponse<DetailInformationDto> GetEmployeeByName(string name)
        {
            Employee? result = _employeeStorage.GetEmployeeByName(name);
            if(result is null)
            {
                return new ApiResponse<DetailInformationDto>
                {
                    Success = true,
                    Result = null,
                    Error = null
                };
            }

            var dto = new DetailInformationDto(result.Name, result.Email, result.Tel, result.Joined);

            return new ApiResponse<DetailInformationDto>
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
