using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteInventarioGeneral
    {
        IEnumerable<ReporteInventarioGeneral> ObtenerTodos(ReporteInventarioGeneral modleo);
        IEnumerable<ReporteBoleteria> Obtenerboleteria(string fecha,string usuario);
        IEnumerable<ReporteEntregaPedido> ObtenerEntregasInstitucionales(string fechaEntrega, string fechaUso, string pedido, string asesor, string cliente, string producto);
    }
}
