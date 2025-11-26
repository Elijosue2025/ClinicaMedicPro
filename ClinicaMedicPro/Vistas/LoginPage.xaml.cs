using ClinicaMedicPro.Service;
namespace ClinicaMedicPro.Vistas;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string correo = EntryCorreo.Text?.Trim();
        string clave = EntryPassword.Text?.Trim();

        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
        {
            await DisplayAlert("Error", "Ingrese correo y contraseña", "OK");
            return;
        }

        var auth = new AuthService();
        var usuario = await auth.LoginAsync(correo, clave);

        if (usuario == null)
        {
            await DisplayAlert("Error", "Credenciales incorrectas", "OK");
            return;
        }
        await DisplayAlert("Bienvenido", $"Hola {usuario.nombre}", "OK");

        // Redirección según rol
        switch (usuario.rol?.Trim().ToLower())  // ? con ? para seguridad extra
        {
            case "admin":
                await Shell.Current.GoToAsync("//AdminPage");
                break;
            case "medico":
                    await Shell.Current.GoToAsync("//MedicoPage");
                break;
            case "paciente":
                await Shell.Current.GoToAsync("//PacientePage");
                break;
            default:
                await DisplayAlert("Error", "Rol no válido", "OK");
                break;
        }

    }

    private void OnRegistroClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("RegistroPage");
    }

    private void OnHuellaClicked(object sender, EventArgs e)
    {
        // Luego agregamos biometría.
    }

    private void OnGoogleLoginClicked(object sender, EventArgs e)
    {
        // Luego agregamos Google Auth.
    }
}
