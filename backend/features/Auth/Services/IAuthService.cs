using Backend.features.Auth.DTOs;

namespace Backend.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(UsuarioLoginDTO dto);
    }
}