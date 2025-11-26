using ClinicaMedicPro.Vistas;

namespace ClinicaMedicPro
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AdminPage", typeof(AdminPage));
            Routing.RegisterRoute("MedicoPage", typeof(MedicoPage));
            Routing.RegisterRoute("PacientePage", typeof(PacientePage));
        }
    }
}
