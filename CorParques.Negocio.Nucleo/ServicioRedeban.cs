using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Contratos;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace CorParques.Negocio.Nucleo
{
    public class ServicioRedeban : IServicioRedeban
    {

        IRepositorioLogRedebanSolicitud _repositorioSolicitud;

        IRepositorioLogRedebanRespuesta _repositorioRespuesta;

        IRepositorioLogRedebanSolicitudAnulacion _repositorioSolicitudAnulacion;


        public ServicioRedeban(IRepositorioLogRedebanSolicitud repositorioSolciitud, IRepositorioLogRedebanRespuesta repositorioRespuesta, IRepositorioLogRedebanSolicitudAnulacion repositorioSolicitudAnulacion )
        {
            _repositorioSolicitud = repositorioSolciitud;
            _repositorioRespuesta = repositorioRespuesta;
            _repositorioSolicitudAnulacion = repositorioSolicitudAnulacion;
        }

        public IEnumerable<LogRedebanSolicitud> ObtenerTodosLogRedebanSolicitud()
        {
            return _repositorioSolicitud.ObtenerLista();
        }
        public bool ActualizarLogRedebanSolicitud(ref LogRedebanSolicitud logRedebanSolicitud)
        {
            return _repositorioSolicitud.Actualizar(ref logRedebanSolicitud);
        }
        public bool EliminarLogRedebanSolicitud(LogRedebanSolicitud logRedebanSolicitud)
        {
            return _repositorioSolicitud.Eliminar(logRedebanSolicitud);
        }
        public int InsertarLogRedebanSolicitud(ref LogRedebanSolicitud logRedebanSolicitud)
        {
            return _repositorioSolicitud.Insertar(ref logRedebanSolicitud);
        }

        public LogRedebanSolicitud ObtenerLogRedebanSolicitud(int idLogRedebanSolicitud)
        {
            var logRedebanSolicitud = _repositorioSolicitud.Obtener(idLogRedebanSolicitud);
            return logRedebanSolicitud;
        }

        public IEnumerable<LogRedebanRespuesta> ObtenerTodosLogRedebanRespuesta()
        {
            return _repositorioRespuesta.ObtenerLista();
        }
        public int ActualizarLogRedebanRespuesta(ref LogRedebanRespuesta logRedebanRespuesta)
        {
            return _repositorioRespuesta.ActualizarLogRedebanRespuesta( logRedebanRespuesta);
        }
        public bool EliminarLogRedebanRespuesta(LogRedebanRespuesta logRedebanRespuesta)
        {
            return _repositorioRespuesta.Eliminar(logRedebanRespuesta);
        }
        public int InsertarLogRedebanRespuesta(ref LogRedebanRespuesta logRedebanRespuesta)
        {
            return _repositorioRespuesta.Insertar(ref logRedebanRespuesta);
        }
        public LogRedebanRespuesta ObtenerLogRedebanRespuesta(int idLogRedebanRespuesta)
        {
            var logRedebanRespuesta = _repositorioRespuesta.Obtener(idLogRedebanRespuesta);
            return logRedebanRespuesta;
        }

        public IEnumerable<LogRedebanSolicitudAnulacion> ObtenerTodosLogRedebanSolicitudAnulacion()
        {
            return _repositorioSolicitudAnulacion.ObtenerLista();
        }
        public bool ActualizarLogRedebanSolicitudAnulacion(ref LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion)
        {
            return _repositorioSolicitudAnulacion.Actualizar(ref logRedebanSolicitudAnulacion);
        }
        public bool EliminarLogRedebanSolicitudAnulacion(LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion)
        {
            return _repositorioSolicitudAnulacion.Eliminar(logRedebanSolicitudAnulacion);
        }
        public int InsertarLogRedebanSolicitudAnulacion(ref LogRedebanSolicitudAnulacion logRedebanSolicitudAnulacion)
        {
            return _repositorioSolicitudAnulacion.Insertar(ref logRedebanSolicitudAnulacion);
        }

        public LogRedebanSolicitudAnulacion ObtenerLogRedebanSolicitudAnulacion(int idLogRedebanSolicitudAnulacion)
        {
            var logRedebanSolicitudAnulacion = _repositorioSolicitudAnulacion.Obtener(idLogRedebanSolicitudAnulacion);
            return logRedebanSolicitudAnulacion;
        }
    }
}
