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
    public class ServicioReporteInventarioGeneral : IServicioReporteInventarioGeneral
    {
        private readonly IRepositorioReporteInventarioGeneral _respositorio;

        public ServicioReporteInventarioGeneral(IRepositorioReporteInventarioGeneral repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<ReporteInventarioGeneral> ObtenerTodos(ReporteInventarioGeneral modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }

        public IEnumerable<ReporteBoleteria> Obtenerboleteria(string fecha,string usuario)
        {
            return _respositorio.Obtenerboleteria(fecha,usuario);
        }
        public IEnumerable<ReporteEntregaPedido> ObtenerEntregasInstitucionales(string fechaEntrega, string fechaUso, string pedido, string asesor, string cliente, string producto)
        {
            return _respositorio.ObtenerEntregasInstitucionales(fechaEntrega, fechaUso, pedido, asesor, cliente, producto);
        }
    }
}
