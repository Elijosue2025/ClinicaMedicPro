using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;

namespace ClinicaMedicPro.GestionUsers;

public partial class UsuariosPage : ContentPage
{
    public UsuariosPage()
    {
        InitializeComponent();
        _ = CargarUsuarios();
    }

    private async Task CargarUsuarios()
    {
        try
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=usuario");
            var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new();

            listaUsuarios.ItemsSource = usuarios
                .Select(u => new Usuario
                {
                    id = u.id,
                    nombre = u.nombre,
                    us_correo = u.us_correo,
                    rol = u.rol,
                    especialidad = u.rol == "medico" ? u.especialidad : "—",
                })
                .OrderByDescending(u => u.id)
                .ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar usuarios: " + ex.Message, "OK");
        }
    }

    private async void OnNuevoUsuarioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearUsuarioPage());
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            await Navigation.PushAsync(new EditarUsuarioPage(id));
        }
    }
    private async void OnVolverDashboardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AdminPage");
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            bool confirmar = await DisplayAlert("Eliminar", "¿Estás seguro de eliminar este usuario?", "Sí", "No");
            if (confirmar)
            {
                try
                {
                    using var client = new HttpClient();
                    var response = await client.DeleteAsync($"{ApiConfig.BaseUrl}?resource=usuario&id={id}");
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Usuario eliminado", "OK");
                        await CargarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }
    }
}