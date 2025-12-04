using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;

namespace ClinicaMedicPro.PacienteView;

public partial class MisCitasPacientePage : ContentPage
{
    public ObservableCollection<CitaViewModel> Citas { get; set; } = new();

    public MisCitasPacientePage()
    {
        InitializeComponent();
        BindingContext = this;
        CargarCitas();
    }

    private async void CargarCitas()
    {
        var pacienteId = Preferences.Default.Get("PacienteId", 0);
        if (pacienteId == 0)
        {
            await DisplayAlert("Error", "No se encontró tu ID de paciente", "OK");
            return;
        }

        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=cita&paciente_id={pacienteId}");
            var citasRaw = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json) ?? new();

            Citas.Clear();
            foreach (var c in citasRaw)
            {
                var cita = new CitaViewModel
                {
                    pk_cita = Convert.ToInt32(c["pk_cita"]),
                    ci_fecha = DateTime.Parse(c["ci_fecha"].ToString()),
                    ci_hora = c["ci_hora"]?.ToString() ?? "",
                    ci_motivo = c["ci_motivo"]?.ToString() ?? "Sin motivo",
                    ci_estado = c["ci_estado"]?.ToString() ?? "desconocido",
                    medico_nombre = c.ContainsKey("medico_nombre") ? c["medico_nombre"]?.ToString() : "Médico no asignado",
                    me_especialidad = c.ContainsKey("me_especialidad") ? c["me_especialidad"]?.ToString() : ""
                };
                Citas.Add(cita);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar tus citas: " + ex.Message, "OK");
        }
    }

    private async void OnCancelarCita(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            bool confirmar = await DisplayAlert("Cancelar Cita", "¿Estás seguro de cancelar esta cita?", "Sí", "No");
            if (!confirmar) return;

            var data = new { ci_estado = "cancelada" };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var client = new HttpClient();
                var response = await client.PutAsync($"{ApiConfig.BaseUrl}?resource=cita&id={id}", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Cita cancelada correctamente", "OK");
                    CargarCitas();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo cancelar la cita", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

    private async void OnIrAgendarCita(object sender, EventArgs e)
        => await Navigation.PushAsync(new AgendarCitaPacientePage());
}

public class CitaViewModel : BindableObject
{
    public int pk_cita { get; set; }
    public DateTime ci_fecha { get; set; }
    public string ci_hora { get; set; }
    public string ci_motivo { get; set; }
    public string ci_estado { get; set; }
    public string medico_nombre { get; set; }
    public string me_especialidad { get; set; }

    public Color EstadoColor => ci_estado.ToLower() switch
    {
        "agendada" => Colors.Orange,
        "atendida" => Colors.Green,
        "cancelada" => Colors.Red,
        _ => Colors.Gray
    };

    public bool PuedeCancelar => ci_estado.ToLower() == "agendada";
}