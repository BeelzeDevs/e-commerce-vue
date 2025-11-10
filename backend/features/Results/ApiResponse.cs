namespace Backend.Features.Results
{
    public class ApiResponse<T>
    {
        public T Results { get; set; }

        public ApiResponse(T results) => Results = results;

        public static ApiResponse<ResultSuccess> Success(string message) =>
            new(new ResultSuccess { SuccessMessage = message });

        public static ApiResponse<ResultError> Error(string message) =>
            new(new ResultError { ErrorMessage = message });
    }

    public class ResultSuccess
    {
        public string SuccessMessage { get; set; } = string.Empty;
    }

    public class ResultError
    {
        public string ErrorMessage { get; set; } = string.Empty;
    }
}