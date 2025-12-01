using ClinicaMedicPro.Modelos;
using System.Text.Json;
using System.Windows.Input;


namespace ClinicaMedicPro.VistasGestion
{
    public partial class EditarMedicoPage : ContentPage
    {
        private readonly EditarMedicoViewModel _vm;

        public EditarMedicoPage(Medico medico, Action onSuccess = null)
        {
            InitializeComponent();
            _vm = new EditarMedicoViewModel(medico, onSuccess);
            BindingContext = _vm;
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    // ViewModel separado (MVVM puro)
    public class EditarMedicoViewModel : BindableObject
    {
        private readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "http://127.0.0.1/wsCitas/api.php";
        private readonly Action _onSuccess;

        public Medico Medico { get; set; }
        public string Especialidad
        {
            get => Medico.me_especialidad ?? string.Empty;
            set
            {
                if (Medico.me_especialidad != value)
                {
                    Medico.me_especialidad = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotBusy)); }
        }

        public bool IsNotBusy => !IsBusy;

        public ICommand GuardarCommand { get; }

        public EditarMedicoViewModel(Medico medico, Action onSuccess)
        {
            Medico = medico ?? throw new ArgumentNullException(nameof(medico));
            _onSuccess = onSuccess;
            GuardarCommand = new Command(async () => await GuardarCambios(), () => !IsBusy);
        }

        private async Task GuardarCambios()
        {
            if (string.IsNullOrWhiteSpace(Especialidad.Trim()))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La especialidad es obligatoria", "OK");
                return;
            }

            IsBusy = true;
            ((Command)GuardarCommand).ChangeCanExecute();

            try
            {
                var datos = new { me_especialidad = Especialidad.Trim() };
                var json = JsonSerializer.Serialize(datos);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var url = $"{BaseUrl}?resource=medico&id={Medico.pk_medico}";
                var response = await _client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Médico actualizado correctamente", "OK");
                    _onSuccess?.Invoke();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", error, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                ((Command)GuardarCommand).ChangeCanExecute();
            }
        }
    }
}