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
    public class ServicioReporteRedFechaAbierta : IServicioReporteRedFechaAbierta
    {
        private readonly IRepositorioReporteRedFechaAbierta _respositorio;

        public ServicioReporteRedFechaAbierta(IRepositorioReporteRedFechaAbierta repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<TipoGeneral> obtenerTiposProducto()
        {
            return _respositorio.obtenerTiposProducto();
        }

         public IEnumerable<ReporteRedFechaAbierta> ObtenerTodos(ReporteRedFechaAbierta modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }

        public IEnumerable<TipoGeneral> obtenerTodosVendedores()
        {
            return _respositorio.obtenerTodosVendedores();
        }
    }
}
