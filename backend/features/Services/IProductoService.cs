using Backend.features.DTOs;

namespace Backend.features.Services
{
    public interface IProductoService
    {
        Task<List<ProductoReadDTO>> GetAll();
        Task<ProductoReadDTO?> GetById(int id);
        Task<ProductoReadDTO?> Create(ProductoCreateDTO dto);
        Task<bool> Update(int id, ProductoCreateDTO dto);
        Task<bool> DeleteByLogic(int id);
    }
}