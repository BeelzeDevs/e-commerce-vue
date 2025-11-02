using Backend.features.DTOs;

namespace Backend.features.Services
{
    public interface IOrdenService
    {
        Task<List<OrdenReadDTO>> GetAll();
        Task<OrdenReadDTO?> GetById(int id);
        Task<OrdenReadDTO?> Create(OrdenCreateDTO dto);
        Task<bool> Update(int id, OrdenCreateDTO dto);
        Task<bool> Delete(int id);
    }
}