﻿namespace Ecommerce.Models
{
    public class CarritoUsuario
    {
        public int IdCarrito { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public decimal? PrecioLista { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
    }
}
