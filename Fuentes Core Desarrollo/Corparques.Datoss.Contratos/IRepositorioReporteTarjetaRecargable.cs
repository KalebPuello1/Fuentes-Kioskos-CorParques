using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteTarjetaRecargable
    {

        IEnumerable<ReporteTarjetaRecargable> ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string FechaCompra = null, string FechaVencimiento = null, string Cliente = null);
        IEnumerable<TipoGeneral> ObtenerClientes();



    }
}
