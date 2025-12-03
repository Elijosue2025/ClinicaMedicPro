using Newtonsoft.Json;

namespace ClinicaMedicPro.Modelos;

public class HistorialClinico
{
    [JsonProperty("pk_historial")]
    public int PkHistorial { get; set; }

    [JsonProperty("hi_diagnostico")]
    public string HiDiagnostico { get; set; } = "";

    [JsonProperty("hi_tratamiento")]
    public string? HiTratamiento { get; set; }

    [JsonProperty("hi_notas")]
    public string? HiNotas { get; set; }

    [JsonProperty("hi_fecha")]
    public string HiFecha { get; set; } = "";

    [JsonProperty("medico_nombre")]
    public string? MedicoNombre { get; set; }

    [JsonProperty("paciente_nombre")]
    public string? PacienteNombre { get; set; }

    [JsonProperty("me_especialidad")]
    public string? MeEspecialidad { get; set; }
}