using Protov4.DTO;
using System.Data.SqlClient;
using System.Data;

namespace Protov4.DAO
{

    public class PedidosDAO: DbConnection {
        public PedidosDAO(IConfiguration configuration) : base(configuration)
        {
            // Constructor que llama al constructor de la clase base (DbConnection) pasando la configuración.
            // Esto asegura que el objeto de conexión se inicialice correctamente.
        }


        public List<PedidosDTO> ListarPedidos()
        {
            try
            {
                var Lista = new List<PedidosDTO>(); //Creamos una lista con los mismos campos del modelo Pedidos

                using (var connection = GetSqlConnection())
                {
                    connection.Open(); // Abrir la conexión a la base de datos
                    SqlCommand cmd = new SqlCommand("VerPedidos", connection); // Llamar al procedimiento almacenado "VerPedidos"
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new PedidosDTO     // Agregar detalles de los pedidos a la lista
                            {
                                id_pedido = (int)dr["id_pedido"],
                                ciudad_envio = dr["ciudad_envio"].ToString(),
                                Calle_principal = dr["Calle_principal"].ToString(),
                                Calle_secundaria = dr["Calle_secundaria"].ToString(),
                                nombre_estado = dr["nombre_estado"].ToString()
                            });
                        }
                    }
                }
                return Lista;
            }
            catch (Exception ex)
            {
                // Agregar manejo de errores aquí si es necesario
                Console.WriteLine("Error en RegistrarPedidos: " + ex.Message);
                return new List<PedidosDTO>();
            }
        }

        public void CambiarEstado(int id_pedido, int id_tipo_estado)
        {
            using (var connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("CambiarEstado", connection);
                cmd.Parameters.AddWithValue("@id_pedido", id_pedido);
                cmd.Parameters.AddWithValue("@id_tipo_estado", id_tipo_estado);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
