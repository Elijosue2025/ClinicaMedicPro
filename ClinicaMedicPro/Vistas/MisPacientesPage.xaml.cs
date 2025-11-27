using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;  // ← TU LIBRERÍA
using System.Collections.ObjectModel;

namespace ClinicaMedicPro.Vistas;  // ← O ClinicaMedicPro si usas opción 1

public partial class MisPacientesPage : ContentPage
{
    private readonly HttpClient client = new();
    private const string UrlBase = "http://192.168.1.55/wsCitas/api.php";

    public ObservableCollection<PacienteMedico> Pacientes { get; set; } = new();

    public MisPacientesPage()
    {
        InitializeComponent();
        BindingContext = this;
        _ = CargarPacientes();  // async void → mejor con _
    }

    private async Task CargarPacientes()
    {
        try
        {
            int medicoId = Preferences.Default.Get("MedicoId", 1);
            var url = $"{UrlBase}?resource=pacientes&medico_id={medicoId}";
            var json = await client.GetStringAsync(url);

            var lista = JsonConvert.DeserializeObject<List<PacienteMedico>>(json);
            Pacientes.Clear();
            if (lista != null)
                foreach (var p in lista) Pacientes.Add(p);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar los pacientes: " + ex.Message, "OK");
        }
    }

    private async void OnPacienteTapped(object sender, EventArgs e)
    {
        if (sender is not Label lbl) return;
        if (lbl.BindingContext is not PacienteMedico paciente) return;

        await Shell.Current.GoToAsync($"DetallePacientePage?pacienteId={paciente.pk_paciente}");
        // O si usas Navigation: await Navigation.PushAsync(new DetallePacientePage(paciente.pk_paciente));
    }
}