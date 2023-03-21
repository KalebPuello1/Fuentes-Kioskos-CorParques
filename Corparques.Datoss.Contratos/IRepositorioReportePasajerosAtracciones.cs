using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReportePasajerosAtracciones
    {
        IEnumerable<ReportePasajeros> ObtenerReporte(string FechaInicial, string FechaFinal, int? IdPunto, int? IdTipoProducto);
    }
}
