using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioNotificacion
    {
        bool Enviar(Notificacion modelo);
        bool CambiarEstado(int id, int idUsuario);
        IEnumerable<Notificacion> Obtener(int id);
        IEnumerable<Notificacion> ObtenerNotificaciones();
    }
}
