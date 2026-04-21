using CasoEstudio2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CasoEstudio2.Controllers
{
    public class CasasController : Controller
    {
        private readonly string _connectionString;

        public CasasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        // GET: /Casas/ConsultaCasas
        public IActionResult ConsultaCasas()
        {
            List<CasasModel> lista = new List<CasasModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ConsultarCasas", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new CasasModel
                    {
                        IdCasa = Convert.ToInt64(reader["IdCasa"]),
                        DescripcionCasa = reader["DescripcionCasa"].ToString(),
                        PrecioCasa = Convert.ToDecimal(reader["PrecioCasa"]),
                        UsuarioAlquiler = reader["UsuarioAlquiler"] == DBNull.Value ? null : reader["UsuarioAlquiler"].ToString(),
                        FechaAlquiler = reader["FechaAlquiler"] == DBNull.Value ? null : Convert.ToDateTime(reader["FechaAlquiler"])
                    });
                }
            }

            return View(lista);
        }

        // GET: /Casas/AlquilerCasas
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

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AlquilarCasa", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdCasa", model.IdCasa);
                cmd.Parameters.AddWithValue("@UsuarioAlquiler", model.UsuarioAlquiler);
                cmd.Parameters.AddWithValue("@FechaAlquiler", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("ConsultaCasas");
        }

        private List<CasasModel> ObtenerCasasDisponibles()
        {
            List<CasasModel> casas = new List<CasasModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ObtenerCasasDisponibles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    casas.Add(new CasasModel
                    {
                        IdCasa = Convert.ToInt64(reader["IdCasa"]),
                        DescripcionCasa = reader["DescripcionCasa"].ToString(),
                        PrecioCasa = Convert.ToDecimal(reader["PrecioCasa"])
                    });
                }
            }

            return casas;
        }
    }
}
