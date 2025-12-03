using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedicPro.Modelos
{
    public class PacienteCompleto
    {
        public int pk_paciente { get; set; }
        public int fk_usuario { get; set; }
        public string? us_nombre { get; set; }
        public string? us_correo { get; set; }
        public string? pa_cedula { get; set; }
        public string? pa_telefono { get; set; }
        public string? pa_direccion { get; set; }
    }
}
