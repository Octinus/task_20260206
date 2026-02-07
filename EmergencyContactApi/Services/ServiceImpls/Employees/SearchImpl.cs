using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Services.Interfaces.Employees;
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
            _employeeStorage.GetEmployeeInformationByName(name);
        }

        public ApiResponse<PagedListDto> GetEmployeePagedList(int page, int pageSize)
        {
            _employeeStorage.GetEmployeePagedList(page, pageSize);
        }
    }
}
