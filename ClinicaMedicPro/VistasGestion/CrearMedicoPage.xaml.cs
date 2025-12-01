using ClinicaMedicPro.Modelos;
using System.Text;
using System.Text.Json;

namespace ClinicaMedicPro.VistasGestion
{
    public partial class CrearMedicoPage : ContentPage
    {
        private readonly Usuario _usuario;

        public CrearMedicoPage(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;

            // Mostrar datos del usuario seleccionado
            lblNombre.Text = usuario.nombre;
            lblCorreo.Text = usuario.us_correo;
        }

        private async void OnCrearMedicoClicked(object sender, EventArgs e)
        {
            string especialidad = txtEspecialidad.Text?.Trim();

            if (string.IsNullOrWhiteSpace(especialidad))
            {
                await DisplayAlert("Error", "La especialidad es obligatoria", "OK");
                return;
            }

            try
            {
                var datos = new
                {
                    fk_usuario = _usuario.id,
                    me_especialidad = especialidad
                };

                var json = JsonSerializer.Serialize(datos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync(
                    "http://127.0.0.1/wsCitas/api.php?resource=medico", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", 
                        $"Dr(a). {_usuario.nombre} creado como médico correctamente", "OK");
                    
                    // Regresa a la lista de médicos
                    await Navigation.PopToRootAsync();
                    // O si solo quieres volver una página:
                    // await Navigation.PopAsync();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error del servidor", error, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}