using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteMovimientoInventario
    {
        string[] ObtenerUnidadMedida();
        IEnumerable<TipoMovimiento> ObtenerTiposMovimiento();
        IEnumerable<ReporteMovimientoInventario> ObtenerReporte(ReporteMovimientoInventario modelo);
        IEnumerable<ReportePedidoRestaurante> ObtenerReporteRestaurante(ReportePedidoRestaurante modelo);
        IEnumerable<ReporteBonoRegalo> ObtenerReporteBonoRegalo(ReporteBonoRegalo modelo);
        
    }
}
