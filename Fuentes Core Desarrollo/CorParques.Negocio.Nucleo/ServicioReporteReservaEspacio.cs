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
    public class ServicioReporteReservaEspacio : IServicioReporteReservaEspacio
    {
        private readonly IRepositorioReporteReservaEspacio _respositorio;

        public ServicioReporteReservaEspacio(IRepositorioReporteReservaEspacio repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<ReporteReservaEspacio> ObtenerTodos(ReporteReservaEspacio modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }
    }
}
