using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Helpers;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Request;
using EmergencyContactApi.Models.Results;
using EmergencyContactApi.Services.Interfaces.Employees;
using System.Text.Json;

namespace EmergencyContactApi.Services.ServiceImpls.Employees
{
    public class RegisterImpl : IRegisterService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public RegisterImpl(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public ApiResponse<RegisterResult> AddEmployees(ImportRequest request)
        {
            bool hasFile = request.FormFile is not null;
            bool hasRawString = request.RawString is not null;
            try
            {
                if (hasFile && hasRawString)
                {
                    throw new Exception("파일업로드, 직접입력 둘 중 하나만 입력해주세요.");
                }

                if (!hasFile && !hasRawString)
                {
                    throw new Exception("파일업로드, 직접입력 아무것도 없습니다.");
                }

                if (hasFile)
                {
                    string fileName = ImportRequestParser.GetFileName(request.FormFile);
                    string fileExtsion = ImportRequestParser.GetFileFormat(fileName);
                    string fileContent = ImportRequestParser.GetFileContent(request.FormFile);

                    if (string.Equals(fileExtsion, "json"))
                    {
                        List<AddDto> addDtos = JsonSerializer.Deserialize<List<AddDto>>(fileContent, new JsonSerializerOptions {
                                                                                                                                PropertyNameCaseInsensitive = true
                                                                                                                            })!;
                        if (addDtos == null || addDtos.Count == 0)
                            throw new Exception("JSON이 비어 있습니다.");
                        ImportRequestParser.ValidateDtos(addDtos);
                        _employeeStorage.AddEmployees(addDtos);

                        return new ApiResponse<RegisterResult>
                        {
                            Success = true,
                            Result = new RegisterResult(true, addDtos.Count, null),
                            Error = null
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (JsonException jsonEx)
            {
                return new ApiResponse<RegisterResult>
                {
                    Success = false,
                    Result = null,
                    Error = "Json 형식이 맞지 않습니다."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RegisterResult>
                {
                    Success = false,
                    Result = null,
                    Error = ex.Message
                };
            }
        }
    }
}
