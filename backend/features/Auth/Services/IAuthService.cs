using Backend.Features.Auth.DTOs;

namespace Backend.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(UsuarioLoginDTO dto);
    }
}