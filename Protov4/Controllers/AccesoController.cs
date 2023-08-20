using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Protov4.DAO;
using System.Security.Claims;
using Protov4.DTO;

namespace Protov4.Controllers
{
    public class AccesoController : Controller
    {
        private readonly UsuariosDAO _usuariosDAO;

        public AccesoController(UsuariosDAO usuariosDAO)
        {
            _usuariosDAO = usuariosDAO;
        }

        // Acción para mostrar la página de inicio de sesión
        public IActionResult Login()
        {
            // Obtener el principal de reclamaciones asociado con la solicitud actual
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated) // Verificar si el usuario ya está autenticado y redirigir a la página principal si es así
                {
                    return RedirectToAction("Index", "Home"); // Redirige a la página principal si ya está autenticado
                }
            }
            return View();
        }

        // Acción POST para procesar el inicio de sesión
        [HttpPost]
        public async Task<IActionResult> Login(UsuariosDTO user)
        {
            try
            {
                // Se valida el usuario en la base de datos y se obtiene su información
                (int id_usuario, int id_rol_user, int id_cliente, string nombre_compuesto) = _usuariosDAO.ValidarUsuario(user);
                if (id_usuario != 0)
                {
                    var claims = new List<Claim> // Se crean las reclamaciones para el usuario autenticado
                    {
                    new Claim(ClaimTypes.Name, nombre_compuesto),
                    new Claim("id_usuario", id_usuario.ToString()),
                    new Claim("id_cliente", id_cliente.ToString()),
                    new Claim("id_rol_user", id_rol_user.ToString())

                    };
                    // Agregar una reclamación específica para el rol del usuario
                    if (id_rol_user == 1)
                    {
                        claims.Add(new Claim("id_rol_user", "1")); // Administrador
                    }
                    else
                    {
                        claims.Add(new Claim("id_rol_user", "2")); // Usuario normal
                    }
                    // Se crea la identidad del usuario y se realiza la autenticación
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (id_rol_user == 1)
                    {
                        return RedirectToAction("Administrador", "Administrador"); // Redirige al panel de administración si es un administrador
                    }
                    else
                    {
                        // Se registra una auditoría y se redirige a la página principal
                        _usuariosDAO.RegistrarAuditoria(id_usuario, DateTime.Now, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Error = "Credenciales incorrectas";
                }
                return View();
            }
            catch (System.Exception)
            {
                ViewBag.Error = "Credenciales incorrectas"; // Muestra un mensaje de error si las credenciales son incorrectas
                return View();
            }
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null && c.Identity.IsAuthenticated)
            {
                // Obtener el principal de reclamaciones asociado con la solicitud actual
                var idUsuarioClaim = c.FindFirstValue("id_usuario");
                if (int.TryParse(idUsuarioClaim, out int id_usuario)) // Verificar si el principal de reclamaciones tiene una identidad válida y está autenticado
                {
                    bool esInicioSesion = false; // Indicar que es un cierre de sesión
                    _usuariosDAO.RegistrarAuditoria(id_usuario, DateTime.Now, esInicioSesion); // Registrar la auditoría de cierre de sesión
                }
            }
            HttpContext.Session.Remove("IdPedidoActual"); // Remueve un valor de la sesión
            Response.Cookies.Delete("carrito"); // Elimina una cookie llamada "carrito"

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Realiza el proceso de cierre de sesión
            return RedirectToAction("Login", "Acceso"); // Redirige a la página de inicio de sesión

        }

        // Acción para mostrar la página de registro de usuario
        public ActionResult Registrar()
        {
            return View();
        }

        // Acción para mostrar la vista del administrador
        public ActionResult AdminPage()
        {
            return View();
        }

        // Acción POST para procesar el registro de usuario
        [HttpPost]
        public ActionResult Registrar(ClientesDTO nclient)
        {
            if (!ModelState.IsValid)
            {
                // El modelo no es válido, regresar a la vista con mensajes de error
                return View();
            }

            if (nclient.contrasena_nueva != nclient.confirmar_contrasena)
            {
                ModelState.AddModelError("confirmar_contrasena", "Las contraseñas no coinciden");
                return View(nclient);
            }

            bool registrado = _usuariosDAO.Registrar(nclient); // Intenta registrar al nuevo cliente

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso"); // Redirige al inicio de sesión si el registro es exitoso
            }
            else
            {
                ViewBag.Error = "Credenciales incorrectas"; // Muestra un mensaje de error si el registro falla
            }

            return View();
        }
    }
}
