using Backend.Features.DTOs;

namespace Backend.Features.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaReadDTO>> GetAll();
        Task<CategoriaReadDTO> GetById(int id);
        Task<CategoriaReadDTO> Create(CategoriaCreateDTO dto);
        Task<bool> Update(int id, CategoriaCreateDTO dto);

    }

}