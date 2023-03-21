

using CorParques.Negocio.Entidades;
using System.Collections.Generic;

namespace Corparques.Datos.Contratos
{
    public interface IRepositorioReporteVentasPorConvenio 
    {
        IEnumerable<ReporteVentasPorConvenio> ObtenerReporte(string FechaInicial, string FechaFinal);
    }
}
