namespace Backend.Features.Auth.DTOs
{
    public class AuthResponseDTO <T>
    {    
        public T Results { get; set; }

        public AuthResponseDTO(T Results) => this.Results = Results;
        public static AuthResponseDTO<ResultError> Fail(string error) =>
        new( new ResultError { ErrorMessage = error });   

    }
    public class LoginRespDTO
    {
        public string Nombre {get;set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Rol {get;set;} = string.Empty;
        public DateTime Expiration { get; set; }
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
