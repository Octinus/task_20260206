using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Models.Results;

namespace EmergencyContactApi.DataStorages.Interfaces
{
    public interface IEmployeeStorage
    {
        PagedResult<Employee> GetEmployeePagedList(int page, int pageSize);
        List<Employee> GetEmployeeByName(string name);
        RegisterResult AddEmployees(List<Employee> employees);
    }
}
