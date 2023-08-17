using System.ComponentModel.DataAnnotations;

namespace Protov4.DTO
{
    public class PedidoDTO
    {
        [Key]
        public int id_pedido { get; set; }  // ID del pedido
        public int id_cliente { get; set; }  // ID del cliente que realizó el pedido
        public decimal pago_total { get; set; }  // Monto total del pago realizado en el pedido
        public string? Ciudad_envio { get; set; }  // Ciudad de envío del pedido
        [Required(ErrorMessage ="Campo requerido")]
        public string? Calle_principal { get; set; }  // Calle principal de la dirección de envío
        [Required(ErrorMessage = "Campo requerido")]
        public string? Calle_secundaria { get; set; }  // Calle secundaria de la dirección de envío
        public int id_tipo_pago { get; set; }  // ID del tipo de pago utilizado
        public int id_tipo_estado { get; set; }  // ID del tipo de estado del pedido (por ejemplo, "En proceso", "Entregado", etc.)
        public DateTime fecha_pedido { get; set; }  // Fecha en que se realizó el pedido
    }
}
