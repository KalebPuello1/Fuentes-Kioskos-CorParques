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
    public class ServicioReporteRecaudosVentas : IServicioReporteRecaudosVentas
    {
        private readonly IRepositorioReporteRecaudosVentas _respositorio;

        public ServicioReporteRecaudosVentas(IRepositorioReporteRecaudosVentas repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<ReporteRecaudosVentas> ObtenerTodos(ReporteRecaudosVentas modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }
    }
}
