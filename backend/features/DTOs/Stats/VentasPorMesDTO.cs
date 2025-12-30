namespace Backend.Features.DTOs
{
    public class VentasPorMesDTO
    {
        public int Año {get;set;}
        public int Mes {get;set;}
        public decimal TotalVentas {get;set;}
        public int CantidadOrdenes {get;set;}
    }
}