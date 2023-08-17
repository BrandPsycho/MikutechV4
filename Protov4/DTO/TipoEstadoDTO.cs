using System.ComponentModel.DataAnnotations;

namespace Protov4.DTO
{
    public class TipoEstadoDTO
    {
        [Key]
        public int id_tipo_estado { get; set; }  // ID del tipo de estado
        public string? nombre_estado { get; set; }  // Nombre del estado (por ejemplo, "En proceso", "Entregado", etc.)
    }
}
