using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteDonaciones
    {
        IEnumerable<ReporteDonaciones> ObtenerReporte(string FechaInicial, string FechaFinal, string prodfucto, int? punto);
        IEnumerable<TipoGeneral> ObtenerProductos();
    }
}
