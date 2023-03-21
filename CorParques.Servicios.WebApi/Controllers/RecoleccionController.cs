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
    public class RecoleccionController : ApiController
    {

        private readonly IServicioRecoleccion _service;

        public RecoleccionController(IServicioRecoleccion service)
        {
            _service = service;
        }

  
        [HttpGet]
        [Route("api/Recoleccion/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerRecoleccionActiva/{IdUsuario}/{IdPunto}/{Cierre}/{IdEstado}")]
        public HttpResponseMessage ObtenerRecoleccionActiva(int IdUsuario, int IdPunto, int Cierre, int IdEstado)
        {
            var item = _service.ObtenerRecoleccionActiva(IdUsuario, IdPunto, (Cierre == 1), IdEstado);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPost]
        [Route("api/Recoleccion/Insertar")]
        public HttpResponseMessage Insertar(Recoleccion modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPost]
        [Route("api/Recoleccion/InsertarGeneral")]
        public HttpResponseMessage InsertarGeneral(Recoleccion modelo)
        {
            string strError;
            var item = _service.InsertaGeneral(modelo, out strError);
            return item != 0 ? Request.CreateResponse(HttpStatusCode.OK,  item)
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }


        [HttpPost]
        [Route("api/Recoleccion/Actualizar")]
        public HttpResponseMessage Actualizar(Recoleccion modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpPut]
        [Route("api/Recoleccion/Eliminar")]
        public HttpResponseMessage Eliminar(Recoleccion modelo)
        {
            string strError;
            var item = _service.Eliminar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerDocumentosRecoleccion/{IdUsuario}")]
        public HttpResponseMessage ObtenerDocumentosRecoleccion(int IdUsuario)
        {
            var item = _service.ObtenerDocumentosRecoleccion(IdUsuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerTopesRecoleccion/{IdUsuario}/{IdPunto}")]
        public HttpResponseMessage ObtenerTopesRecoleccion(int IdUsuario, int IdPunto)
        {
            var item = _service.ObtenerTopesRecoleccion(IdUsuario, IdPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerTopesCierreTaquilla/{IdUsuario}/{IdPunto}")]
        public HttpResponseMessage ObtenerTopesCierreTaquilla(int IdUsuario, int IdPunto)
        {
            var item = _service.ObtenerTopesCierreTaquilla(IdUsuario, IdPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerBrazaletesRestantes/{IdUsuario}/{IdPunto}")]
        public HttpResponseMessage ObtenerBrazaletesRestantes(int IdUsuario, int IdPunto)
        {
            var item = _service.ObtenerBrazaletesRestantes(IdUsuario, IdPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerAlertas")]
        public HttpResponseMessage ObtenerAlertas()
        {
            var item = _service.ObtenerNotificaciones();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerPuntosRecoleccion/{IdEstado}/{Cierre}")]
        public HttpResponseMessage ObtenerPuntosRecoleccion(int IdEstado, int Cierre)
        {
            var item = _service.ObtenerPuntosRecoleccion(IdEstado, (Cierre == 1));
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Recoleccion/ObtenerTaquillerosConRecoleccion/{IdEstado}/{Cierre}")]
        public HttpResponseMessage ObtenerTaquillerosConRecoleccion(int IdEstado, int Cierre)
        {
            var item = _service.ObtenerTaquillerosConRecoleccion(IdEstado, (Cierre == 1));
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        
        [HttpGet]
        [Route("api/Recoleccion/ObtenerNovedadesRecoleccion/{IdUsuario}")]
        public HttpResponseMessage ObtenerNovedadesRecoleccion(int IdUsuario)
        {
            var item = _service.ObtenerNovedadesRecoleccion(IdUsuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Recoleccion/ObtenerRecoleccionesActivas/{IdUsuario}/{IdPunto}/{Cierre}/{IdEstado}")]
        public HttpResponseMessage ObtenerRecoleccionesActivas(int IdUsuario, int IdPunto, int Cierre, int IdEstado)
        {
            var item = _service.ObtenerRecoleccionesActivas(IdUsuario, IdPunto, (Cierre == 1), IdEstado);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

    }
}
