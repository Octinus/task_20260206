namespace EmergencyContactApi.Models.Commons
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Result { get; set; }
        public string? Error { get; init; }
    }
}
