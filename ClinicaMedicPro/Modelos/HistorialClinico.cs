using Newtonsoft.Json;

namespace ClinicaMedicPro.Modelos;

public class HistorialClinico
{
    [JsonProperty("pk_historial")]
    public int pk_historial { get; set; }

    [JsonProperty("hi_diagnostico")]
    public string hi_diagnostico { get; set; } = "";

    [JsonProperty("hi_tratamiento")]
    public string? hi_tratamiento { get; set; }

    [JsonProperty("hi_notas")]
    public string? hi_notas { get; set; }

    [JsonProperty("hi_fecha")]
    public string hi_fecha { get; set; } = "";

    [JsonProperty("medico_nombre")]
    public string? medico_nombre { get; set; }
}