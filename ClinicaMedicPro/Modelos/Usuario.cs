using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedicPro.Modelos
{
    public class Usuario
    {
        public int id { get; set; }

        [JsonProperty("us_nombre")]
        public string nombre { get; set; }

        [JsonProperty("us_correo")]
        public string usuario { get; set; }  // o correo, como prefieras

        public string clave { get; set; }

        [JsonProperty("us_tipo")]           // ← ESTO ES LO IMPORTANTE
        public string rol { get; set; }     // ← ahora sí se llena con "admin", "paciente", etc.

        // los demás campos si los usas
        public string us_ubicacion { get; set; }
        public string us_creado { get; set; }
    }
}
