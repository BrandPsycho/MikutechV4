using Protov4.DTO;
using System.Data.SqlClient;
using System.Data;

namespace Protov4.DAO
{
    public class DashboardDAO : DbConnection
    {
        private readonly ProductoDAO db;
        SqlCommand cmd = new SqlCommand();
        public DashboardDAO(IConfiguration configuration) : base(configuration)
        {
            db = new ProductoDAO(configuration);
        }
        public List<DashboardDTO> MasVendidos()
        {
            List<DashboardDTO> listsql = new List<DashboardDTO>();
            List<DashboardDTO> list = new List<DashboardDTO>();

            var listmongo = new List<DashboardDTO>();
            using (var connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("ObtenerProductosMasVendidos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listsql.Add(new DashboardDTO()
                    {
                        id_producto = reader.GetString(0),
                        cantidad = reader.GetInt32(1)
                    });
                }
                foreach (var item in listsql)
                {

                    var pro = db.ObtenerSeleccion(item.id_producto.ToString());
                    var items = pro.Select(p => new DashboardDTO
                    {
                        id_producto = p.Id.ToString(),
                        Nombre_Producto = p.Nombre_Producto,
                        existencias=p.Existencia,
                        tipo=p.Tipo,
                        cantidad = 0
                    }); ;
                    listmongo.AddRange(items);
                }



                foreach (var itemSql in listsql)
                {
                    // Buscar el elemento correspondiente en listmongo basado en algún identificador único
                    var itemMongo = listmongo.FirstOrDefault(item => item.id_producto == itemSql.id_producto);

                    if (itemMongo != null)
                    {
                        // Combinar las propiedades del elemento de listsql y listmongo en un nuevo objeto CarritoFullDTO
                        var dashItem = new DashboardDTO
                        {
                            id_producto = itemSql.id_producto,
                            cantidad = itemSql.cantidad,
                            Nombre_Producto = itemMongo.Nombre_Producto,
                            existencias=itemMongo.existencias,
                            tipo=itemMongo.tipo
                            
                        };

                        list.Add(dashItem);
                    }
                }

                return list;
            }

        }
    }
}
