using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes; 

namespace Protov4.DTO
{
    public class PedidoDetalleDTO{

        [Key]
        public int id_pedido_detalle { get; set; }  // ID del detalle del pedido
        public int id_pedido { get; set; }  // ID del pedido al que pertenece este detalle
        public string? id_producto { get; set; }  // ID del producto en el carrito
        public decimal precio { get; set; }  // Precio del producto
        public int cantidad { get; set; }  // Cantidad del producto en el carrito
        public decimal subtotal_producto { get; set; }  // Subtotal del producto (precio * cantidad)
    }

    
}
