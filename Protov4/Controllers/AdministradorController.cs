using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protov4.DAO;
using Protov4.DTO;
using System.Security.Cryptography;

namespace Protov4.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdministradorController : Controller
    {
        private readonly MikuTechFactory db;
        public AdministradorController(IConfiguration configuration)
        {
            db = new MikutechDAO(configuration);

        }
        public IActionResult Administrador()
        {
            var productos = ListarProductos("");
            return View("Productos", productos);
        }
        [HttpGet]
        public List<ProductoDTO> ListarProductos(string tipo)
        {
            List<ProductoDTO> productos;
            productos = db.GetAllProductos(tipo);
            var ProductoDTO = productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre_Producto = p.Nombre_Producto,
                Precio = p.Precio,
                Imagen = p.Imagen,
                Marca = p.Marca,
                Existencia = p.Existencia,
                Tipo = p.Tipo,
                Especificaciones = new EspecificacionesDTO
                {
                    Fabricante=p.Especificaciones.Fabricante,
                    Modelo=p.Especificaciones.Modelo,
                    Zócalo=p.Especificaciones.Zócalo,
                    Velocidad=p.Especificaciones.Velocidad,
                    TamañoVRAM=p.Especificaciones.TamañoVRAM,
                    Interfaz=p.Especificaciones.Interfaz,
                    TecnologíaRAM=p.Especificaciones.TecnologíaRAM,
                    Almacenamiento=p.Especificaciones.Almacenamiento,
                    Tamañomemoria=p.Especificaciones.Tamañomemoria,
                    Descripción=p.Especificaciones.Descripción
                }
            }).ToList();

            return ProductoDTO;
        }
        public ActionResult Auditoria()
        {
            var audotira = obtenerAuditoria();
            return View(audotira);
        }
        [HttpGet]
        public List<AuditoriaDTO> obtenerAuditoria()
        {
            List<AuditoriaDTO> auditoria;
            auditoria = db.ObtenerAuditoria();
            var AuditoriaDTO = auditoria.Select(p => new AuditoriaDTO
            {
                id_auditoria = p.id_auditoria,
                id_usuario = p.id_usuario,
                nombre_cliente = p.nombre_cliente,
                apellido_cliente = p.apellido_cliente,
                correo_elec = p.correo_elec,
                fecha_inicio_sesion = p.fecha_inicio_sesion,
                fecha_cierre_session = p.fecha_cierre_session
            }).ToList();

            return AuditoriaDTO;
        }
        public ActionResult CrearNuevoProducto()
        {

            return View();
        }
        [HttpPost]
        public ActionResult NuevoProducto(string nombre, string imagenBase64, float precio, string Marca, int existencia, string tipo, string fabricante, string modelo, string velocidad, string Zócalo, string TamañoVRAM, string Interfaz, string TecnologiaRAM, string tamañomemoria, string Almacenamiento, List<string> Descripcion)
        {
            db.InsertarProducto(nombre, imagenBase64,precio, Marca, existencia, tipo, fabricante, modelo, velocidad, Zócalo, TamañoVRAM, Interfaz, TecnologiaRAM, tamañomemoria, Almacenamiento, Descripcion);
            var productos = ListarProductos("");
            return View("Productos", productos);
        }
        [HttpPost]
        public ActionResult eliminarProducto(string id)
        {
            db.eliminarProducto(id);
            var productos = ListarProductos("");
            return View("Productos", productos);
        }
      
        public ActionResult EditarProducto(string _id)
        {
            var productos = ObjetoSeleccion(_id);
            return View(productos);
        }
        public ActionResult EditarProductoDAO(string _id, string nombre, double precio, string tipo, string imagenBase64, string Marca, int existencia, string Fabricante, string Modelo, string Velocidad, string Zocalo, string TamañoVram, string Interfaz, string Tamañomemoria, string TecnologiaRam, string Almacenamiento, List<string> Descripcion)
        {
            List<string> descripcionesSinDuplicados = Descripcion.Distinct().ToList();

            db.ActualizarProducto(_id, nombre, precio, tipo, imagenBase64, Marca, existencia, Fabricante, Modelo, Velocidad, Zocalo, TamañoVram, Interfaz, Tamañomemoria, TecnologiaRam, Almacenamiento, descripcionesSinDuplicados);
            var productos = ListarProductos("");
            return View("Productos", productos);
        }
        [HttpGet]
        public List<ProductoDTO> ObjetoSeleccion(string _id)
        {
            List<ProductoDTO> productos;
            productos = db.GetSeleccion(_id);
            var ProductoDTO = productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre_Producto = p.Nombre_Producto,
                Precio = p.Precio,
                Imagen = p.Imagen,
                Marca = p.Marca,
                Existencia = p.Existencia,
                Tipo = p.Tipo,
                Especificaciones = new EspecificacionesDTO
                {
                    Fabricante = p.Especificaciones.Fabricante,
                    Modelo = p.Especificaciones.Modelo,
                    Velocidad = p.Especificaciones.Velocidad,
                    Zócalo = p.Especificaciones.Zócalo,
                    TamañoVRAM = p.Especificaciones.TamañoVRAM,
                    Interfaz = p.Especificaciones.Interfaz,
                    Tamañomemoria = p.Especificaciones.Tamañomemoria,
                    TecnologíaRAM = p.Especificaciones.TecnologíaRAM,
                    Almacenamiento = p.Especificaciones.Almacenamiento,
                    Descripción = p.Especificaciones.Descripción
                }

            }).ToList();

            return ProductoDTO;
        }
    }
}
