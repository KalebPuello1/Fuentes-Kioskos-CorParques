using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteMovientoInventario : IServicioReporteMovimientoInventario
    {
        private readonly IRepositorioReporteMovimientoInventario _repositorio;
        public ServicioReporteMovientoInventario(IRepositorioReporteMovimientoInventario repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteMovimientoInventario> ObtenerReporte(ReporteMovimientoInventario modelo)
        {
            return _repositorio.ObtenerReporte(modelo);
        }
        public IEnumerable<ReportePedidoRestaurante> ObtenerReporteRestaurante(ReportePedidoRestaurante modelo)
        {
            return _repositorio.ObtenerReporteRestaurante(modelo);
        }

        public IEnumerable<ReporteBonoRegalo> ObtenerReporteBonoRegalo(ReporteBonoRegalo modelo)
        {
            return _repositorio.ObtenerReporteBonoRegalo(modelo);
        }

        
        public IEnumerable<TipoMovimiento> ObtenerTiposMovimiento()
        {
            return _repositorio.ObtenerTiposMovimiento();
        }

        public string[] ObtenerUnidadMedida()
        {
            return _repositorio.ObtenerUnidadMedida();
        }
    }
}
