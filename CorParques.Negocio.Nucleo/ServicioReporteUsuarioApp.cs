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
    public class ServicioReporteUsuarioApp : IServicioReporteUsuarioApp
    {
        private readonly IRepositorioReporteUsuarioApp _repositorio;

        public ServicioReporteUsuarioApp(IRepositorioReporteUsuarioApp repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteUsuarioApp>[] ObtenerReporte(string Correoelectronico)
        {
            return _repositorio.ObtenerReporte(Correoelectronico);
        }
    }
}
