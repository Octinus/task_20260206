using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Helpers;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Models.Request;

namespace EmergencyContactApi.DataStorages.InMemory
{
    public class InMemoryEmployeeStorage : IEmployeeStorage
    {
        private static readonly List<Employee> _employees = new();

        /// <summary>
        /// Service 구현체에서 검증된 paramter로 직원List를 추가.
        /// </summary>
        /// <param name="dtos"></param>
        public void AddEmployees(List<AddDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var employee = new Employee(
                                    dto.Name,
                                    dto.Email,
                                    dto.Tel,
                                    ImportRequestParser.ParseJoined(dto.Joined) 
                                );

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
