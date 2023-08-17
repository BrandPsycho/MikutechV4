using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Protov4.DTO
{
    public class CarritoFullDTO
    {

        public string? id_producto { get; set; }  // ID del producto en el carrito
        public byte[]? Imagen { get; set; }  // Imagen del producto (representada como un arreglo de bytes)
        public string? Nombre_Producto { get; set; }  // Nombre del producto
        public int cantidad { get; set; }  // Cantidad del producto en el carrito
        public int existencias { get; set; }  // Cantidad de existencias del producto
        public decimal Precio { get; set; }  // Precio del producto
        public decimal subtotal_producto { get; set; }  // Subtotal del producto (precio * cantidad)

    }
}
