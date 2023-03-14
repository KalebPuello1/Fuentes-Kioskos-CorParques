using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteCostoProducto
    {
        IEnumerable<ReporteCostoProducto> Obtener(ReporteCostoProducto modleo);
        IEnumerable<TipoGeneral> obtenerProductos();
    }
}
