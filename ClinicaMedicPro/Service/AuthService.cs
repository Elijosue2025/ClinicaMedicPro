using ClinicaMedicPro.Modelos;
using Newtonsoft.Json;
using System.Text;

public class AuthService
{
    private readonly string baseUrl = "http://localhost/wsCitas/api.php?resource=usuario&action=login";

    public async Task<Usuario?> LoginAsync(string correo, string password)
    {
        using var client = new HttpClient();

        var data = new
        {
            us_correo = correo,
            us_password = password
        };

        string json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(baseUrl, content);
        if (!response.IsSuccessStatusCode)
            return null;

        var jsonResp = await response.Content.ReadAsStringAsync();

        if (jsonResp.Contains("error"))
            return null;

        return JsonConvert.DeserializeObject<Usuario>(jsonResp);
    }
}

