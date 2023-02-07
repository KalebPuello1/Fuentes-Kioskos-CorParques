using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioTipoBrazalete
    {
        bool ActualizarBrazalete(TipoBrazalete Modelo);
        IEnumerable<Puntos> ObtenerAtraccionxBrazalete(int idTipoBrazalete);
        IEnumerable<TipoBrazalete> ObtenerTodosBrazalete();
        IEnumerable<TipoBrazalete> ObtenerBrazaletesSupervisor(int supervisor);
        bool BorrarTipoBrazalete(int idTipoBrazalete);
        IEnumerable<TipoGeneral> ObtenerListaSimple();
        IEnumerable<TipoGeneral> ObtenerListaSimpleAtraccion();
        IEnumerable<TipoGeneral> ObtenerListaSimpleEstado();
        TipoBrazalete Obtener(int id);
        bool Desactivar(int id);
        IEnumerable<TipoBrazalete> ObtenerTodosBrazaleteInventario(int IdPunto);
    }
}
