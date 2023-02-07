using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using System.Windows.Forms;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteRecoleccion : IServicioReporteRecoleccion
    {
        private readonly IRepositorioReporteRecoleccion _repositorio;

        public ServicioReporteRecoleccion(IRepositorioReporteRecoleccion repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteRecoleccion> ObtenerReporte(string Fecha, int? IdTaquillero, int? IdSupervisor)
        {
            return _repositorio.ObtenerReporte(Fecha , IdTaquillero, IdSupervisor);
        }
    }
}
