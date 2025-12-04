using ClinicaMedicPro.Service;
using ClinicaMedicPro.Modelos;
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

        // GUARDAR DATOS COMUNES
        Preferences.Default.Set("UsuarioId", usuario.id);
        Preferences.Default.Set("UsuarioNombre", usuario.nombre ?? "Usuario");
        Preferences.Default.Set("UsuarioRol", usuario.rol?.Trim().ToLower() ?? "paciente");

        // SI ES PACIENTE → OBTENER SU pk_paciente
        if (usuario.rol?.Trim().ToLower() == "paciente")
        {
            int pacienteId = await ObtenerPacienteId(usuario.id);
            if (pacienteId > 0)
            {
                Preferences.Default.Set("PacienteId", pacienteId);
            }
            else
            {
                await DisplayAlert("Error", "No se encontró tu perfil de paciente", "OK");
                return;
            }
        }

        // SI ES MÉDICO → OBTENER SU pk_medico
        if (usuario.rol?.Trim().ToLower() == "medico")
        {
            int medicoId = await ObtenerMedicoId(usuario.id);
            if (medicoId > 0)
            {
                Preferences.Default.Set("MedicoId", medicoId);
                Preferences.Default.Set("Especialidad", usuario.especialidad ?? "General");
            }
        }

        // REDIRECCIÓN SEGÚN ROL
        switch (Preferences.Default.Get("UsuarioRol", "paciente"))
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
                await DisplayAlert("Error", "Rol desconocido", "OK");
                break;
        }
    }

    // MÉTODOS PARA OBTENER IDs DEL USUARIO
    private async Task<int> ObtenerPacienteId(int usuarioId)
    {
        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=paciente&usuario_id={usuarioId}");
            var lista = JsonConvert.DeserializeObject<List<PacienteCompleto>>(json);
            return lista?.FirstOrDefault()?.pk_paciente ?? 0;
        }
        catch { return 0; }
    }

    private async Task<int> ObtenerMedicoId(int usuarioId)
    {
        try
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync($"{ApiConfig.BaseUrl}?resource=medico&usuario_id={usuarioId}");
            var lista = JsonConvert.DeserializeObject<List<Medico>>(json);
            return lista?.FirstOrDefault()?.pk_medico ?? 0;
        }
        catch { return 0; }
    }

    private async void OnRegistroClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("RegistroPage");

    private void OnHuellaClicked(object sender, EventArgs e)
    {
        // Biometría futura
    }

    private void OnGoogleLoginClicked(object sender, EventArgs e)
    {
        // Google Auth futuro
    }
}

