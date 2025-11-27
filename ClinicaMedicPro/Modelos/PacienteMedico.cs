using Newtonsoft.Json;

namespace ClinicaMedicPro.Modelos;

public class PacienteMedico
{
    [JsonProperty("pk_paciente")]
    public int pk_paciente { get; set; }

    [JsonProperty("us_nombre")]
    public string us_nombre { get; set; } = "";

    [JsonProperty("us_correo")]
    public string us_correo { get; set; } = "";

    [JsonProperty("pa_cedula")]
    public string pa_cedula { get; set; } = "";

    [JsonProperty("pa_telefono")]
    public string? pa_telefono { get; set; }

    public string Iniciales =>
    string.Join("", us_nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Take(2)
        .Select(p => p.Length > 0 ? char.ToUpper(p[0]) : ' ')
        .Where(c => c != ' '));
}