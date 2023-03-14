using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentasPorHoraFiltros
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoPunto { get; set; }
        public string CB { get; set; }
        public IEnumerable<ReporteVentasPorHora> objReporteVentasPorHora { get; set; }
    }
}
