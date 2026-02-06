namespace EmergencyContactApi.Helpers
{
    public static class ImportRequestParser
    {
        public static string GetFileName(IFormFile file)
        {
            return file.FileName;
        }

        public static string CheckFileFormat(string? fileName)
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
    }
}
