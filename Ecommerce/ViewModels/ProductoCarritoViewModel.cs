namespace Ecommerce.ViewModels
{
    public class ProductoCarritoViewModel
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public decimal? PrecioLista { get; set; }
        public decimal Precio { get; set; }
        public int? DsctoPorcentaje { get; set; }
        public decimal Descuento { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public bool Activo { get; set; }
    }
}
