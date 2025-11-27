namespace ClinicaMedicPro.Vistas;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        InitializeComponent();
        CargarDatosUsuario();
    }

    private void CargarDatosUsuario()
    {
        var nombre = Preferences.Default.Get("UsuarioNombre", "Administrador");
        var rol = Preferences.Default.Get("UsuarioRol", "admin");

        LabelNombreUsuario.Text = nombre;
        LabelRol.Text = rol.Trim().ToLower() switch
        {
            "admin" => "Administrador del Sistema",
            "medico" => "Médico",
            "paciente" => "Paciente",
            _ => "Usuario"
        };
    }

    // TODOS LOS TAPPED
    private async void OnNotificacionesTapped(object sender, EventArgs e)
        => await DisplayAlert("Notificaciones", "Aquí irán las notificaciones", "OK");

    private async void OnPacientesTapped(object sender, EventArgs e)
        => await DisplayAlert("Pacientes", "Lista de pacientes", "OK");

    private async void OnMedicosTapped(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//GestionMedicosPage");

    private async void OnCitasHoyTapped(object sender, EventArgs e)
        => await DisplayAlert("Citas", "Calendario de hoy", "OK");

    private async void OnIngresosTapped(object sender, EventArgs e)
        => await DisplayAlert("Ingresos", "Reportes financieros", "OK");

    private async void OnGestionUsuariosTapped(object sender, EventArgs e)
        => await DisplayAlert("Usuarios", "Gestión de cuentas", "OK");

    private async void OnGestionMedicosTapped(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//GestionMedicosPage");

    private async void OnCitasTapped(object sender, EventArgs e)
        => await DisplayAlert("Citas", "Calendario completo", "OK");

    private async void OnReportesTapped(object sender, EventArgs e)
        => await DisplayAlert("Reportes", "Estadísticas detalladas", "OK");

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        Preferences.Default.Clear();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}