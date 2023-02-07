using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteVentasDirectas
    {
        IEnumerable<ReporteVentas>[] ObtenerReporte(string FechaInicial , string FechaFinal, int? IdPunto, int? IdTaquillero , int? IdFormaPago , int? IdFranquicia, string CentroBeneficio);
    }
}
