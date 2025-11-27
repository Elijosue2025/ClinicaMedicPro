using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Text;

namespace ClinicaMedicPro.Service;

public class AuthService
{
    private readonly HttpClient client = new();
    private readonly string loginUrl = $"{ApiConfig.BaseUrl}?resource=usuario&action=login";

    public async Task<Usuario?> LoginAsync(string correo, string password)
    {
        try
        {
            var data = new { us_correo = correo, us_password = password };
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(loginUrl, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var jsonResp = await response.Content.ReadAsStringAsync();

            // Si tu API devuelve algo como { "error": "..." }
            if (jsonResp.Contains("\"error\"") || jsonResp.Contains("error"))
                return null;

            return JsonConvert.DeserializeObject<Usuario>(jsonResp);
        }
        catch
        {
            return null; // Falló la conexión o el servidor
        }
    }
}

