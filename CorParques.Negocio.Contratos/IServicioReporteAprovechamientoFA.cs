using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporteAprovechamientoFA
    {
        IEnumerable<ReporteAprovechamientoFA> ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string cliente = null, string pedido = null, string factura = null);

        
    }
}
