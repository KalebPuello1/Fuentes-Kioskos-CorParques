using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteReservaEspacio
    {
        [Column("Fecha")]
        public string Fecha { get; set; }
        [Column("TipoReserva")]
        public string TipoReserva { get; set; }
        [Column("HoraInicio")]
        public string HoraInicio { get; set; }
        [Column("HoraFin")]
        public string HoraFin { get; set; }
        [Column("NumeroReserva")]
        public string NumeroReserva { get; set; }
        [Column("NumeroPedido")]
        public string NumeroPedido { get; set; }
        [Column("Asesor")]
        public string Asesor { get; set; }
        [Column("Cliente")]
        public string Cliente { get; set; }
        [Column("TipoEspacio")]
        public string TipoEspacio { get; set; }
        [Column("Espacio")]
        public string Espacio { get; set; }        
        [Column("productos")]
        public string productos { get; set; }
        [Column("Cantidad")]
        public string Cantidad { get; set; }
        [Column("Observaciones")]
        public string Observaciones { get; set; }

        [Editable(false)]
        public string fechaInicialGet { get; set; }

        [Editable(false)]
        public string fechaFinalGet { get; set; }

        [Editable(false)]
        public int idEspGet { get; set; }

        [Editable(false)]
        public int idTipEpsGet { get; set; }

        [Editable(false)]
        public string horaIniGet { get; set; }

        [Editable(false)]
        public string horaFinGet { get; set; }

        [Editable(false)]
        public string txtNPedidoGet { get; set; }

    }
}
