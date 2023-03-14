using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReimpresion
    {
        IEnumerable<Reimpresion> ObtenerReimpresion(string Punto, string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodBrazalete, string CodInicialfactura, string CodFinalFactura);
        IEnumerable<Reimpresion> GetReimpresion(ReimpresionFiltros modelo);
    }
}
