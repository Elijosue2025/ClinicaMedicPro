namespace ClinicaMedicPro.Modelos;

public class CitaDetalle
{
    public int pk_cita { get; set; }
    public int fk_paciente { get; set; }
    public int fk_medico { get; set; }

    public string ci_fecha { get; set; } = "";
    public string ci_hora { get; set; } = "";
    public string ci_motivo { get; set; } = "";
    public string ci_estado { get; set; } = "";

    // Datos del médico que atiende
    public string? medico_nombre { get; set; }
    public string? me_especialidad { get; set; }
}