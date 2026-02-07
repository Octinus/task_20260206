using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Helpers;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Entity;
using EmergencyContactApi.Models.Request;
using EmergencyContactApi.Models.Results;

namespace EmergencyContactApi.DataStorages.InMemory
{
    public class InMemoryEmployeeStorage : IEmployeeStorage
    {
        private static readonly List<Employee> _employees = new();

        /// <summary>
        /// Service 구현체에서 검증된 paramter로 직원List를 추가.
        /// </summary>
        /// <param name="dtos"></param>
        public RegisterResult AddEmployees(List<AddDto> dtos)
        {
            var successResults = new List<SuccessResult>();
            var failureResults = new List<FailureResult>();

            foreach (var dto in dtos)
            {
                DateTime joined;
                try
                {
                    joined = ImportRequestParser.ParseJoined(dto.Joined);
                }
                catch (Exception ex)
                {
                    failureResults.Add(new FailureResult
                    {
                        FailedDto = dto,
                        Reason = ex.Message
                    });
                    continue;
                }

                bool isDup = _employees.Any(e => string.Equals(e.Name, dto.Name, StringComparison.Ordinal) &&
                                                 string.Equals(e.Email, dto.Email, StringComparison.Ordinal) &&
                                                 string.Equals(e.Tel, dto.Tel, StringComparison.Ordinal) &&
                                                 e.Joined.Date == joined.Date
                                            );

                if (isDup)
                {
                    failureResults.Add(new FailureResult
                    {
                        FailedDto = dto,
                        Reason = "이미 등록된 직원정보입니다."
                    });
                    continue;
                }

                var employee = new Employee(dto.Name, dto.Email, dto.Tel, joined);
                _employees.Add(employee);

                successResults.Add(new SuccessResult
                {
                    SucceededDto = dto
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
