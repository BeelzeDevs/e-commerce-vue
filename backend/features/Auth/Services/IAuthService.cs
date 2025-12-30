using Backend.Features.Auth.DTOs;
using Backend.Features.Results;

namespace Backend.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<LoginRespDTO> LoginAsync(UsuarioLoginDTO dto);
    }
}