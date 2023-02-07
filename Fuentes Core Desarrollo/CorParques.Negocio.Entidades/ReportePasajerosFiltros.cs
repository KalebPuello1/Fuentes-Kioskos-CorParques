using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReportePasajerosFiltros
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int IdPunto { get; set; }
        public int TipoProducto { get; set; }
        public IEnumerable<ReportePasajeros> objReportePasajeros { get; set; }
    }
}
