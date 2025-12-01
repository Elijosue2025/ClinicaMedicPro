using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;

namespace ClinicaMedicPro.VistasGestion
{
    public partial class NuevoMedicoPage : ContentPage
    {
        private List<Usuario> TodosUsuarios = new();
        private string _filtro = "";
        public List<Usuario> UsuariosFiltrados => string.IsNullOrWhiteSpace(_filtro)
            ? TodosUsuarios
            : TodosUsuarios.Where(u => u.nombre.Contains(_filtro, StringComparison.OrdinalIgnoreCase) ||
                                      u.us_correo.Contains(_filtro, StringComparison.OrdinalIgnoreCase)).ToList();

        public bool IsBusy { get; set; }

        public NuevoMedicoPage()
        {
            InitializeComponent();
            BindingContext = this;
            CargarUsuarios();
        }

        private async void CargarUsuarios()
        {
            try
            {
                IsBusy = true;
                OnPropertyChanged(nameof(IsBusy));

                var client = new HttpClient();
                var json = await client.GetStringAsync("http://127.0.0.1/wsCitas/api.php?resource=usuario");
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);

                // Filtrar: solo usuarios que no son ya médicos (opcional)
                var medicos = await client.GetStringAsync("http://127.0.0.1/wsCitas/api.php?resource=medico");
                var medicosLista = JsonConvert.DeserializeObject<List<Medico>>(medicos);
                var idsMedicos = medicosLista.Select(m => m.fk_usuario).ToHashSet();

                TodosUsuarios = usuarios.Where(u => !idsMedicos.Contains(u.id)).ToList();
                OnPropertyChanged(nameof(UsuariosFiltrados));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudieron cargar usuarios: " + ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private void OnFiltroChanged(object sender, TextChangedEventArgs e)
        {
            _filtro = e.NewTextValue ?? "";
            OnPropertyChanged(nameof(UsuariosFiltrados));
        }

        private async void OnUsuarioTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Usuario usuario)
            {
                await Navigation.PushAsync(new CrearMedicoPage(usuario));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }

    // Modelo simple

}