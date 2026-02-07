using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Helpers;
using EmergencyContactApi.Models.Commons;
using EmergencyContactApi.Models.EmployeeDto;
using EmergencyContactApi.Models.Request;
using EmergencyContactApi.Models.Results;
using EmergencyContactApi.Services.Interfaces.Employees;
using System.Text.Json;
using static EmergencyContactApi.Helpers.ImportRequestParser;

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

                RegisterResult result;

                if (hasFile)
                {
                    string fileName = ImportRequestParser.GetFileName(request.FormFile);
                    AllowedFileExtension fileExtsion = ImportRequestParser.GetFileFormat(fileName);
                    string fileContent = ImportRequestParser.GetFileContent(request.FormFile);

                    List<AddDto> addDtos = new();
                    if (fileExtsion == AllowedFileExtension.Json)
                    {
                        var option = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        if (ImportRequestParser.IsJsonArray(fileContent))
                        {
                            addDtos = JsonSerializer.Deserialize<List<AddDto>>(fileContent, option);
                        }
                        else
                        {
                            AddDto addDto = JsonSerializer.Deserialize<AddDto>(fileContent, option);
                            if (addDto == null)
                                throw new Exception("JSON이 비어 있습니다.");

                            addDtos.Add(addDto);
                        }

                        if (addDtos == null || addDtos.Count == 0)
                            throw new Exception("JSON이 비어 있습니다.");

                        ImportRequestParser.ValidateDtos(addDtos);

                        result = _employeeStorage.AddEmployees(addDtos);
                    }
                    else
                    {
                        var csvDtos = fileContent.Replace("\r\n", "\n")
                                                 .Replace("\r", "\n")
                                                 .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        for (int i = 0; i < csvDtos.Length; i++)
                        {
                            var csvDto = csvDtos[i];
                            var csvDtoCol = csvDto.Split(',', StringSplitOptions.TrimEntries);

                            if (csvDtoCol.Length < 4 || csvDtoCol.Length > 4)
                                throw new Exception($"등록 가능한 형식에 맞지 않는 구성입니다. 파일내용을 확인해주세요. ([{i + 1}행] 컬럼수 오류)");

                            var dto = new AddDto
                            {
                                Name = csvDtoCol[0],
                                Email = csvDtoCol[1],
                                Tel = csvDtoCol[2],
                                Joined = csvDtoCol[3]
                            };

                            addDtos.Add(dto);
                        }

                        result = _employeeStorage.AddEmployees(addDtos);
                    }
                }
                else
                {
                    return null;
                }

                return new ApiResponse<RegisterResult>
                {
                    Success = true,
                    Result = result,
                    Error = null
                };
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
