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
    public class ServicioReporteFANVendidas : IServicioReporteFANVendidas
    {
        private readonly IRepositorioReporteFANVendidas _repositorio;

        public ServicioReporteFANVendidas(IRepositorioReporteFANVendidas repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteFANVendidas> ObtenerReporte(string FechaInicial)
        {            
            return _repositorio.ObtenerReporte(FechaInicial);
        }
    }
}
