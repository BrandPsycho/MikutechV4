using System.ComponentModel.DataAnnotations;

namespace Protov4.DTO
{
    public class TipoPagoDTO
    {
        [Key]
        public int id_tipo_pago { get; set; }  // ID del tipo de pago
        public string? nombre_pago { get; set; }  // Nombre del método de pago (por ejemplo, "Tarjeta de crédito", "PayPal", etc.)
    }
}
