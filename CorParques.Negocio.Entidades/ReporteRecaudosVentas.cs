using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteRecaudosVentas
    {
        [Column("con")]
        public string Concecutivo { get; set; }

        [Column("fec")]
        public string Fecha { get; set; }

        [Column("cli")]
        public string Cliente{ get; set; }

        [Column("val")]
        public string Valor { get; set; }

        [Column("for")]
        public string FormaPago { get; set; }

        [Column("num")]
        public string NumeroReferencia { get; set; }

        [Column("ent")]
        public string Entidad { get; set; }

        [Column("fra")]
        public string Franquicia { get; set; }

        [Column("obs")]
        public string Observaciones { get; set; }

        [Editable(false)]
        public string _FechaInicial { get; set; }

        [Editable(false)]
        public string _FechaFinal { get; set; }

        [Editable(false)]
        public int _Consecutivo { get; set; }

        [Editable(false)]
        public string _Cliente { get; set; }

        [Editable(false)]
        public int _FormaPago { get; set; }

        [Editable(false)]
        public int _Entidad { get; set; }
    }
}
