using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Text;

namespace ClinicaMedicPro.PacienteView;

public partial class AgendarCitaPacientePage : ContentPage
{
    private List<Medico> medicos = new();
    private int PacienteId => Preferences.Default.Get("PacienteId", 0);

    public AgendarCitaPacientePage()  // SIN PARÁMETROS
    {
        InitializeComponent();
        CargarMedicos();
    }

    private async void CargarMedicos()
    {
        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=medico");
            medicos = JsonConvert.DeserializeObject<List<Medico>>(json) ?? new();

            PickerMedicos.Items.Clear();
            foreach (var m in medicos)
                PickerMedicos.Items.Add($"{m.us_nombre} - {m.me_especialidad}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se cargaron médicos: " + ex.Message, "OK");
        }
    }

    private async void OnAgendarCitaClicked(object sender, EventArgs e)
    {
        if (PickerMedicos.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Selecciona un médico", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(EditorMotivo.Text))
        {
            await DisplayAlert("Error", "Escribe el motivo", "OK");
            return;
        }

        var medico = medicos[PickerMedicos.SelectedIndex];

        // LA ÚNICA FORMA QUE FUNCIONA EN TODOS LOS DISPOSITIVOS
        string hora24 = $"{TimePickerHora.Time.Hours:D2}:{TimePickerHora.Time.Minutes:D2}";
        // Ejemplo: 09:30, 14:15, 00:05 → siempre formato 24h correcto

        var data = new
        {
            fk_paciente = PacienteId,
            fk_medico = medico.pk_medico,
            ci_fecha = DatePickerFecha.Date.ToString("yyyy-MM-dd"),
            ci_hora = hora24,
            ci_motivo = EditorMotivo.Text.Trim(),
            ci_estado = "agendada"
        };

        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var client = new HttpClient();
            var response = await client.PostAsync($"{ApiConfig.BaseUrl}?resource=cita", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito!",
                    $"Cita agendada con Dr(a). {medico.us_nombre}\n" +
                    $"el {DatePickerFecha.Date:dd/MM/yyyy} a las {hora24}", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error del servidor", error, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}