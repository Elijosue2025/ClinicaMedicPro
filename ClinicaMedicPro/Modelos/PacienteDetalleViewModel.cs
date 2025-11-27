using ClinicaMedicPro.Modelos;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Linq; // ← Necesario para Split, Take, Select, etc.

namespace ClinicaMedicPro.Vistas;

public class PacienteDetalleViewModel : BindableObject
{
    private readonly HttpClient client = new();
    private const string UrlBase = "http://10.0.2.2/wsCitas/api.php";

    public ObservableCollection<HistorialClinico> Historial { get; set; } = new();
    public ObservableCollection<CitaMedico> Citas { get; set; } = new();

    private string _nombre = "Cargando...";
    public string NombreCompleto
    {
        get => _nombre;
        set { _nombre = value; OnPropertyChanged(); }
    }

    public string Correo { get; set; } = "";
    public string Cedula { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Iniciales { get; set; } = "??";

    public async Task CargarPacienteAsync(int pacienteId)
    {
        try
        {
            var url = $"{UrlBase}?resource=paciente&id={pacienteId}";
            var json = await client.GetStringAsync(url);
            var p = JsonConvert.DeserializeObject<PacienteMedico>(json);

            if (p != null)
            {
                NombreCompleto = p.us_nombre;
                Correo = p.us_correo;
                Cedula = $"Cédula: {p.pa_cedula}";
                Telefono = $"Tel: {p.pa_telefono ?? "No registrado"}";

                // CORREGIDO: Iniciales sin error CS0411
                var partes = p.us_nombre?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (partes == null || partes.Length == 0)
                    Iniciales = "??";
                else
                {
                    string ini1 = partes[0].Length > 0 ? partes[0][0].ToString().ToUpper() : "?";
                    string ini2 = partes.Length > 1 && partes[1].Length > 0 ? partes[1][0].ToString().ToUpper() : "";
                    Iniciales = ini1 + ini2;
                }
            }
        }
        catch (Exception ex)
        {
            NombreCompleto = "Error al cargar";
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async Task CargarHistorialAsync(int pacienteId)
    {
        try
        {
            var url = $"{UrlBase}?resource=historial&paciente_id={pacienteId}";
            var json = await client.GetStringAsync(url);
            var lista = JsonConvert.DeserializeObject<List<HistorialClinico>>(json);
            Historial.Clear();
            if (lista != null)
                foreach (var h in lista) Historial.Add(h);
        }
        catch { }
    }

    public async Task CargarCitasAsync(int pacienteId, int medicoId)
    {
        try
        {
            var url = $"{UrlBase}?resource=citas_paciente&paciente_id={pacienteId}&medico_id={medicoId}";
            var json = await client.GetStringAsync(url);
            var lista = JsonConvert.DeserializeObject<List<CitaMedico>>(json);
            Citas.Clear();
            if (lista != null)
                foreach (var c in lista) Citas.Add(c);
        }
        catch { }
    }
}