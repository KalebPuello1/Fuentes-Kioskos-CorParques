using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteTarjetaRecargable
    {
        IEnumerable<ReporteTarjetaRecargable> ObtenerReporteTarjetas(string FechaInicial = null, string FechaFinal = null, string FechaCompra = null, string FechaVencimiento = null, string Cliente = null);
        IEnumerable<TipoGeneral> ObtenerClientes();
    }
}
