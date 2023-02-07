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
    public class GestionMantenimientoControlController : ApiController
    {
        private readonly IServicioGestionMantenimientoControl _servicio;

        public GestionMantenimientoControlController(IServicioGestionMantenimientoControl servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Route("api/GestionMantenimientoControl/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _servicio.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/GestionMantenimientoControl/ActualizarMantenbimientoControl")]
        public HttpResponseMessage ActualizarMantenbimientoControl(GestionMantenimientoControl modelo)
        {
            var item = _servicio.ActualizarMantenbimientoControl(modelo);
            return item != false ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [Route("api/GestionMantenimientoControl/Obtener/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _servicio.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        //[HttpDelete]
        //[Route("api/Attractions/Delete/{id}")]
        //public HttpResponseMessage Eliminar(int id)
        //{
        //    var item = _servicio.Eliminar(id);
        //    return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
        //                    : Request.CreateResponse(HttpStatusCode.OK, item);
        //}
    }
}
