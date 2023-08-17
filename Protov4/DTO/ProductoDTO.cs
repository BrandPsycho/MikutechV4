using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Protov4.DTO
{
    public class ProductoDTO
    {
        [BsonId]
        public ObjectId Id { get; set; }  // ID del producto en la base de datos MongoDB
        public string? Nombre_Producto { get; set; }  // Nombre del producto
        public double Precio { get; set; }  // Precio del producto
        public string? Marca { get; set; }  // Marca del producto
        public int Existencia { get; set; }  // Cantidad de existencias del producto
        public string? Tipo { get; set; }  // Tipo de producto (por ejemplo, "Procesador", "Gráfica", etc.)
        public byte[]? Imagen { get; set; }  // Imagen del producto (representada como un arreglo de bytes)
        public EspecificacionesDTO? Especificaciones { get; set; }  // Objeto que contiene las especificaciones detalladas del producto

    }
    public class EspecificacionesDTO
    {
      public string? Fabricante { get; set; }  // Fabricante del producto
        public string? Modelo { get; set; }  // Modelo del producto
        public string? Velocidad { get; set; }  // Velocidad del producto
        public string? Zócalo { get; set; }  // Zócalo del producto
        public string? TamañoVRAM { get; set; }  // Tamaño de la memoria VRAM
        public string? Interfaz { get; set; }  // Interfaz del producto
        public string? Tamañomemoria { get; set; }  // Tamaño de la memoria
        public string? TecnologíaRAM { get; set; }  // Tecnología de la memoria RAM
        public string? Almacenamiento { get; set; }  // Capacidad de almacenamiento
        public List<string>? Descripción { get; set; }  // Lista de descripciones detalladas del producto

    }
}
