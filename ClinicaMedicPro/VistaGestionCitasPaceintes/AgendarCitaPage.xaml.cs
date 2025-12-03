namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class AgendarCitaPage : ContentPage
{
    public AgendarCitaPage(int pacienteId)
    {
        InitializeComponent();
        // Aquí puedes usar pacienteId
        Title = "Agendar Cita";
    }

    public AgendarCitaPage() : this(0) { } // Constructor sin parámetros (por si acaso)
}