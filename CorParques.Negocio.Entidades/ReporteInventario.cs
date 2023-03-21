using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteInventario
    {        
        public DateTime? Fecha { get; set; }
        public string Dependencia { get; set; }
        public string NombreEquipo { get; set; }
        public string Serial { get; set; }
        public string CodigoActivo { get; set; }
        public string IdPuntoSap { get; set; }
        public string Ubicacion { get; set; }
        public string Sede { get; set; }
        public string Oficina { get; set; }

    }
}
