using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteDonaciones
    {
        IEnumerable<ReporteDonaciones> ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string producto = null, int? punto = null);
        IEnumerable<TipoGeneral> ObtenerProductos();

    }
}
