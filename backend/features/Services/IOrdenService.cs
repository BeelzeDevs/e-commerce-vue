using Backend.Features.DTOs;
using Backend.Features.Models;

namespace Backend.Features.Services
{
    public interface IOrdenService
    {
        // Ordenes
        Task<List<OrdenReadDTO>> GetAll();
        Task<ResultadoPaginado<OrdenReadDTO>> GetAllPagerFilterAdmin(OrdenQueryDTO queryOrd);
        Task<OrdenReadDTO> GetByOrdenId(int id);
        Task<List<OrdenReadDTO>> GetOrdenesByUsarioId(int UsuarioId);
        Task<OrdenReadDTO> Create(OrdenCreateDTO dto);
        // Task<bool> Update(int id, OrdenUpdateDTO dto);
        // Task<bool> Delete(int id);
        // Detalles
        Task<List<DetalleReadDTO>> GetDetallesByOrdenId(int OrdenId);
        // Task<bool> DeleteDetalle(DetalleDeleteDTO dto);
        // Task<bool> UpdateDetalle(DetalleCreateDTO dto);

    }
}