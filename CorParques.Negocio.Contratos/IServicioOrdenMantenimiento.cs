using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioOrdenMantenimiento
    {
        IEnumerable<OrdenMantenimiento> ObtenerOrdenesMantenimiento(string Punto, long NumeroOrden);
        bool ActualizaHoraOrden(string observaciones, int idUsuarioAprobador, long NumeroOrden, int Aprobado, int IdOperaciones, int Procesado, string CodSapPunto);
        IEnumerable<Retorno> ObtenerRetornoPorNumeroOrden(long NumeroOrden);

        //RDSH: Retorna una orden de mantenimiento por el numero de la orden.
        OrdenMantenimiento ObtenerOrdenMantenimiento(long intNumeroOrden);
    }
}
