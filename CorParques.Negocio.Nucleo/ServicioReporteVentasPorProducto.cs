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
    public class ServicioReporteVentasPorProducto : IServicioReporteVentasPorProducto
    {
        private readonly IRepositorioReporteVentasPorProducto _repositorio;

        public ServicioReporteVentasPorProducto(IRepositorioReporteVentasPorProducto repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteVentasPorProducto> ObtenerReporte(string FechaInicial, string FechaFinal, string CodigoProducto, string CodigoPunto, string CentroBeneficio)
        {            
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, CodigoProducto, CodigoPunto, CentroBeneficio);
        }
    }
}
