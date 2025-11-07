using Backend.features.DTOs;

namespace Backend.features.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaReadDTO>> GetAll();
        Task<CategoriaReadDTO?> GetById(int id);
        Task<CategoriaReadDTO> Create(CategoriaCreateDTO dto);
        Task<bool> Update(int id, CategoriaCreateDTO dto);

    }

}