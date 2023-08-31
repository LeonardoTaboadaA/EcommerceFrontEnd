namespace Ecommerce.Models
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public string UserId { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public bool Activo { get; set; }
    }
}
