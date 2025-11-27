// ApiConfig.cs
namespace ClinicaMedicPro;

public static class ApiConfig
{
    public static string BaseUrl
    {
        get
        {
            // EN WINDOWS (tu PC) → localhost funciona
            if (OperatingSystem.IsWindows())
                return "http://localhost/wsCitas/api.php";

            // EN ANDROID FÍSICO → tu IP real
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                return DeviceInfo.DeviceType == DeviceType.Physical
                    ? "http://192.168.1.55/wsCitas/api.php"   // ← TU IP REAL (cámbiala si es necesario)
                    : "http://10.0.2.2/wsCitas/api.php";      // Emulador
            }

            // iOS o Mac
            return "http://192.168.1.55/wsCitas/api.php";
        }
    }
}