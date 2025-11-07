using Backend.features.DTOs;

namespace Backend.features.Services
{
    public interface IOrdenService
    {
        // Ordenes
        Task<List<OrdenReadDTO>> GetAll();
        Task<OrdenReadDTO?> GetById(int id);
        Task<List<OrdenReadDTO>> GetOrdenesByUsarioId(int UsuarioId);
        Task<OrdenReadDTO?> Create(OrdenCreateDTO dto);
        Task<bool> Update(int id, OrdenCreateDTO dto);
        Task<bool> Delete(int id);
        // Detalles
        Task<List<DetalleReadDTO>> GetDetallesByOrdenId(int OrdenId);
        Task<bool> AddDetalle(DetalleCreateDTO dto);
        Task<bool> DeleteDetalle(DetalleDeleteDTO dto);
        Task<bool> UpdateDetalle(DetalleCreateDTO dto);
    }
}