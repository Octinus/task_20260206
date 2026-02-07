namespace EmergencyContactApi.Models.Results
{
    public class RegisterResult
    {
        public List<SuccessResult> SuccessResults { get; private set; }
        public int SuccessCount { get; private set; }
        public List<FailureResult> FailureResults { get; private set; }
        public int FailureCount { get; private set; }

        public RegisterResult(List<SuccessResult> successResults, List<FailureResult> failureResults)
        {
            SuccessResults = successResults;
            SuccessCount = successResults.Count;
            FailureResults = failureResults;
            FailureCount = failureResults.Count;
        }
    }
}
