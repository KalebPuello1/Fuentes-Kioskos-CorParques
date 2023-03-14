using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
   public class Acompanamiento
    {
        public int IdProducto { get; set; }
        public int IdAcomp { get; set; }
        public int Consecutivo { get; set; }
        public string Nombre { get; set; }
        public string Observaciones { get; set; }

        public string NombreTipo { get; set; }

        public int TipoEntrega { get; set; }
    }
}
