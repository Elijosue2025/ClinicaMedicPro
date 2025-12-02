using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
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
                    usuario_id = _usuario.id,
                    especialidad = especialidad
                };

                var json = JsonConvert.SerializeObject(datos);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync(
                    $"{ApiConfig.BaseUrl}?resource=convertir-medico", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito",
                        $"Dr(a). {_usuario.nombre} ahora es médico ({especialidad})", "OK");

                    await Navigation.PopToRootAsync(); // vuelve y refresca todo
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", error, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de conexión", ex.Message, "OK");
            }
        }
    }
}