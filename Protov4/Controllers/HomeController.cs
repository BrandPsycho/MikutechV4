using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Protov4.Models;
using Protov4.DAO;
using System.Security.Claims;

namespace Protov4.Controllers
{
    [Authorize] // Se aplica el atributo [Authorize] a toda la clase, lo que significa que se requiere autenticación para acceder a sus métodos.
    public class HomeController : Controller
    {
        private readonly UsuariosDAO _usuariosDAO;

        public HomeController(UsuariosDAO usuariosDAO)
        {
            _usuariosDAO = usuariosDAO;
        }

        // Acción para la página de inicio
        public IActionResult Index()
        {
            return View();
        }

        // Acción para la página "Mi Cuenta"
        public IActionResult MiCuenta()
        {
            ClaimsPrincipal c = HttpContext.User; // Obtener el principal de reclamaciones asociado con la solicitud actual
            if (c.Identity != null && c.Identity.IsAuthenticated) // Verificar si el usuario está autenticado y obtener su ID de cliente para listar sus pedidos
            {
                if (int.TryParse(c.FindFirstValue("id_cliente"), out int idCliente))
                {
                    var Lista = _usuariosDAO.ListarPedidos(idCliente);
                    return View(Lista); // Mostrar la lista de pedidos del cliente autenticado
                }
            }
            // Si el usuario no está autenticado o se produce un error, redirigir a la página principal
            return RedirectToAction("Index", "Home");
        }

        // Acción para la página "Acerca De"
        public ActionResult AcercaDe() {
            return View();
        }

        // Acción para la página de administrador
        public IActionResult Admin()
        {
            return View();
        }

        // Acción para manejar errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Mostrar la vista de error con información sobre el error actual
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}