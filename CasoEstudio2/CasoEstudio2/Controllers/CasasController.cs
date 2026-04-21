using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio2.Controllers
{
    [Route("casas/[controller]")]
    public class CasasController : Controller
    {
        [HttpGet("ConsultaCasas")]
        public IActionResult ConsultarCasas()
        {
            return View();
        }

        [HttpGet("AlquilarCasas")]
        public IActionResult AlquilarCasas()
        {
            return View();
        }
    }
}
