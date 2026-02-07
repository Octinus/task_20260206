using EmergencyContactApi.Models.EmployeeDto;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace EmergencyContactApi.Helpers
{
    public static class ImportRequestParser
    {
        public enum AllowedFileExtension
        {
            Json,
            Csv
        }

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
                string content =  reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(content))
                    throw new Exception("파일에 아무 내용이 없습니다.");

                return content;
            }
        }

        /// <summary>
        /// 업로드된 파일 내용 기반의 DTO의 유효성 검증.
        /// </summary>
        /// <param name="dtos"></param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidateDtos(List<AddDto> dtos)
        {
            var results = new List<ValidationResult>();

            foreach (var dto in dtos)
            {
                results.Clear();
                var ctx = new ValidationContext(dto);

                if (!Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true))
                {
                    var firstError = results.First();
                    throw new ValidationException(firstError.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// 직원정보중 joined 문자열을 DateTime으로 파싱.
        /// </summary>
        /// <param name="joined"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static DateTime ParseJoined(string joined)
        {
            if (string.IsNullOrWhiteSpace(joined))
                throw new FormatException("joined 값이 없습니다.");

            joined = joined.Trim();

            string[] formats = {"yyyy.MM.dd","yyyy-MM-dd"};

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


        public static AllowedFileExtension CheckRawStringFormat(string rawString)
        {
            var start = rawString.TrimStart();

            if (start.StartsWith("{") || start.StartsWith("["))
                return AllowedFileExtension.Json;

            return AllowedFileExtension.Csv;
        }
    }
}
