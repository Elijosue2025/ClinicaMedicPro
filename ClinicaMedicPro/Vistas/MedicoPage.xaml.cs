using ClinicaMedicPro.Vistas;

namespace ClinicaMedicPro.Vistas;

public partial class MedicoPage : ContentPage
{
    public MedicoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Cargar el nombre desde Preferences (o desde tu AuthService)
        var nombre = Preferences.Default.Get("UsuarioNombre", "Médico");
        BindingContext = new { NombreMedico = nombre };
    }

    private async void IrAgendarCita_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Citas", "Agendar nueva cita", "OK");

    private async void IrProfesionales_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Profesionales", "Lista de médicos", "OK");

    private async void IrMisPacientes_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Pacientes", "Mis pacientes asignados", "OK");

    private async void IrUbicacion_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Ubicación", "Ver mapa de la clínica", "OK");

    private async void IrChat_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Chat", "Abrir chat en tiempo real", "OK");

    private async void CerrarSesion_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Remove("UsuarioNombre");
        Preferences.Default.Remove("UsuarioRol");
        await Shell.Current.GoToAsync("//LoginPage");
    }
}