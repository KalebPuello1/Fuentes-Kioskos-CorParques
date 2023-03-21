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
    public class ServicioReportePasajerosAtracciones : IServicioReportePasajerosAtracciones
    {
        private readonly IRepositorioReportePasajerosAtracciones _repositorio;

        public ServicioReportePasajerosAtracciones(IRepositorioReportePasajerosAtracciones repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReportePasajeros> ObtenerReporte(string FechaInicial, string FechaFinal, int? IdPunto, int? IdTipoProducto)
        {
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, IdPunto, IdTipoProducto);
        }
    }
}
