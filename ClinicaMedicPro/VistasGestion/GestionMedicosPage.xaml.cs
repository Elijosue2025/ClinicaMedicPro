// GestionMedicosPage.xaml.cs
using ClinicaMedicPro.Modelos;
using System.Collections.ObjectModel;

namespace ClinicaMedicPro.VistasGestion
{
    public partial class GestionMedicosPage : ContentPage
    {
        public ObservableCollection<Medico> Medicos { get; set; } = new();

        public GestionMedicosPage()
        {
            InitializeComponent();
            BindingContext = this;
            CargarMedicos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarMedicos();
        }

        private async void CargarMedicos()
        {
            try
            {
                var client = new HttpClient();
                var json = await client.GetStringAsync("http://127.0.0.1/wsCitas/api.php?resource=medico");
                var lista = System.Text.Json.JsonSerializer.Deserialize<List<Medico>>(json,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Medicos.Clear();
                foreach (var m in lista ?? new List<Medico>())
                    Medicos.Add(m);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudieron cargar los médicos: " + ex.Message, "OK");
            }
        }

        private async void OnNuevoMedicoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NuevoMedicoPage());
        }

        // AQUÍ ESTABAN FALTANDO ESTOS TRES MÉTODOS
        private async void OnHorariosClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int id)
            {
                var medico = Medicos.FirstOrDefault(m => m.pk_medico == id);
                await DisplayAlert("Horarios",
                    $"Funcionalidad de horarios para Dr(a). {medico?.us_nombre} próximamente", "OK");
            }
        }

        private async void OnEditarClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int id)
            {
                var medico = Medicos.FirstOrDefault(m => m.pk_medico == id);
                if (medico != null)
                {
                    await Navigation.PushAsync(new EditarMedicoPage(medico));
                }
            }
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is int id)
            {
                var medico = Medicos.FirstOrDefault(m => m.pk_medico == id);
                if (medico == null) return;

                bool confirmar = await DisplayAlert("Eliminar médico",
                    $"¿Estás seguro de eliminar al Dr(a). {medico.us_nombre}?",
                    "Sí, eliminar", "Cancelar");

                if (!confirmar) return;

                try
                {
                    var client = new HttpClient();
                    var response = await client.DeleteAsync($"http://127.0.0.1/wsCitas/api.php?resource=medico&id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Médico eliminado correctamente", "OK");
                        CargarMedicos();
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error", error, "OK");
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