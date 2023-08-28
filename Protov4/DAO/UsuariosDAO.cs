using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Protov4.DTO;
using Protov4.DAO;

namespace Protov4.DAO
{
    public class UsuariosDAO : DbConnection
    {

        public UsuariosDAO(IConfiguration configuration) : base(configuration)
        {
            // Constructor que llama al constructor de la clase base (DbConnection) pasando la configuración.
            // Esto asegura que el objeto de conexión se inicialice correctamente.
        }

        // Método para validar las credenciales del usuario al intentar iniciar sesión
        public (int id_usuario, int id_rol_user, int id_cliente, string nombre_compuesto) ValidarUsuario(UsuariosDTO nuser)
        {
            int id_usuario = 0;
            int id_cliente = 0;
            int id_rol_user = 0;
            string nombre_compuesto = string.Empty;

            nuser.contrasena = ConvertirSha256(nuser.contrasena);

            using (var connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Login_ValidarUsuario", connection);
                cmd.Parameters.AddWithValue("@correo_elec", nuser.correo_elec);
                cmd.Parameters.AddWithValue("@contrasena", nuser.contrasena);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id_usuario = Convert.ToInt32(reader["id_usuario"]);
                        id_rol_user = Convert.ToInt32(reader["id_rol_user"]);
                        id_cliente = Convert.ToInt32(reader["id_cliente"]);

                        if (id_rol_user == 1)
                        {
                            nombre_compuesto = "Administrador";
                        }
                        else
                        {
                            reader.Close();
                            using (var cmdNombre = new SqlCommand("ObtenerNombreCompuestoPorIdCliente", connection))
                            {
                                cmdNombre.CommandType = CommandType.StoredProcedure;
                                cmdNombre.Parameters.AddWithValue("@id_cliente", id_cliente);

                                using (var readerNombre = cmdNombre.ExecuteReader())
                                {
                                    if (readerNombre.Read())
                                    {
                                        nombre_compuesto = readerNombre["nombre_compuesto"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                connection.Close();
                return (id_usuario, id_rol_user, id_cliente, nombre_compuesto);
            }
        }

        // Método para registrar un nuevo cliente
        public bool Registrar(ClientesDTO nclient, out string mensaje)
        {
            bool registrado = false;
            mensaje = "";
            try
            {
                // Convertir la contraseña a su hash SHA256 correspondiente
                nclient.contrasena_nueva = ConvertirSha256(nclient.contrasena_nueva);

                using (var connection = GetSqlConnection())
                {
                    connection.Open(); // Abrir la conexión a la base de datos
                    using (SqlCommand cmd = new SqlCommand("Login_RegistrarUsuario", connection)) //Se llama al procedimiento almacenado y se le envían los parámetros necesarios
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@correo_elec", SqlDbType.VarChar).Value = nclient.correo_nuevo;
                        cmd.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = nclient.contrasena_nueva;
                        cmd.Parameters.Add("@nombre_cliente", SqlDbType.VarChar).Value = nclient.nombre_cliente;
                        cmd.Parameters.Add("@apellido_cliente", SqlDbType.VarChar).Value = nclient.apellido_cliente;
                        cmd.Parameters.Add("@telefono_cliente", SqlDbType.VarChar).Value = nclient.telefono_cliente;
                        cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado
                        registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value); // Obtener el resultado del proceso de registro
                        mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // Puedes manejar el error aquí si es necesario.
            }
            return registrado;
        }

        // Método para registrar una auditoría de inicio o cierre de sesión
        public void RegistrarAuditoria(int idUsuario, DateTime fechaSesion, bool esInicioSesion)
        {
            try
            {
                using (var connection = GetSqlConnection())
                {
                    connection.Open(); // Abrir la conexión a la base de datos
                    using (SqlCommand cmd = new SqlCommand("Login_RegistrarAuditoria", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                        // Si esInicioSesion es verdadero, registramos la fecha de inicio de sesión
                        if (esInicioSesion)
                        {
                            cmd.Parameters.AddWithValue("@fecha_inicio_sesion", fechaSesion);
                            cmd.Parameters.AddWithValue("@fecha_cierre_sesion", DBNull.Value); // Valor nulo para cierre
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@fecha_inicio_sesion", DBNull.Value); // Valor nulo para inicio
                            cmd.Parameters.AddWithValue("@fecha_cierre_sesion", fechaSesion); // Registramos fecha de cierre
                        }

                        cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado
                    }
                }
            }
            catch (Exception ex)
            {
                // Agregar manejo de errores aquí si es necesario
                Console.WriteLine("Error en RegistrarAuditoria: " + ex.Message);
            }
        }

        // Método para listar los pedidos de un cliente
        public List<MisPedidosDTO> ListarPedidos(int id_cliente)
        {
            try
            {
                var Lista = new List<MisPedidosDTO>(); //Creamos una lista con los mismos campos del modelo MisPedidos

                using (var connection = GetSqlConnection())
                {
                    connection.Open(); // Abrir la conexión a la base de datos
                    SqlCommand cmd = new SqlCommand("ObtenerDatosPedido", connection); // Llamar al procedimiento almacenado "ObtenerDatosPedido"
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new MisPedidosDTO     // Agregar detalles de los pedidos a la lista
                            {
                                ciudad_envio = dr["ciudad_envio"].ToString(),
                                fecha_pedido = ((DateTime)dr["fecha_pedido"]).ToString("dd/MM/yyyy"),
                                nombre_pago = dr["nombre_pago"].ToString(),
                                pago_total = dr.IsDBNull(dr.GetOrdinal("pago_total")) ? null : (decimal?)dr.GetDecimal(dr.GetOrdinal("pago_total")),
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
                Console.WriteLine("Error en RegistrarAuditoria: " + ex.Message);
                return new List<MisPedidosDTO>();
            }
        }

        //private string ObtenerNombreCompuesto(int idCliente)
        //{
        //    using (var connection = GetSqlConnection())
        //    {
        //        connection.Open();
        //        SqlCommand cmd = new SqlCommand("ObtenerNombreCompuestoPorIdCliente", connection);
        //        cmd.Parameters.AddWithValue("@id_cliente", idCliente);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                string nombre_compuesto = reader["nombre_compuesto"].ToString();
        //                return nombre_compuesto;
        //            }
        //            else
        //            {
        //                return "Nombre Desconocido"; // O cualquier otro valor predeterminado
        //            }
        //        }
        //    }
        //}

        // Método estático para convertir un texto en su hash SHA256 correspondiente
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

    }
}

