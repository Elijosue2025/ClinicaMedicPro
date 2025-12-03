using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Text;

namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class AgendarCitaPage : ContentPage
{
    private readonly int _pacienteId;
    private readonly string _pacienteNombre;
    private readonly string _pacienteCorreo;
    private List<Medico> _medicos = new();

    public DateTime Hoy => DateTime.Today;
    public DateTime FechaSeleccionada { get; set; } = DateTime.Today;
    public TimeSpan HoraSeleccionada { get; set; } = TimeSpan.FromHours(9);

    public AgendarCitaPage(int pacienteId, string nombre, string correo)
    {
        InitializeComponent();
        BindingContext = this;

        _pacienteId = pacienteId;
        _pacienteNombre = nombre;
        _pacienteCorreo = correo;

        // CORREGIDO: nombres coinciden con XAML
        lblPaciente.Text = nombre;
        lblCorreo.Text = correo;

        CargarMedicos();
    }

    private async void CargarMedicos()
    {
        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=medico");
            _medicos = JsonConvert.DeserializeObject<List<Medico>>(json) ?? new();

            foreach (var m in _medicos)
            {
                pickerMedicos.Items.Add($"{m.us_nombre} - {m.me_especialidad}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar médicos: " + ex.Message, "OK");
        }
    }

    private async void OnAgendarClicked(object sender, EventArgs e)
    {
        if (pickerMedicos.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Selecciona un médico", "OK");
            return;
        }

        var medicoSeleccionado = _medicos[pickerMedicos.SelectedIndex];

        try
        {
            var citaData = new
            {
                fk_paciente = _pacienteId,
                fk_medico = medicoSeleccionado.pk_medico,
                ci_fecha = FechaSeleccionada.ToString("yyyy-MM-dd"),
                ci_hora = HoraSeleccionada.ToString(@"hh\:mm"),
                ci_motivo = txtMotivo.Text?.Trim() ?? "Sin motivo",
                ci_estado = "agendada"
            };

            var json = JsonConvert.SerializeObject(citaData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync($"{ApiConfig.BaseUrl}?resource=cita", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito",
                    $"Cita agendada con Dr(a). {medicoSeleccionado.us_nombre} para el {FechaSeleccionada:dd/MM/yyyy} a las {HoraSeleccionada:hh\\:mm}",
                    "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo agendar", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}