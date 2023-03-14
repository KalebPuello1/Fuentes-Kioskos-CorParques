using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class NotificacionController : ApiController
    {
        private readonly IServicioNotificacion _service;

        public NotificacionController(IServicioNotificacion service)
        {
            _service = service;
        }

        
        [HttpPost]
        [Route("api/Notificacion/Set")]
        public HttpResponseMessage Send(Notificacion modelo)
        {
            var rta = _service.Enviar(modelo);
            var listadoNotificaciones = _service.ObtenerNotificaciones().ToList();
            foreach (var item in listadoNotificaciones)
            {
                item.ListaGrupos = obtenerMensajeTiempo(item.FechaCreacion);
            }

            SignalR.StartHub.SR2_UpdateAllClientNotification(listadoNotificaciones.AsEnumerable().OrderByDescending(M => M.FechaCreacion));
            return !rta ? Request.CreateResponse(HttpStatusCode.NotFound, "Se presento un problema enviando las notificaciones")
                            : Request.CreateResponse(HttpStatusCode.OK, "Notificación enviada exitosamente.");
        }
        [HttpGet]
        [Route("api/Notificacion/SetView/{id}/{idUsuario}")]
        public HttpResponseMessage SetView(int id,int idUsuario)
        {
            _service.CambiarEstado(id, idUsuario);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        [Route("api/Notificacion/Get/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var rta = _service.Obtener(id);
            return rta != null ? Request.CreateResponse(HttpStatusCode.OK, rta):
                                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("api/Notificacion/GetAllToday")]
        public HttpResponseMessage GetAllToday()
        {
            var rta = _service.ObtenerNotificaciones();
            return rta != null ? Request.CreateResponse(HttpStatusCode.OK, rta) :
                                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        private string obtenerMensajeTiempo(DateTime item)
        {
            TimeSpan span = DateTime.Now.Subtract(item);
            var mensaje = "";
            if (span.Seconds < 60 && span.Minutes < 1 && span.Hours < 1)
                mensaje = "hace segundos";
            else if (span.Minutes < 60 && span.Hours < 1)
                mensaje = $"hace {span.Minutes} minutos";
            else
                mensaje = $"hace {span.Hours} hora" + (span.Hours > 1 ? "s" : "");
            return mensaje;
        }
    }
}
