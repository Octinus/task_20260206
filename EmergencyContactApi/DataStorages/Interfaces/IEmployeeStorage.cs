using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Models.Results;

namespace EmergencyContactApi.DataStorages.Interfaces
{
    public interface IEmployeeStorage
    {
        PagedResult<Employee> GetEmployeePagedList(int page, int pageSize);
        Employee? GetEmployeeByName(string name);
        RegisterResult AddEmployees(List<AddDto> employees);
    }
}
