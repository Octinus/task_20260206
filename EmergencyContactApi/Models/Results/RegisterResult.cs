namespace EmergencyContactApi.Models.Results
{
    public class RegisterResult
    {
        public bool Success { get; private set; }
        public int AddedCount { get; private set; }
        public string? ErrorMessage { get; private set;  }

        public RegisterResult(bool success, int addedCount, string? errorMessage)
        {
            Success = success;
            AddedCount = addedCount;
            ErrorMessage = errorMessage;
        }
    }
}
