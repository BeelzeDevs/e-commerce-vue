using Backend.Features.Auth.DTOs;

namespace Backend.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO<LoginRespDTO>> LoginAsync(UsuarioLoginDTO dto);
    }
}