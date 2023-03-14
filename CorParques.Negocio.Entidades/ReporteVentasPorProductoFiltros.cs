using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentasPorProductoFiltros
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoPunto { get; set; }
        public string CentroBeneficio { get; set; }
        public IEnumerable<ReporteVentasPorProducto> objReporteVentasPorProducto { get; set; }
    }
}
