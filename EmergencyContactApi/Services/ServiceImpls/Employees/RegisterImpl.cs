using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Helpers;
using EmergencyContactApi.Models.Commons;
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
            bool hasRawString = !string.IsNullOrEmpty(request.RawString);
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

                    if (fileExtsion == AllowedFileExtension.Json)
                    {
                        var addDtos = ImportRequestParser.JsonContentParser(fileContent);
                        result = _employeeStorage.AddEmployees(addDtos);
                    }
                    else
                    {
                        var addDtos = ImportRequestParser.CsvContentParser(fileContent);
                        result = _employeeStorage.AddEmployees(addDtos);
                    }
                }
                else
                {
                    string rawString = request.RawString;
                    
                    if(ImportRequestParser.CheckRawStringFormat(rawString) == AllowedFileExtension.Json)
                    {
                        var addDtos = ImportRequestParser.JsonContentParser(rawString);
                        result = _employeeStorage.AddEmployees(addDtos);
                    }
                    else
                    {
                        var addDtos = ImportRequestParser.CsvContentParser(rawString);
                        result = _employeeStorage.AddEmployees(addDtos);
                    }
                }

                return new ApiResponse<RegisterResult>
                {
                    Success = true,
                    Result = result,
                    Error = null
                };
            }
            catch (JsonException)
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
