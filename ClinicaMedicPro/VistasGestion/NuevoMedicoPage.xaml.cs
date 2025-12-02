using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.ComponentModel;

namespace ClinicaMedicPro.VistasGestion;

public partial class NuevoMedicoPage : ContentPage, INotifyPropertyChanged
{
    private List<Usuario> TodosUsuarios = new();
    private string _filtro = "";

    public List<Usuario> UsuariosFiltrados => string.IsNullOrWhiteSpace(_filtro)
        ? TodosUsuarios
        : TodosUsuarios.Where(u =>
            u.nombre.Contains(_filtro, StringComparison.OrdinalIgnoreCase) ||
            u.us_correo.Contains(_filtro, StringComparison.OrdinalIgnoreCase)).ToList();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public NuevoMedicoPage()
    {
        InitializeComponent();
        BindingContext = this;
        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        IsBusy = true;
        try
        {
            var client = new HttpClient();

            // Cargar todos los usuarios
            var jsonUsuarios = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=usuario");
            var todos = JsonConvert.DeserializeObject<List<Usuario>>(jsonUsuarios) ?? new();

            // Cargar médicos existentes
            var jsonMedicos = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=medico");
            var medicos = JsonConvert.DeserializeObject<List<Medico>>(jsonMedicos) ?? new();
            var idsMedicos = medicos.Select(m => m.fk_usuario).ToHashSet();

            // Filtrar: solo usuarios que NO son médicos
            TodosUsuarios = todos.Where(u => !idsMedicos.Contains(u.id)).ToList();
            OnPropertyChanged(nameof(UsuariosFiltrados));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar usuarios: " + ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        _filtro = e.NewTextValue ?? "";
        OnPropertyChanged(nameof(UsuariosFiltrados));
    }

    private async void OnUsuarioSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Usuario usuario)
        {
            ((CollectionView)sender).SelectedItem = null;
            await Navigation.PushAsync(new CrearMedicoPage(usuario));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}