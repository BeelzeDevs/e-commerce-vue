namespace Backend.Features.Auth.DTOs
{
    public class AuthResponseDTO <T>
    {    
        public List<T>? Results { get; set; }
        public string? ErrorMessage { get; set; }

        public static AuthResponseDTO<T> Ok(T data) =>
            new() {  Results = new List<T> { data } };

        public static AuthResponseDTO<T> Fail(string error) =>
        new() { ErrorMessage = error };   

    }
    public class LoginRespDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Rol {get;set;} = string.Empty;
        public DateTime Expiration { get; set; }
    }

}