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
    public class ServicioReporteVentasDirectas : IServicioReporteVentasDirectas
    {
        private readonly IRepositorioReporteVentasDirectas _repositorio;

        public ServicioReporteVentasDirectas(IRepositorioReporteVentasDirectas repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteVentas>[] ObtenerReporte(string FechaInicial, string FechaFinal, int? IdPunto, int? IdTaquillero, int? IdFormaPago, int? IdFranquicia, string CentroBeneficio)
        {
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, IdPunto, IdTaquillero, IdFormaPago, IdFranquicia, CentroBeneficio);
        }
    }
}
