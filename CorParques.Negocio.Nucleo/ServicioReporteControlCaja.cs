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
    public class ServicioReporteControlCaja : IServicioReporteControlCaja
    {
        private readonly IRepositorioReporteControlCaja _respositorio;

        public ServicioReporteControlCaja(IRepositorioReporteControlCaja repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<ReporteControlCaja> ObtenerTodos(ReporteControlCaja modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }
    }
}
