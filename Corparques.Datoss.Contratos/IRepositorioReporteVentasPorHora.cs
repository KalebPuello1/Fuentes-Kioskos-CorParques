using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteVentasPorHora
    {
        IEnumerable<ReporteVentasPorHora> ObtenerReporte(string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodigoProducto, string NombreProducto, string CodigoPunto, string CB);
    }
}
