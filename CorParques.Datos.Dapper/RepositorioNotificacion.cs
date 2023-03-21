using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioNotificacion : RepositorioBase<Notificacion>, IRepositorioNotificacion
    {
        public IEnumerable<Notificacion> ObtenerNotificacionesXPunto(int id)
        {
            return _cnn.Query<Notificacion>("SP_GetAllNotificationInPoint", param: new { idPunto = id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public IEnumerable<Notificacion> ObtenerNotificaciones()
        {
            return _cnn.Query<Notificacion>("SP_GetAllNotificationToday", null, commandType: System.Data.CommandType.StoredProcedure);
        }

        public IEnumerable<int> ObtenerPuntos(string grupos)
        {
           return _cnn.Query<int>("SP_GetAllPointsInGroup", param: new { idGroup = grupos }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
