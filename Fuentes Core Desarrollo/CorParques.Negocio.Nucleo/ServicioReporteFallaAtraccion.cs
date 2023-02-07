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
    public class ServicioReporteFallaAtraccion : IServicioReporteFallaAtraccion
    {
        private readonly IRepositorioReporteFallaAtraccion _respositorio;

        public ServicioReporteFallaAtraccion(IRepositorioReporteFallaAtraccion repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<ReporteFallaAtraccion> ObtenerTodos(ReporteFallaAtraccion modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }
    }
}
