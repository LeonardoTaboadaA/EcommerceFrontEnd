using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entidades
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public decimal Igv { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}
