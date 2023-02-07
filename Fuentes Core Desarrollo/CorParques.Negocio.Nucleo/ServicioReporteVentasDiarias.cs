using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteVentasDiarias : IServicioReporteVentasDiarias
    {
        private readonly IRepositorioReporteVentasDiarias _repositorio;

        public ServicioReporteVentasDiarias(IRepositorioReporteVentasDiarias repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteInventario> ObtenerReporteInventario()
        {
            return _repositorio.ObtenerReporteInventario();
        }

        public IEnumerable<ReporteVentasDiario> ObtenerReporteDiario(String fecha)
        {
            return _repositorio.ObtenerReporteDiario(fecha);
        }
    }
}
