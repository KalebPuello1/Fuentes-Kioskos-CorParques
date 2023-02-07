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
    public class ServicioReporteTarjetaRecargable : IServicioReporteTarjetaRecargable
    {
        private readonly IRepositorioReporteTarjetaRecargable _repositorio;

        public ServicioReporteTarjetaRecargable(IRepositorioReporteTarjetaRecargable repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ReporteTarjetaRecargable> ObtenerReporteTarjetas(string FechaInicial = null, string FechaFinal = null, string FechaCompra = null, string FechaVencimiento = null, string Cliente = null)
        {
            return _repositorio.ObtenerReporte(FechaInicial,FechaFinal,FechaCompra,FechaVencimiento,Cliente);
        }
        public IEnumerable<TipoGeneral> ObtenerClientes()
        {
            return _repositorio.ObtenerClientes();
        }
    }
}
