using Backend.features.DTOs;
namespace Backend.features.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioReadDTO>> GetAll();
        Task<UsuarioReadDTO?> GetById(int id);
        Task<UsuarioReadDTO> Create(UsuarioCreateDTO dto);
        Task<bool> Update(int id, UsuarioCreateDTO dto);
        Task<bool> DeleteByLogic(int id);
    }
}