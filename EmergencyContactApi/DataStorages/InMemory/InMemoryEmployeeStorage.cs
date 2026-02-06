using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Models.Entity;

namespace EmergencyContactApi.DataStorages.InMemory
{
    public class InMemoryEmployeeStorage : IEmployeeStorage
    {
        private static readonly List<Employee> _employees = new();

        /// <summary>
        /// Service 구현체에서 검증된 paramter로 직원List를 추가.
        /// </summary>
        /// <param name="employees"></param>
        public void AddEmployees(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                _employees.Add(employee);
            }
        }

        /// <summary>
        /// 이름으로 직원 정보 조회
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Employee? GetEmployeeInformationByName(string name)
        {
            return _employees.FirstOrDefault(employee => employee.Name.Equals(name, StringComparison.Ordinal));
        }

        /// <summary>
        /// page, pageSize로 직원 List 조회
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Employee> GetEmployeePagedList(int page, int pageSize)
        {
            return _employees.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
