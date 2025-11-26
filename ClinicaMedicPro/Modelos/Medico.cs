using System.Text.Json.Serialization;

namespace ClinicaMedicPro.Modelos
{
    public class Medico
    {
        [JsonPropertyName("pk_medico")]
        public int pk_medico { get; set; }

        [JsonPropertyName("fk_usuario")]
        public int fk_usuario { get; set; }

        [JsonPropertyName("me_especialidad")]
        public string? me_especialidad { get; set; }

        [JsonPropertyName("me_creado")]
        public string? me_creado { get; set; }

        // Datos del usuario (del JOIN)
        [JsonPropertyName("pk_usuario")]
        public int pk_usuario { get; set; }

        [JsonPropertyName("us_nombre")]
        public string us_nombre { get; set; } = string.Empty;

        [JsonPropertyName("us_correo")]
        public string us_correo { get; set; } = string.Empty;

        [JsonPropertyName("us_huella")]
        public string? us_huella { get; set; }

        [JsonPropertyName("us_ubicacion")]
        public string? us_ubicacion { get; set; }

        // Propiedades calculadas (no necesitan JsonPropertyName)
        public string NombreCompleto => us_nombre;
        public string Correo => us_correo;
        public string EspecialidadDisplay => string.IsNullOrEmpty(me_especialidad) ? "General" : me_especialidad;
        public string Iniciales =>
            string.Join("", us_nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .Select(p => char.ToUpper(p[0]))).ToUpper();
    }
}