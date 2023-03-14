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
    public class ServicioReporteVentasPorHora : IServicioReporteVentasPorHora
    {
        private readonly IRepositorioReporteVentasPorHora _repositorio;

        public ServicioReporteVentasPorHora(IRepositorioReporteVentasPorHora repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteVentasPorHora> ObtenerReporte(string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodigoProducto, string NombreProducto, string CodigoPunto, string CB)
        {            
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, HoraInicial, HoraFinal, CodigoProducto, NombreProducto, CodigoPunto, CB);
        }
    }
}
