using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Text;

namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class CrearPacientePage : ContentPage
{
    private readonly Usuario _usuario;

    public CrearPacientePage(Usuario usuario)
    {
            InitializeComponent();
        _usuario = usuario;
        lblNombre.Text = _usuario.nombre;
        lblCorreo.Text = _usuario.us_correo;
    }

    private async void OnCrearPacienteClicked(object sender, EventArgs e)
    {
        string cedula = txtCedula.Text?.Trim();
        if (string.IsNullOrWhiteSpace(cedula))
        {
            await DisplayAlert("Error", "La cédula es obligatoria", "OK");
            return;
        }

        try
        {
            using var client = new HttpClient();

            // 1. Crear paciente
            var pacienteData = new
            {
                fk_usuario = _usuario.id,
                pa_cedula = cedula,
                pa_telefono = txtTelefono.Text?.Trim(),
                pa_direccion = txtDireccion.Text?.Trim()
            };

            var jsonPaciente = JsonConvert.SerializeObject(pacienteData);
            var contentPaciente = new StringContent(jsonPaciente, Encoding.UTF8, "application/json");
            var responsePaciente = await client.PostAsync($"{ApiConfig.BaseUrl}?resource=paciente", contentPaciente);

            // 2. SOLO ACTUALIZAR EL ROL (no se borra nombre ni correo)
            var rolData = new { us_tipo = "paciente" };
            var jsonRol = JsonConvert.SerializeObject(rolData);
            var contentRol = new StringContent(jsonRol, Encoding.UTF8, "application/json");
            await client.PutAsync($"{ApiConfig.BaseUrl}?resource=usuario&id={_usuario.id}", contentRol);

            if (responsePaciente.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito",
                    $"{_usuario.nombre} ahora es paciente", "OK");
                await Navigation.PopToRootAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}