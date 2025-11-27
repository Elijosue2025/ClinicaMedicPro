using System.Text.Json.Serialization;

namespace ClinicaMedicPro.Modelos;

public class CitaMedico
{
    [JsonPropertyName("pk_cita")]
    public int pk_cita { get; set; }

    [JsonPropertyName("ci_fecha")]
    public DateTime ci_fecha { get; set; }

    [JsonPropertyName("ci_hora")]
    public string ci_hora { get; set; } = "";

    [JsonPropertyName("ci_motivo")]
    public string? ci_motivo { get; set; }

    [JsonPropertyName("ci_estado")]
    public string ci_estado { get; set; } = "agendada";

    // Para mostrar bonito
    public string FechaDisplay => ci_fecha.ToString("dddd, dd MMMM yyyy");
    public string HoraDisplay => ci_hora;
    public string EstadoColor => ci_estado.ToLower() switch
    {
        "completada" => "#10B981",
        "cancelada" => "#EF4444",
        _ => "#3B82F6"
    };
}