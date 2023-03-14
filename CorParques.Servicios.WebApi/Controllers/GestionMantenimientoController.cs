using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class GestionMantenimientoController : ApiController
    {
        private readonly IServicioGestionMantenimiento _servicio;

        public GestionMantenimientoController(IServicioGestionMantenimiento servicio)
        {
            _servicio = servicio;
        }
        
        [HttpGet]
        [Route("api/GestionMantenimiento/ObtenerxAtraccion/{id}")]
        public HttpResponseMessage ObtenerxAtraccion(int id)
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneral>>($"Mantenimiento({id})");
            if (list == null)
            {
                list = _servicio.ObtenerxAtraccion(id);
                Cache.SetCache($"Mantenimiento({id})", list, Cache.Long);
            }
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
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
