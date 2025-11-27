using ClinicaMedicPro.Service;
using Newtonsoft.Json;
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

        // GUARDAR DATOS DEL USUARIO (muy importante para las demás páginas)
        Preferences.Default.Set("UsuarioId", usuario.id);
        Preferences.Default.Set("UsuarioNombre", usuario.nombre ?? "Usuario");
        Preferences.Default.Set("UsuarioRol", usuario.rol ?? "paciente");

        // REDIRECCIÓN SEGÚN ROL
        switch (usuario.rol?.Trim().ToLower())
        {
            case "admin":
                await Shell.Current.GoToAsync("//AdminPage");
                break;
            case "medico":
                // ... tu código de medicoId
                await Shell.Current.GoToAsync("//MedicoPage");
                break;
            case "paciente":
                await Shell.Current.GoToAsync("//PacientePage");
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
