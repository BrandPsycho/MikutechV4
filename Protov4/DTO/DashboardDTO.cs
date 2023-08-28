namespace Protov4.DTO
{
    public class DashboardDTO
    {
        public string? id_producto { get; set; } //Id del producto
        public string? Nombre_Producto { get; set; }  // Nombre del producto
        public int cantidad { get; set; }  // Cantidad del producto en el carrito
        public int existencias { get; set; } //Existencia del producto
        public string? tipo { get; set; } //Tipo o Categoria del Producto
    }
}
