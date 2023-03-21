using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteVentasPorProducto
    {
        IEnumerable<ReporteVentasPorProducto> ObtenerReporte(string FechaInicial, string FechaFinal, string CodigoProducto, string CodigoPunto, string CentroBeneficio);
    }
}
