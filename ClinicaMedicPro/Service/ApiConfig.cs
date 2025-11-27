// ApiConfig.cs
using Microsoft.Maui.Devices;

// ApiConfig.cs
using Microsoft.Maui.Devices;

namespace ClinicaMedicPro;

public static class ApiConfig
{
    public static string BaseUrl
    {
        get
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                return DeviceInfo.Current.DeviceType == DeviceType.Physical
                    ? "http://192.168.1.55/wsCitas/api.php"   // CAMBIA ESTA IP
                    : "http://10.0.2.2/wsCitas/api.php";
            }

            // En Windows siempre es localhost
            if (OperatingSystem.IsWindows())
                return "http://localhost/wsCitas/api.php";

            // En iOS/Mac o cualquier otro → IP del PC
            return "http://192.168.1.55/wsCitas/api.php";
        }
    }
}