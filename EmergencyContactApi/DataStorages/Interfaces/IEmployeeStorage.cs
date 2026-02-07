using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Models.Results;

namespace EmergencyContactApi.DataStorages.Interfaces
{
    public interface IEmployeeStorage
    {
        List<Employee> GetEmployeePagedList(int page, int pageSize);
        Employee? GetEmployeeInformationByName(string name);
        RegisterResult AddEmployees(List<AddDto> employees);
    }
}
