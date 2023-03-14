using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioOperacion
    {
        IEnumerable<Operaciones> ObtenerOrdenes(string Punto, string NumDocumento);
        Operaciones ObtenerOrdenPorNumeroOrden(long NumeroOrden);

        //RDSH: Retorna las operaciones por numero de orden.
        IEnumerable<Operaciones> ObtenerOperacionesPorOrden(long intNumeroOrden);

    }
}
