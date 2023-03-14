using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    public class AcompanamientoXProducto
    {
        public int IdProducto { get; set; }
        public int IdAcomp { get; set; }
        public int MaxAcompanamientos { get; set; }
        public string Nombre { get; set; }
        public int Id_TipoAcomp { get; set; }

        public int Max_Acompanamiento { get; set; }
        public int Max_AcompBebidas { get; set; }
    }
}
