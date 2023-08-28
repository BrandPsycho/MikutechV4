using System.ComponentModel.DataAnnotations;

namespace Protov4.DTO
{
    public class PedidosDTO
    {
        [Key]
        public int id_pedido { get; set; }  // ID del pedido
        public string? ciudad_envio { get; set; }  // Ciudad de envío del pedido
        public string? Calle_principal { get; set; }  // Calle principal de la dirección de envío
        public string? Calle_secundaria { get; set; }  // Calle secundaria de la dirección de envío
        public string? nombre_estado { get; set; }
    }
}
