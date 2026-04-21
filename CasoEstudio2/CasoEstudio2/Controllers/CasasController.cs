using CasoEstudio2.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CasoEstudio2.Controllers
{
    public class CasasController : Controller
    {

        private readonly IConfiguration _configuration;
        public CasasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: /Casas/ConsultaCasas
        [HttpGet]
        public IActionResult ConsultaCasas()
        {
            using var context = new SqlConnection(
                _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var casas = context.Query<CasasModel>("SP_ConsultarCasas",
                commandType: CommandType.StoredProcedure);

            return View(casas);
        }

        // GET: /Casas/AlquilerCasas
        [HttpGet]
        public IActionResult AlquilerCasas()
        {
            ViewBag.Casas = ObtenerCasasDisponibles();
            return View(new CasasModel());
        }

        // POST: /Casas/AlquilerCasas
        [HttpPost]
        public IActionResult AlquilerCasas(CasasModel model)
        {
            if (string.IsNullOrEmpty(model.UsuarioAlquiler))
            {
                ModelState.AddModelError("UsuarioAlquiler", "El usuario es requerido.");
                ViewBag.Casas = ObtenerCasasDisponibles();
                return View(model);
            }

            using var context = new SqlConnection(
                _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parameters = new DynamicParameters();

            parameters.Add("@IdCasa", model.IdCasa);
            parameters.Add("@UsuarioAlquiler", model.UsuarioAlquiler);
            parameters.Add("@FechaAlquiler", DateTime.Now);

            context.Execute("SP_AlquilarCasa", parameters, commandType: CommandType.StoredProcedure);

            return RedirectToAction("ConsultaCasas");
        }

        private List<CasasModel> ObtenerCasasDisponibles()
        {
            using var context = new SqlConnection(
                _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var casasDisponibles = context.Query<CasasModel>("SP_ObtenerCasasDisponibles",
                commandType: CommandType.StoredProcedure);
            return ViewBag.Casas = casasDisponibles.ToList();
        }

    }
}
