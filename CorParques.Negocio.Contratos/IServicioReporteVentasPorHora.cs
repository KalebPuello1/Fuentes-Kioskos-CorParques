using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteVentasPorHora
    {
        IEnumerable<ReporteVentasPorHora> ObtenerReporte(string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodigoProducto, string NombreProducto, string CodigoPunto, string CB);
    }
}
