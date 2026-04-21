using System.Numerics;

namespace CasoEstudio2.Models
{
    public class CasasModel
    {
        public long IdCasa { get; set; }
        public string DescripcionCasa { get; set; }
        public decimal PrecioCasa { get; set; }
        public string? UsuarioAlquiler { get; set; }
        public DateTime? FechaAlquiler { get; set; }
        public string? Estado { get; set; }
    }
}
