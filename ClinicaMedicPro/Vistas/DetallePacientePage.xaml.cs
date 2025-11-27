namespace ClinicaMedicPro.Vistas;

public partial class DetallePacientePage : ContentPage
{
    private readonly PacienteDetalleViewModel vm;

    public DetallePacientePage(int pacienteId)
    {
        InitializeComponent();
        vm = new PacienteDetalleViewModel();
        BindingContext = vm;

        _ = CargarDatosAsync(pacienteId);
    }

    private async Task CargarDatosAsync(int pacienteId)
    {
        int medicoId = Preferences.Default.Get("MedicoId", 1);
        await vm.CargarPacienteAsync(pacienteId);
        await vm.CargarHistorialAsync(pacienteId);
        await vm.CargarCitasAsync(pacienteId, medicoId);
    }
}