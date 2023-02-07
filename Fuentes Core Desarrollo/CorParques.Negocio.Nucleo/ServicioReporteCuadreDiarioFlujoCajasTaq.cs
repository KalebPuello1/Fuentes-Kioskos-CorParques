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
    public class ServicioReporteCuadreDiarioFlujoCajasTaq : IServicioReporteCuadreDiarioFlujoCajasTaq
    {
        private readonly IRepositorioReporteCuadreDiarioFlujoCajasTaq _respositorio;

        public ServicioReporteCuadreDiarioFlujoCajasTaq(IRepositorioReporteCuadreDiarioFlujoCajasTaq repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<ReporteCuadreDiarioFlujoCajasTaq> ObtenerTodos(ReporteCuadreDiarioFlujoCajasTaq modelo)
        {
            return _respositorio.ObtenerTodos(modelo);
        }
    }
}
