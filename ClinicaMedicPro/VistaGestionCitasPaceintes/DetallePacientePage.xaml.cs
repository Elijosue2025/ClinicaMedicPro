using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;

namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class DetallePacientePage : ContentPage
{
    private readonly int _pacienteId;

    public DetallePacientePage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        _ = CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            using var client = new HttpClient();

            // Cargar paciente
            var jsonPaciente = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=paciente&id={_pacienteId}");
            var paciente = JsonConvert.DeserializeObject<PacienteCompleto>(jsonPaciente);

            if (paciente != null)
            {
                Title = $"Paciente: {paciente.us_nombre}";
                BindingContext = new
                {
                    us_nombre = paciente.us_nombre,
                    us_correo = paciente.us_correo,
                    pa_cedula = paciente.pa_cedula,
                    pa_telefono = paciente.pa_telefono,
                    pa_direccion = paciente.pa_direccion,
                    Iniciales = string.Concat(paciente.us_nombre.Split(' ').Take(2).Select(n => n[0])).ToUpper()
                };
            }

            // CARGAR CITAS DEL PACIENTE
            var jsonCitas = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=cita&paciente_id={_pacienteId}");
            var citas = JsonConvert.DeserializeObject<List<CitaDetalle>>(jsonCitas) ?? new();

            listaCitas.ItemsSource = citas.Any() ? citas : new List<CitaDetalle>();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}