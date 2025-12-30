using Backend.Features.DTOs;
namespace Backend.Features.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioReadDTO>> GetAll();
        Task<ResultadoPaginado<UsuarioReadDTO>> GetAllPagerFilterAdmin(UsuarioQueryDTO query);
        Task<UsuarioReadDTO> GetById(int id);
        Task<UsuarioReadDTO> Create(UsuarioCreateDTO dto);
        Task<UsuarioReadDTO> CreateAdmin(CreateAdminDTO dto);
        Task<bool> Update(int id, UsuarioUpdateDTO dto);
        Task<bool> DeleteByLogic(int id);
    }
}