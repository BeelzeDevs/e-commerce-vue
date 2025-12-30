namespace Backend.Features.DTOs
{
    public class CreateAdminDTO
    {
        public string Nombre {get;set;} = string.Empty;
        public string Email {get;set;} = string.Empty;
        public string Password {get;set;} = string.Empty;
    }
}