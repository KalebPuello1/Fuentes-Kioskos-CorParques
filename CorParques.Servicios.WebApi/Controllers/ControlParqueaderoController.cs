using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ControlParqueaderoController : ApiController
    {
        private readonly IServicioControlParqueadero _service;

        public ControlParqueaderoController(IServicioControlParqueadero service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/ControlParqueadero/Insert")]
        public HttpResponseMessage Crear(ControlParqueadero modelo)
        {
            HttpResponseMessage returnObject = null;

            string sMensaje = "";
            var item = _service.Insertar(modelo, out sMensaje);
            if (item != null)
            {
                //returnObject = Request.CreateResponse(HttpStatusCode.OK, "Salida registrada con exito tiquete <b>"+ item.Id.ToString() + "</b>");
                returnObject = Request.CreateResponse(HttpStatusCode.OK, item.Id.ToString());
            }
            else
            {
                returnObject = Request.CreateResponse(HttpStatusCode.InternalServerError, sMensaje);
            }

            return returnObject;
        }


        [HttpPost]
        [Route("api/ControlParqueadero/RegistrarSalida")]
        public HttpResponseMessage RegistrarSalida(ControlParqueadero modelo)
        {
            string sMensaje = "";
            var item = _service.RegistrarSalida(modelo, out sMensaje);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound, sMensaje)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }



        [HttpPost]
        [Route("api/ControlParqueadero/CalcularPago")]
        public HttpResponseMessage CalcularPago(ControlParqueadero ingreso)
        {
            
            string mensaje = "";
            var item = _service.CalcularPago(ingreso, out mensaje);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound, mensaje)
                            : Request.CreateResponse(HttpStatusCode.OK, new JavaScriptSerializer().Serialize(item));
        }

        [HttpGet]
        [Route("api/ControlParqueadero/ObtenerDisponibilidad")]
        public HttpResponseMessage ObtenerDisponibilidad()
        {
            var item = _service.ObtenerDisponibilidad();
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/ControlParqueadero/GetPlaca/{id}")]
        public HttpResponseMessage ObtenerPlaca(int id)
        {
            var item = _service.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item.IdEstado == 3 ? "MSJ:El vehiculo registrado con esta placa ya salio del parque" : (item.IdEstado == 2 ? "Msj: Este parqueadero ya ha sido pagado" : item.Placa));
        }


        //[HttpGet]
        //[Route("api/ControlParqueadero/GetIngresoByPlaca/{Placa}")]
        //public HttpResponseMessage GetIngresoByPlaca(string Placa)
        //{
        //    var item = _service.ObtenerPorPlaca(Placa);
        //    return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
        //                    : Request.CreateResponse(HttpStatusCode.OK, item);
        //}

        //[HttpPut]
        //[Route("api/ControlParqueadero/Update")]
        //public HttpResponseMessage ActualizarSalida(ControlParqueadero ingreso)
        //{
        //    var item = _service.Actualizar(ingreso);
        //    return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
        //                    : Request.CreateResponse(HttpStatusCode.OK, item);
        //}

    }
}