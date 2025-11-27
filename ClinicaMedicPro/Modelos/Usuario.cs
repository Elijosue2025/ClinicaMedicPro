using Newtonsoft.Json;

namespace ClinicaMedicPro.Modelos
{
    public class Usuario
    {
        [JsonProperty("pk_usuario")]
        public int id { get; set; }               // ← ahora sí recibe el ID

        [JsonProperty("us_nombre")]
        public string nombre { get; set; } = "";

        [JsonProperty("us_tipo")]
        public string rol { get; set; } = "";

        [JsonProperty("especialidad")]
        public string especialidad { get; set; } = "General";

        // opcional: correo si lo necesitas
        // [JsonProperty("us_correo")] public string correo { get; set; }
    }
}