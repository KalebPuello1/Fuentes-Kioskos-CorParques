using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteInventarioGeneral
    {
        IEnumerable<ReporteInventarioGeneral> ObtenerTodos(ReporteInventarioGeneral modelo);
        IEnumerable<ReporteBoleteria> Obtenerboleteria(string fecha, string usuario);
        IEnumerable<ReporteEntregaPedido> ObtenerEntregasInstitucionales(string fechaEntrega, string fechaUso, string pedido, string asesor, string cliente, string producto);
    }
}
