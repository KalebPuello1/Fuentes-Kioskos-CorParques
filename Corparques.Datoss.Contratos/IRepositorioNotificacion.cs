using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioNotificacion : IRepositorioBase<Notificacion>
    {
        IEnumerable<int> ObtenerPuntos(string grupos);
        IEnumerable<Notificacion> ObtenerNotificacionesXPunto(int id);
        IEnumerable<Notificacion> ObtenerNotificaciones();

    }
}
