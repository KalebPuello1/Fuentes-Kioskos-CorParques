using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteRedFechaAbierta
    {
        [Editable(false)]
        [Column("NumeroFactura")]
        public string NumeroFactura { get; set; }
        [Column("FechRed")]
        public string FechRed { get; set; }

        [Column("NumSolicPed")]
        public string NumSolicPed { get; set; }

        [Column("Producto")]
        public string Producto { get; set; }

        [Editable(false)]
        public double? ValorBruto { get; set; }

        [Editable(false)]
        public double? Impuesto { get; set; }

        [Editable(false)]
        public double? Total { get; set; }

        [Column("CantTicketVend")]
        public string CantTicketVend { get; set; }

        [Column("CantTicketRed")]
        public string CantTicketRed { get; set; }

        [Column("CantTicketNoRed")]
        public string CantTicketNoRed { get; set; }

        [Column("Asesor")]
        public string Asesor { get; set; }
        [Editable(false)]
        public string Canal { get; set; }

        [Editable(false)]
        public string fechaInicial { get; set; }
        [Editable(false)]
        public string fechaFinal { get; set; }
        [Editable(false)]
        public string SapAsesor { get; set; }
        [Editable(false)]
        public string SapTipoProducto { get; set; }

        public string codsappedido { get; set; }

        public string CodSapTipoImpuesto { get; set; }

        public int TotalVencido { get; set; }

        public string codsapproducto { get; set; }

        public string nombre { get; set;  }

        public DateTime Fecha { get; set; }

        public int final { get; set; }

        public int SumValUno { get; set; }

        public int SumValDos { get; set; }

        public int Valor { get; set; }

        public int Dato { get; set; }

    }
}
