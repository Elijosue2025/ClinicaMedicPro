using Newtonsoft.Json;
using System.Text;

namespace ClinicaMedicPro.VistasGestion;

public partial class NuevoMedicoPage : ContentPage
{
    private readonly HttpClient client = new HttpClient();
    private const string UrlApi = "http://10.0.2.2/wsCitas/api.php?resource=medico"; // Cambia si usas otra IP

    public NuevoMedicoPage()
    {
        InitializeComponent();
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtCorreo.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            pickerEspecialidad.SelectedItem == null)
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
            return;
        }

        // Confirmar
        bool confirm = await DisplayAlert("Confirmar",
            $"¿Crear médico:\n{txtNombre.Text}\n{txtCorreo.Text}?", "Sí", "No");
        if (!confirm) return;

        // Crear objeto para enviar
        var nuevoMedico = new
        {
            us_nombre = txtNombre.Text.Trim(),
            us_correo = txtCorreo.Text.Trim().ToLower(),
            us_password = txtPassword.Text,
            us_tipo = "medico",
            me_especialidad = pickerEspecialidad.SelectedItem.ToString()
        };

        try
        {
            var json = JsonConvert.SerializeObject(nuevoMedico);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(UrlApi, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Médico creado correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", "No se pudo crear:\n" + error, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de conexión", ex.Message, "OK");
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}