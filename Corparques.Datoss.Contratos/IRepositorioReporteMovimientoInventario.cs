using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteMovimientoInventario
    {
        string[] ObtenerUnidadMedida();
        IEnumerable<TipoMovimiento> ObtenerTiposMovimiento();

        IEnumerable<ReportePedidoRestaurante> ObtenerReporteRestaurante(ReportePedidoRestaurante modelo);
        IEnumerable<ReporteBonoRegalo> ObtenerReporteBonoRegalo(ReporteBonoRegalo modelo);


        IEnumerable<ReporteMovimientoInventario> ObtenerReporte(ReporteMovimientoInventario modelo);
    }
}
