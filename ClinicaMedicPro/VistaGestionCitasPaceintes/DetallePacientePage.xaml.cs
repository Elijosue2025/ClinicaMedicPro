namespace ClinicaMedicPro.VistaGestionCitasPaceintes;

public partial class DetallePacientePage : ContentPage
{
    public DetallePacientePage(int pacienteId)
    {
        InitializeComponent();
        Title = "Detalle del Paciente";
        // Cargar datos del paciente con pacienteId
    }

    public DetallePacientePage() : this(0) { }
}