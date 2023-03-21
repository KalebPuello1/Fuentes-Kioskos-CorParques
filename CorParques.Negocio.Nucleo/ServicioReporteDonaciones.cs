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
    public class ServicioReporteDonaciones : IServicioReporteDonaciones
    {
        private readonly IRepositorioReporteDonaciones _repositorio;

        public ServicioReporteDonaciones(IRepositorioReporteDonaciones repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<TipoGeneral> ObtenerProductos()
        {
            return _repositorio.ObtenerProductos();
        }

        public IEnumerable<ReporteDonaciones> ObtenerReporte(string FechaInicial, string FechaFinal, string producto,  int? punto)
        {
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, producto, punto);
        }
    }
}
