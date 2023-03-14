using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
  public  class ListaCortesia
    {

        public int Id { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public int IdUsuarioVisitante { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Descripcion { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int Cantidad { get; set; }

        public string NumeroTicket { get; set; }

        public string Correo { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }

        public string RutaSoporte { get; set; }

        public bool Activo { get; set; }

        public string Observacion { get; set; }
 
        public int IdTipoCortesia { get; set; }

        public string NombreTipo { get; set; }

        public bool Aprobacion { get; set; }
    }
}
