using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioReimpresion
    {
        IEnumerable<Reimpresion> ObtenerReimpresion(string Punto, string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodBrazalete, string CodInicialFactura, string CodFinalFactura);
        IEnumerable<Reimpresion> GetReimpresion(ReimpresionFiltros modelo);
    }
}
