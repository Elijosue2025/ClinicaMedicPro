using Newtonsoft.Json;

using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;

namespace ClinicaMedicPro.GestionUsers;

public partial class EditarUsuarioPage : ContentPage
{
    private readonly int _usuarioId;
    private Usuario _usuarioOriginal;

    public EditarUsuarioPage(int usuarioId)
    {
        InitializeComponent();
        _usuarioId = usuarioId;
        _ = CargarUsuarioAsync();
    }

    private async Task CargarUsuarioAsync()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{ApiConfig.BaseUrl}?resource=usuario&id={_usuarioId}");
            var json = await response.Content.ReadAsStringAsync();
            _usuarioOriginal = JsonConvert.DeserializeObject<Usuario>(json);

            if (_usuarioOriginal != null)
            {
                txtNombre.Text = _usuarioOriginal.nombre;
                txtCorreo.Text = _usuarioOriginal.us_correo;
                pickerRol.SelectedItem = _usuarioOriginal.rol;
                txtEspecialidad.Text = _usuarioOriginal.especialidad;

                // Mostrar especialidad solo si es médico
                txtEspecialidad.IsVisible = _usuarioOriginal.rol == "medico";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cargar el usuario: " + ex.Message, "OK");
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtCorreo.Text) ||
            pickerRol.SelectedItem == null)
        {
            await DisplayAlert("Error", "Completa todos los campos obligatorios", "OK");
            return;
        }

        var datosActualizados = new
        {
            us_nombre = txtNombre.Text.Trim(),
            us_correo = txtCorreo.Text.Trim(),
            us_tipo = pickerRol.SelectedItem.ToString(),
            especialidad = pickerRol.SelectedItem.ToString() == "medico" ? txtEspecialidad.Text : null
        };

        try
        {
            using var client = new HttpClient();
            var json = JsonConvert.SerializeObject(datosActualizados);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{ApiConfig.BaseUrl}?resource=usuario&id={_usuarioId}", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario actualizado correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", "No se pudo actualizar: " + error, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Mostrar/ocultar especialidad al cambiar el rol
    protected override void OnAppearing()
    {
        base.OnAppearing();
        pickerRol.SelectedIndexChanged += (s, e) =>
        {
            txtEspecialidad.IsVisible = pickerRol.SelectedItem?.ToString() == "medico";
        };
    }
}