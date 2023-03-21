using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioTipoBrazalete
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();
        bool ActualizarBrazalete(TipoBrazalete modelo);
        IEnumerable<Puntos> ObtenerAtraccionxBrazalete(int idTipoBrazalete);
        IEnumerable<TipoBrazalete> ObtenerTodosBrazalete();
        IEnumerable<TipoBrazalete> ObtenerBrazaletesSupervisor( int supervisor);
        bool BorrarTipoBrazalete(int idTipoBrazalete);
        TipoBrazalete ObtererBrazalete(int idTipoBrazalete);
        IEnumerable<TipoBrazalete> ObtenerTodosBrazaleteInventario(int IdPunto);
    }
}
