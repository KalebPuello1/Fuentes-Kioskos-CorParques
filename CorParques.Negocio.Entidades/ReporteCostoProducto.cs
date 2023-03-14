using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteCostoProducto
    {
        [Column("codigo")]
        public string codigo { get; set; }

        [Column("nombre")]
        public string nombre { get; set; }

        [Column("valorVentaUnitario")]
        public string valorVentaUnitario { get; set; }

        [Column("cantidad")]
        public string cantidad { get; set; }

        [Column("costoUnitario")]
        public string costoUnitario { get; set; }

        [Column("valorTotal")]
        public string valorTotal { get; set; }

        [Column("costoTotal")]
        public string costoTotal { get; set; }

        [Column("porcentajeCosto")]
        public string porcentajeCosto { get; set; }

        [Editable(false)]
        public string fechaInicial { get; set; }
        [Editable(false)]
        public string fechaFinal { get; set; }
        [Editable(false)]
        public string CodSap { get; set; }
    }
}
