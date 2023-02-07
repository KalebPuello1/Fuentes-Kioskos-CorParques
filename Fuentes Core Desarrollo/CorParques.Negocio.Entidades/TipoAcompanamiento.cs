using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
   public class TipoAcompanamiento
    {
        public int Id{ get; set; }
        public int IdTipoAcomp { get; set; }
        public string NombreTipo { get; set; }
        public int Estado { get; set; }
        public int Max_Acompanamiento { get; set; }
        public string Nombre { get; set; }

    }
}
