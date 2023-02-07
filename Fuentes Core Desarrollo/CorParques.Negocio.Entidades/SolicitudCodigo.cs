using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
  public  class SolicitudCodigo
    {
        public string Correo { get; set; }
        public string Nombre { get; set; }

        public int Consecutivo { get; set; }

        public DateTime FechaGeneracion { get; set; }
        public TimeSpan HoraGeneracion { get; set; }
        public bool Estado { get; set; }

        public string RespuestaPeticion { get; set; }


    }
}
