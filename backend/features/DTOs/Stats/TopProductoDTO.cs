namespace Backend.Features.DTOs
{
    public class TopProductoDTO
    {
        public int ProductoId {get;set;}
        public string Nombre {get;set;} = string.Empty;
        public int CantidadVendida {get;set;}
        public decimal TotalFacturado {get;set;}
    }
}