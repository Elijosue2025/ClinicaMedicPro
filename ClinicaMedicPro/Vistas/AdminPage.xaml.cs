namespace ClinicaMedicPro.Vistas;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        InitializeComponent();
    }

    private async void IrUsuarios_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Usuarios", "Aquí irá la lista de usuarios", "OK");

    private async void IrMedicos_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Médicos", "Gestión de médicos", "OK");

    private async void IrCitas_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Citas", "Calendario de citas", "OK");

    private async void IrReportes_Clicked(object sender, EventArgs e)
        => await DisplayAlert("Reportes", "Estadísticas de la clínica", "OK");

    private async void CerrarSesion_Clicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//LoginPage");
}