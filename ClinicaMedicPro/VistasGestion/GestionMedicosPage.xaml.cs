using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ClinicaMedicPro.VistasGestion;

public partial class GestionMedicosPage : ContentPage
{
    private readonly HttpClient client = new HttpClient();
    private const string UrlApi = "https://127.0.0.1/wsCitas/api.php?resource=medico"; // CAMBIA
    public ObservableCollection<Medico> Medicos { get; set; } = new();
    public GestionMedicosPage()
    {
        InitializeComponent();
        BindingContext = this;
        CargarMedicos();
    }

    private async void CargarMedicos()
    {
        try
        {
            var json = await client.GetStringAsync(UrlApi);
            var lista = JsonConvert.DeserializeObject<List<Medico>>(json);
            Medicos.Clear();
            foreach (var m in lista ?? new List<Medico>()) Medicos.Add(m);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnNuevoMedicoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NuevoMedicoPage());
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var medico = (Medico)boton.CommandParameter;
        await Navigation.PushAsync(new EditarMedicoPage(medico));
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var medico = (Medico)boton.CommandParameter;
        var respuesta = await DisplayAlert("Confirmar", $"¿Eliminar Dr. {medico.us_nombre}?", "Sí", "No");
        if (respuesta)
        {
            var res = await client.DeleteAsync($"{UrlApi}&id={medico.pk_medico}");
            if (res.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Médico eliminado", "OK");
                CargarMedicos();
            }
            else await DisplayAlert("Error", "No se pudo eliminar", "OK");
        }
    }

    private async void OnHorariosClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var medico = (Medico)boton.CommandParameter;
        await DisplayAlert("Horarios", $"Próximamente para Dr. {medico.us_nombre}", "OK");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarMedicos();
    }
}