using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class AlmacenController : ApiController
    {
        private readonly IServicioAlmacen servicio;
        public AlmacenController(IServicioAlmacen servici)
        {
            servicio = servici;
        }
        // GET: Almacen
        [HttpGet]
        [Route("api/Almacen/GetAll")]
        public HttpResponseMessage getAlmacen()
        {
           var dato = servicio.getAllAlmacen();
           return dato.Count() == 0 ? Request.CreateResponse(System.Net.HttpStatusCode.NotFound) :
                Request.CreateResponse(System.Net.HttpStatusCode.OK, dato);
        }
    }
}
