using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Protov4.Models;

namespace Protov4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginUser userLogin)
        {
            return Ok("User Login");
        }
    }
}
