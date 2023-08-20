using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Protov4.DAO;
using Protov4.DTO;
using Protov4.Models;
using System.Security.Claims;

namespace Protov4.Controllers
{
    [Authorize]

    public class ProductoController : Controller
    {
        private readonly MikuTechFactory db;
        private static bool pedido = false;


        public ProductoController(IConfiguration configuration)
        {
            db = new MikutechDAO(configuration);
           
        }

        // GET: ProductoController/Producto
        // Muestra una lista de productos filtrados por tipo
        public ActionResult Producto(string tipo)
        {
            var productos = ListarProductos(tipo);
            ViewData["tipo"] = tipo;
            return View(productos);
        }
        // GET: ProductoController/ProductoSeleccion
        // Muestra los detalles de un producto específico seleccionado por ID
        public ActionResult ProductoSeleccion(string _id)
        {
            var productos = ObjetoSeleccion(_id);
            return View(productos);
        }
        // Método auxiliar HTTP GET: Obtiene los detalles de un producto seleccionado por ID
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
        // Método auxiliar HTTP GET: Obtiene una lista de productos filtrados por tipo
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
                Tipo = p.Tipo
            }).ToList();

            return ProductoDTO;
        }

        // GET: ProductoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductoController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: ProductoController/CrearCarrito
        // Agrega un producto al carrito de compras
        [HttpPost]
        public ActionResult CrearCarrito(int id_cliente, string id_producto, decimal precio, int cantidad, decimal subtotal)
        {
            try
            {
                int id_pedido;
               
                if (HttpContext.Session.GetInt32("IdPedidoActual") != null)
                {
                   
                    id_pedido = db.ObtenerIdPedido();

                    TempData["PedidoRegistrado"] = true;
                    HttpContext.Session.SetInt32("IdPedidoActual", id_pedido);
                }
                else
                {
                    ClaimsPrincipal cl = HttpContext.User;
                    if (cl.Identity != null && cl.Identity.IsAuthenticated) // Verificar si el usuario está autenticado y obtener su ID de cliente para listar sus pedidos
                    {
                        if (int.TryParse(cl.FindFirstValue("id_cliente"), out int idCliente))
                        {
                            db.RegistrarPedido(idCliente);
                        }
                    }
                    id_pedido = db.ObtenerIdPedido();
                    TempData["PedidoRegistrado"] = true;
                    HttpContext.Session.SetInt32("IdPedidoActual", id_pedido);
                   
                }
                
                db.insertarCarrito(id_pedido, id_producto, precio, cantidad, subtotal);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }


        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
