using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class PacientesAdminPage : ContentPage
{
    private List<PacienteCompleto> TodosPacientes = new();
    public ObservableCollection<PacienteCompleto> PacientesFiltrados { get; set; } = new();

    public PacientesAdminPage()
    {
        InitializeComponent();
        BindingContext = this;
        CargarPacientes();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarPacientes(); // Refresca al volver
    }

    private async void CargarPacientes()
    {
        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=paciente");
            var lista = JsonConvert.DeserializeObject<List<PacienteCompleto>>(json);

            TodosPacientes = lista ?? new();
            RefrescarLista("");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar pacientes: " + ex.Message, "OK");
        }
    }

    private void OnBuscarChanged(object sender, TextChangedEventArgs e)
    {
        RefrescarLista(e.NewTextValue?.ToLower() ?? "");
    }

    private void RefrescarLista(string filtro)
    {
        PacientesFiltrados.Clear();
        var filtrados = string.IsNullOrEmpty(filtro)
            ? TodosPacientes
            : TodosPacientes.Where(p =>
                (p.us_nombre?.ToLower().Contains(filtro) ?? false) ||
                (p.pa_cedula?.Contains(filtro) ?? false) ||
                (p.us_correo?.ToLower().Contains(filtro) ?? false));

        foreach (var p in filtrados)
            PacientesFiltrados.Add(p);
    }

    // BOTONES DE ACCIÓN
    private async void OnAgendarCitaClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            var paciente = PacientesFiltrados.FirstOrDefault(p => p.pk_paciente == id);
            if (paciente != null)
            {
                await Navigation.PushAsync(new AgendarCitaPage(
                    paciente.pk_paciente,
                    paciente.us_nombre,
                    paciente.us_correo
                ));
            }
        }
    }

    private async void OnVerDetalleClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            await Navigation.PushAsync(new DetallePacientePage(id));
        }
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            await DisplayAlert("Editar", "Funcionalidad en desarrollo", "OK");
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            bool confirmar = await DisplayAlert("Eliminar", "¿Seguro que deseas eliminar este paciente?", "Sí", "No");
            if (confirmar)
            {
                try
                {
                    var client = new HttpClient();
                    var response = await client.DeleteAsync($"{ApiConfig.BaseUrl}?resource=paciente&id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Paciente eliminado", "OK");
                        CargarPacientes();
                    }
                }
                catch
                {
                    await DisplayAlert("Error", "No se pudo eliminar", "OK");
                }
            }
        }
    }

    private async void OnNuevoPacienteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NuevoPacientePage());
    }

}