using Backend.Features.DTOs;

namespace Backend.Features.Services
{
    public interface IProductoService
    {
        Task<List<ProductoReadDTO>> GetAll();
        Task<List<ProductoReadDTO>> GetAllActives();
        Task<ProductoReadDTO> GetById(int id);
        Task<ProductoReadDTO> Create(ProductoCreateDTO dto);
        Task<bool> Update(int id, ProductoUpdateDTO dto);
        Task<bool> DeleteByLogic(int id);
    }
}