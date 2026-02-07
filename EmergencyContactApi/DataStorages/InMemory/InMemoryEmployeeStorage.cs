using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Models.Results;

namespace EmergencyContactApi.DataStorages.InMemory
{
    public class InMemoryEmployeeStorage : IEmployeeStorage
    {
        private static readonly List<Employee> _employees = new();

        /// <summary>
        /// Service 구현체에서 검증된 paramter로 직원List를 추가.
        /// </summary>
        /// <param name="employees"></param>
        public RegisterResult AddEmployees(List<Employee> employees)
        {
            var successResults = new List<SuccessResult>();
            var failureResults = new List<FailureResult>();

            foreach (var employee in employees)
            {
                bool isDup = _employees.Any(e => string.Equals(e.Name, employee.Name, StringComparison.Ordinal) &&
                                                 string.Equals(e.Email, employee.Email, StringComparison.Ordinal) &&
                                                 string.Equals(e.Tel, employee.Tel, StringComparison.Ordinal) &&
                                                 e.Joined.Date == employee.Joined.Date
                                            );

                if (isDup)
                {
                    failureResults.Add(new FailureResult
                    {
                        Failed = employee,
                        Reason = "이미 등록된 직원정보입니다."
                    });
                    continue;
                }

                _employees.Add(employee);

                successResults.Add(new SuccessResult
                {
                    Succeeded = employee
                });
            }

            return new RegisterResult(successResults, failureResults);
        }

        /// <summary>
        /// 이름으로 직원 정보 조회
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Employee> GetEmployeeByName(string name)
        {
            return _employees.Where(employee => employee.Name.Equals(name, StringComparison.Ordinal)).ToList();
        }

        /// <summary>
        /// page, pageSize로 직원 List 조회
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedResult<Employee> GetEmployeePagedList(int page, int pageSize)
        {
            return new PagedResult<Employee> { 
                Items = _employees.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = _employees.Count
            };
        }
    }
}
