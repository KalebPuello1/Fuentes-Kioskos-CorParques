using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteNotificaciones
    {     
        public string Remitente { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string Receptor { get; set; }
        public string FechaRecepcion { get; set; }          
    }

    public class ReporteNotificacionesFiltros
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
    }

}
