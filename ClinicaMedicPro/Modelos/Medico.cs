// ClinicaMedicPro/Modelos/Medico.cs
using System.Text.Json.Serialization;

namespace ClinicaMedicPro.Modelos
{
    public class Medico
    {
        [JsonPropertyName("pk_medico")]
        public int pk_medico { get; set; }

        [JsonPropertyName("me_especialidad")]
        public string? me_especialidad { get; set; }

        [JsonPropertyName("pk_usuario")]
        public int fk_usuario { get; set; } // ← Correcto: tu API devuelve "pk_usuario"

        [JsonPropertyName("us_nombre")]
        public string us_nombre { get; set; } = string.Empty;

        [JsonPropertyName("us_correo")]
        public string us_correo { get; set; } = string.Empty;

        [JsonPropertyName("us_huella")]
        public string? us_huella { get; set; }

        [JsonPropertyName("us_ubicacion")]
        public string? us_ubicacion { get; set; }

        // Propiedades calculadas
        [JsonIgnore]
        public string Iniciales
        {
            get
            {
                if (string.IsNullOrWhiteSpace(us_nombre)) return "??";
                var partes = us_nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return partes.Length >= 2
                    ? $"{char.ToUpper(partes[0][0])}{char.ToUpper(partes[^1][0])}"
                    : char.ToUpper(partes[0][0]).ToString();
            }
        }

        [JsonIgnore]
        public string EspecialidadDisplay =>
            string.IsNullOrWhiteSpace(me_especialidad) ? "General" : me_especialidad;
    }
}