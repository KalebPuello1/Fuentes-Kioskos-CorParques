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
    public class ServicioReporteAprovechamientoFA : IServicioReporteAprovechamientoFA
    {
        private readonly IRepositorioReporteAprovechamientoFA _repositorio;

        public ServicioReporteAprovechamientoFA(IRepositorioReporteAprovechamientoFA repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteAprovechamientoFA> ObtenerReporte(string FechaInicial, string FechaFinal, string cliente,  string pedido, string factura)
        {
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, cliente, pedido, factura);
        }
    }
}
