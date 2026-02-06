using EmergencyContactApi.Models.EmployeeDto;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EmergencyContactApi.Helpers
{
    public static class ImportRequestParser
    {
        public static string GetFileName(IFormFile file)
        {
            return file.FileName;
        }

        public static string GetFileFormat(string? fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new NullReferenceException("파일명이 없습니다.");

                string extension = Path.GetExtension(fileName);
                if (string.Equals(extension, ".json", StringComparison.OrdinalIgnoreCase))
                    return "json";
                if (string.Equals(extension, ".csv", StringComparison.OrdinalIgnoreCase))
                    return "csv";
                throw new ArgumentException("지원하지 않는 확장자의 파일입니다.");
            }
            catch
            {
                throw;
            }

        }

        public static string GetFileContent(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return reader.ReadToEnd();
            }
        }

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

        public static string CheckRawStringFormat(string payload)
        {
            var start = payload.TrimStart();

            if (start.StartsWith("{") || start.StartsWith("["))
                return "json";

            return "csv";
        }
    }
}
