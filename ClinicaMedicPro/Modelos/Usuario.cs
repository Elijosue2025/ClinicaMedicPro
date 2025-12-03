using Newtonsoft.Json;

namespace ClinicaMedicPro.Modelos
{
    public class Usuario
    {
        [JsonProperty("pk_usuario")]
        public int id { get; set; }

        [JsonProperty("us_nombre")]
        public string nombre { get; set; } = string.Empty;

        [JsonProperty("us_tipo")]
        public string rol { get; set; } = string.Empty;

        [JsonProperty("especialidad")]
        public string especialidad { get; set; } = "General";

        [JsonProperty("us_correo")]
        public string? us_correo { get; set; }

        public override string ToString() => nombre;

    }
}