using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioRedeban
    {
        IEnumerable<LogRedebanSolicitud> ObtenerTodosLogRedebanSolicitud();
        int InsertarLogRedebanSolicitud(ref LogRedebanSolicitud logRedebanSolicitud);
        bool EliminarLogRedebanSolicitud(LogRedebanSolicitud logRedebanSolicitud);
        bool ActualizarLogRedebanSolicitud(ref LogRedebanSolicitud logRedebanSolicitud);
        LogRedebanSolicitud ObtenerLogRedebanSolicitud(int idLogRedebanSolicitud);
        IEnumerable<LogRedebanRespuesta> ObtenerTodosLogRedebanRespuesta();
        int InsertarLogRedebanRespuesta(ref LogRedebanRespuesta logRedebanRespuesta);

        bool EliminarLogRedebanRespuesta(LogRedebanRespuesta logRedebanRespuesta);
        int ActualizarLogRedebanRespuesta(ref LogRedebanRespuesta logRedebanRespuesta);
        LogRedebanRespuesta ObtenerLogRedebanRespuesta(int idLogRedebanRespuesta);
        IEnumerable<LogRedebanSolicitudAnulacion> ObtenerTodosLogRedebanSolicitudAnulacion();
        int InsertarLogRedebanSolicitudAnulacion(ref LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion);
        bool EliminarLogRedebanSolicitudAnulacion(LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion);
        bool ActualizarLogRedebanSolicitudAnulacion(ref LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion);
        LogRedebanSolicitudAnulacion ObtenerLogRedebanSolicitudAnulacion(int idLogRedebanSolicitudAnulacion);

    }
}
