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
    public class ServicioReporteSoporteIT : IServicioReporteSoporteIT
    {
        private readonly IRepositorioReporteSoporteIT _repositorio;

        public ServicioReporteSoporteIT(IRepositorioReporteSoporteIT repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<SoporteIT> ObtenerReporte(string Fecha)
        {
            return _repositorio.ObtenerReporte(Fecha);
        }
    }
}
