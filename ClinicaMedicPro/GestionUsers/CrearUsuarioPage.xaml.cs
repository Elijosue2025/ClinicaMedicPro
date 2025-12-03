using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;

namespace ClinicaMedicPro.GestionUsers;

public partial class CrearUsuarioPage : ContentPage
{
    public CrearUsuarioPage()
    {
        InitializeComponent();
    }
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Usuarios");
        // O si prefieres volver al AdminPage:
        // await Shell.Current.GoToAsync("//AdminPage");
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        // Validación
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtCorreo.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            pickerRol.SelectedItem == null)
        {
            await DisplayAlert("Error", "Completa todos los campos obligatorios", "OK");
            return;
        }

        // Validación especialidad si es médico
        if (pickerRol.SelectedItem.ToString() == "medico" && string.IsNullOrWhiteSpace(txtEspecialidad.Text))
        {
            await DisplayAlert("Error", "La especialidad es obligatoria para médicos", "OK");
            return;
        }

        var nuevoUsuario = new
        {
            us_nombre = txtNombre.Text.Trim(),
            us_correo = txtCorreo.Text.Trim(),
            us_password = txtPassword.Text,
            us_tipo = pickerRol.SelectedItem.ToString(),
            especialidad = pickerRol.SelectedItem.ToString() == "medico" ? txtEspecialidad.Text.Trim() : null
        };

        try
        {
            using var client = new HttpClient();
            var json = JsonConvert.SerializeObject(nuevoUsuario);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // USAMOS EL NUEVO ENDPOINT QUE INSERTA EN 2 TABLAS
            var response = await client.PostAsync($"{ApiConfig.BaseUrl}?resource=crear-usuario", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario creado correctamente", "OK");
                await Shell.Current.GoToAsync("//Usuarios"); // Regresa a la lista
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", "No se pudo crear el usuario:\n" + error, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error de conexión:\n" + ex.Message, "OK");
        }
    }
}