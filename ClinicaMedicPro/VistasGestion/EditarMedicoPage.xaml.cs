using ClinicaMedicPro.Modelos;

namespace ClinicaMedicPro.VistasGestion;

public partial class EditarMedicoPage : ContentPage
{
	public EditarMedicoPage()
	{
		InitializeComponent();
	}

    public EditarMedicoPage(Medico medico)
    {
        Medico = medico;
    }

    public Medico Medico { get; }
}