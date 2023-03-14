using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteVentasDiarias
    {
        IEnumerable<ReporteInventario> ObtenerReporteInventario();
        IEnumerable<ReporteVentasDiario> ObtenerReporteDiario(String fecha);
    }
}
