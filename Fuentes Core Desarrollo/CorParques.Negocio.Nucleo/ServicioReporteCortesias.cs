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
    public class ServicioReporteCortesias : IServicioReporteCortesias
    {
        private readonly IRepositorioReporteCortesias _repositorio;

        public ServicioReporteCortesias(IRepositorioReporteCortesias repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteCortesias> ObtenerReporte(string FechaInicial, string FechaFinal, string Documento, string TarjetaFan)
        {            
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal, Documento, TarjetaFan);            
        }
    }
}
