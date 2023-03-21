using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReporteAprovechamientoFA
    {
        IEnumerable<ReporteAprovechamientoFA> ObtenerReporte(string FechaInicial, string FechaFinal, string cliente, string pedido, string factura);
    }
}
