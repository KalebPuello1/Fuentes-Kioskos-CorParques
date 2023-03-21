using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentasFiltros
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime? FechaCompra { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? IdTaquillero { get; set; }
        public int? IdPunto { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdFranquicia { get; set; }
        public string CentroBeneficio { get; set; }
        public IEnumerable<ReporteVentas> objReporteVentas { get; set; }
        public string idProducto { get; set; }
    }
}
