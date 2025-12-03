using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class NuevoPacientePage : ContentPage
{
    private List<Usuario> TodosUsuarios = new();
    public ObservableCollection<Usuario> UsuariosFiltrados { get; set; } = new();

    public NuevoPacientePage()
    {
        InitializeComponent();
        BindingContext = this;
        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        try
        {
            var client = new HttpClient();
            var jsonUsuarios = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=usuario");
            var todos = JsonConvert.DeserializeObject<List<Usuario>>(jsonUsuarios) ?? new();

            // Cargar pacientes existentes para filtrar
            var jsonPacientes = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=paciente");
            var pacientes = JsonConvert.DeserializeObject<List<PacienteCompleto>>(jsonPacientes) ?? new();
            var idsPacientes = pacientes.Select(p => p.fk_usuario).ToHashSet();

            // Cargar médicos para filtrar
            var jsonMedicos = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=medico");
            var medicos = JsonConvert.DeserializeObject<List<Medico>>(jsonMedicos) ?? new();
            var idsMedicos = medicos.Select(m => m.fk_usuario).ToHashSet();

            // Filtrar usuarios que NO son pacientes ni médicos
            TodosUsuarios = todos.Where(u => !idsPacientes.Contains(u.id) && !idsMedicos.Contains(u.id)).ToList();
            RefrescarLista("");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        RefrescarLista(e.NewTextValue?.ToLower() ?? "");
    }

    private void RefrescarLista(string filtro)
    {
        UsuariosFiltrados.Clear();
        var filtrados = string.IsNullOrEmpty(filtro)
            ? TodosUsuarios
            : TodosUsuarios.Where(u =>
                u.nombre?.ToLower().Contains(filtro) == true ||
                u.us_correo?.ToLower().Contains(filtro) == true);

        foreach (var u in filtrados)
            UsuariosFiltrados.Add(u);
    }

    private async void OnUsuarioSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Usuario usuario)
        {
            ((CollectionView)sender).SelectedItem = null;
            await Navigation.PushAsync(new CrearPacientePage(usuario));
        }
    }
}