using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteDestrezaFiltros
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }

        public string CodigoPunto { get; set; }
        public string CodigoSeries { get; set; }

        public string NombreTipoBoleta { get; set; }
        public string NombreCliente { get; set; }
        public string TipoVenta { get; set; }

        public IEnumerable<ReporteDestrezas> ListaReporte { get; set; }
    }
}
