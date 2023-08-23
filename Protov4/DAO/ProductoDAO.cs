using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Protov4.DTO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Protov4.DAO
{
    public class ProductoDAO 
    {
        private readonly IMongoCollection<ProductoDTO> prod;
        // Inicializa la conexión a la base de datos MongoDB y obtiene la colección de productos
        public ProductoDAO(IConfiguration configuration)
        {
            var mongo = new DBMongo(configuration);
            prod = mongo.GetDatabase().GetCollection<ProductoDTO>("Productos");
        }
        // Obtiene una lista de productos según el tipo especificado
        public List<ProductoDTO> ObtenerProductos(string tipo,string busqueda)
        {
            var filtro = Builders<ProductoDTO>.Filter.Eq(busqueda, tipo);

            if (busqueda=="Nombre_Producto" && tipo!=null)
            {
                var regexPattern = new BsonRegularExpression(new Regex(tipo, RegexOptions.IgnoreCase));

                var filterBuilder = Builders<ProductoDTO>.Filter;
                var filter = filterBuilder.Regex(busqueda, regexPattern);

                var query = prod.Find(filter).ToListAsync();
                return query.Result;
            }
            else { 
            if(tipo==null)
            {
                var query = prod.Find(new BsonDocument()).ToListAsync();
                return query.Result;
            }
            else
            {
                var query = prod.Find(filtro).ToListAsync();
                return query.Result;
            }
            }
        }



        // Obtiene detalles de un producto según su ID en formato string
        public List<ProductoDTO> ObtenerSeleccion(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<ProductoDTO>.Filter.Eq(x => x.Id, objectId);
            return prod.Find(filter).ToList();
        }

        //Método para actualizar las existencias de un producto luego de su compra
        public void ActualizarExistencias(string id,int cantidad)
        {
            var objectId = new ObjectId(id);
            var filtro = Builders<ProductoDTO>.Filter.Eq(p => p.Id, objectId);

            var update = Builders<ProductoDTO>.Update.Inc(p => p.Existencia, -cantidad); // Incrementar la existencia en 10 unidades

            var updateResult = prod.UpdateOne(filtro, update);

        }
        public void ActualizarProducto(string _id,string nombre,double precio,string tipo,string imagenBase64,string Marca,int existencia,string Fabricante,string Modelo,string Velocidad,string Zocalo,string TamañoVram, string Interfaz,string Tamañomemoria,string TecnologiaRam,string Almacenamiento,List<string> Descripcion)
        {
            int index = imagenBase64.IndexOf(",") + 1;
            string base64Data = imagenBase64.Substring(index);
            byte[] imagenBytes = Convert.FromBase64String(base64Data);

            var filter = Builders<ProductoDTO>.Filter.Eq(p => p.Id, ObjectId.Parse(_id)); // Filtrar por el ID del producto a actualizar

            var update = Builders<ProductoDTO>.Update
                .Set(p => p.Nombre_Producto, nombre)
                .Set(p => p.Precio, precio)
                .Set(p => p.Marca, Marca)
                .Set(p => p.Existencia, existencia)
                .Set(p => p.Tipo, tipo)
                .Set(p => p.Imagen, imagenBytes)
                .Set(p => p.Especificaciones.Fabricante, Fabricante)
                .Set(p => p.Especificaciones.Modelo, Modelo)
                .Set(p => p.Especificaciones.Velocidad, Velocidad)
                .Set(p => p.Especificaciones.Zócalo, Zocalo)
                .Set(p => p.Especificaciones.TamañoVRAM, TamañoVram)
                .Set(p => p.Especificaciones.Interfaz, Interfaz)
                .Set(p => p.Especificaciones.TecnologíaRAM, TecnologiaRam)
                .Set(p => p.Especificaciones.Tamañomemoria, Tamañomemoria)
                .Set(p => p.Especificaciones.Almacenamiento, Almacenamiento)
                .Set(p => p.Especificaciones.Descripción, Descripcion);

            var updateResult = prod.UpdateOne(filter, update);
        }
        //Inserta un Producto
        public void InsertarProducto(string nombre,string imagenBase64,float precio,string Marca, int existencia,string tipo,string fabricante,string modelo,string velocidad,string Zócalo,string TamañoVRAM,string Interfaz,string TecnologiaRAM,string tamañomemoria,string Almacenamiento,List<string> Descripcion)
        {
            int index=imagenBase64.IndexOf(",")+1;
            string base64Data = imagenBase64.Substring(index);
            byte[] imagenBytes = Convert.FromBase64String(base64Data);

            var producto = new ProductoDTO
            {
                Nombre_Producto = nombre,
                Precio = precio,
                Marca = Marca,
                Existencia = existencia,
                Tipo = tipo,
                Imagen = imagenBytes,
                Especificaciones =  new EspecificacionesDTO
                {
                    Fabricante=fabricante,
                    Modelo=modelo,
                    Velocidad=velocidad,
                    Zócalo=Zócalo,
                    TamañoVRAM=TamañoVRAM,
                    Interfaz=Interfaz,
                    TecnologíaRAM=TecnologiaRAM,
                    Tamañomemoria=tamañomemoria,
                    Almacenamiento=Almacenamiento,
                    Descripción=Descripcion
                }
            };
            prod.InsertOne(producto);
        }
        public void eliminarProducto(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<ProductoDTO>.Filter.Eq("_id", objectId);
            prod.DeleteOne(filter);
        }
    }
}
