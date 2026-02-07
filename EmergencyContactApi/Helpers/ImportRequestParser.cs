using EmergencyContactApi.Models.EmployeeDto;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace EmergencyContactApi.Helpers
{
    public static class ImportRequestParser
    {
        /// <summary>
        /// 허용된 파일의 확장자
        /// </summary>
        public enum AllowedFileExtension
        {
            Json,
            Csv
        }

        #region 업로드된 파일 정보 관련
        /// <summary>
        /// 업로드된 파일이름 반환.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileName(IFormFile file)
        {
            return file.FileName;
        }

        /// <summary>
        /// 업로드된 파일의 확장자 반환.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static AllowedFileExtension GetFileFormat(string? fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new NullReferenceException("파일명이 없습니다.");

                string extension = Path.GetExtension(fileName);
                if (string.Equals(extension, ".json", StringComparison.OrdinalIgnoreCase))
                    return AllowedFileExtension.Json;

                if (string.Equals(extension, ".csv", StringComparison.OrdinalIgnoreCase))
                    return AllowedFileExtension.Csv;
                throw new ArgumentException("지원하지 않는 확장자의 파일입니다.");
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 업로드된 파일의 내용을 문자열로 반환.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileContent(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                string content = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(content))
                    throw new Exception("파일에 아무 내용이 없습니다.");

                return content;
            }
        }

        #endregion

        #region 직원등록을 위한 파싱 관련
        /// <summary>
        /// 업로드된 파일 내용 기반의 DTO의 유효성 검증.
        /// </summary>
        /// <param name="dtos"></param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidateDtos(List<AddDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var results = new List<ValidationResult>();
                var ctx = new ValidationContext(dto);

                if (!Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true))
                {
                    var firstError = results.First();
                    throw new ValidationException(firstError.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// 파일업로드 또는 textarea 입력된 JSON형식의 문자열 파싱.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<AddDto> JsonContentParser(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                throw new Exception("JSON이 비어 있습니다.");

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<AddDto> addDtos = new();

            if (IsJsonArray(jsonString))
            {
                var list = JsonSerializer.Deserialize<List<AddDto>>(jsonString, option);

                if (list == null || list.Count == 0)
                    throw new Exception("JSON이 비어 있습니다.");

                addDtos.AddRange(list);
            }
            else
            {
                var addDto = JsonSerializer.Deserialize<AddDto>(jsonString, option);

                if (addDto == null)
                    throw new Exception("JSON이 비어 있습니다.");

                addDtos.Add(addDto);
            }

            ValidateDtos(addDtos);

            return addDtos;
        }

        /// <summary>
        /// 파일업로드 또는 textarea 입력된 CSV형식의 문자열 파싱.
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<AddDto> CsvContentParser(string csvString)
        {
            List<AddDto> addDtos = new();

            var csvDtos = csvString.Replace("\r\n", "\n")
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

            return addDtos;
        }

        /// <summary>
        /// 직원정보 중 joined 문자열을 DateTime으로 파싱.
        /// </summary>
        /// <param name="joined"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static DateTime ParseJoined(string joined)
        {
            if (string.IsNullOrWhiteSpace(joined))
                throw new FormatException("joined 값이 없습니다.");

            joined = joined.Trim();

            string[] formats = { "yyyy.MM.dd", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(joined, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                return dt.Date;
            }

            if (DateTime.TryParse(joined, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt.Date;
            }

            throw new FormatException("joined의 날짜 형식이 올바르지 않습니다.");
        }

        /// <summary>
        /// JSON 문자열이 배열형태인지 단일객체 형태 boolean 반환.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsJsonArray(string json)
        {
            var start = json.TrimStart();

            if (start.StartsWith("["))
                return true;

            return false;
        }


        /// <summary>
        /// 해당 문자열이 JSON 형식인지 간단히 판별하여 확장자 반환.
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static AllowedFileExtension CheckRawStringFormat(string rawString)
        {
            var start = rawString.TrimStart();

            if (start.StartsWith("{") || start.StartsWith("["))
                return AllowedFileExtension.Json;

            return AllowedFileExtension.Csv;
        }
        #endregion

    }
}
