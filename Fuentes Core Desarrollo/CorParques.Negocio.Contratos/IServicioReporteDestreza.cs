using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteDestreza
    {
        IEnumerable<ReporteDestrezas> ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string CodigoPunto = null, string NombreTipoBoleta = null, string NombreCliente = null, bool? tipoVenta=null);

        // RDSH: Se pusieron dos hojas para este reporte una para la información general y otra para poder sacar los premios y sus cantidades.
        IEnumerable<ReporteDestrezas>[] ObtenerReporteNuevo(string FechaInicial, string FechaFinal, string CodigoPunto = null, string CodigoSeries = null, string NombreTipoBoleta = null, string NombreCliente = null, bool? tipoVenta = null);

    }
}
