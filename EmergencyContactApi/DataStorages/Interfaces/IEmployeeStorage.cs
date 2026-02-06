using EmergencyContactApi.Models.Entity;

namespace EmergencyContactApi.DataStorages.Interfaces
{
    public interface IEmployeeStorage
    {
        List<Employee> GetEmployeePagedList(int page, int pageSize);
        Employee? GetEmployeeInformationByName(string name);
        void AddEmployees(List<Employee> employees);
    }
}
