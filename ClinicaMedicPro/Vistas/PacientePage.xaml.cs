using ClinicaMedicPro.PacienteView;
using Newtonsoft.Json;

namespace ClinicaMedicPro.Vistas;

public partial class PacientePage : ContentPage
{
    public PacientePage()
    {
        InitializeComponent();
        CargarDatosPaciente();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarDatosPaciente();
    }


    private async void CargarDatosPaciente()
    {
        // Nombre del paciente
        var nombre = Preferences.Default.Get("UsuarioNombre", "Paciente");
        LabelNombreUsuario.Text = nombre;

        // ID del paciente (guardado en el login)
        var pacienteId = Preferences.Default.Get("PacienteId", 0);
        if (pacienteId == 0)
        {
            LabelTotalCitas.Text = "0";
            LabelCitasPendientes.Text = "0";
            return;
        }

        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=cita&paciente_id={pacienteId}");
            var citas = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json) ?? new();

            LabelTotalCitas.Text = citas.Count.ToString();
            LabelCitasPendientes.Text = citas.Count(c =>
                c.ContainsKey("ci_estado") && c["ci_estado"]?.ToString() == "agendada").ToString();
        }
        catch
        {
            LabelTotalCitas.Text = "0";
            LabelCitasPendientes.Text = "0";
        }
    }

    private async void OnMisCitasTapped(object sender, EventArgs e)
        => await Navigation.PushAsync(new MisCitasPacientePage());

    private async void OnAgendarCitaTapped(object sender, EventArgs e)
    {
        // PÁGINA SIN PARÁMETROS (para el paciente logueado)
        await Navigation.PushAsync(new AgendarCitaPacientePage());
    }

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Cerrar Sesión", "¿Estás seguro?", "Sí", "No"))
        {
            Preferences.Default.Clear();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}