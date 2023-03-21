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
    public class ArqueoController : ApiController
    {
        private readonly IServicioArqueo _service;


        public ArqueoController(IServicioArqueo service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("api/Arqueo/ObtenerArqueo")]
        public HttpResponseMessage ObtenerArqueo(int idUsuario, int IdPunto)
        {
            var list = _service.ObtenerArqueo(idUsuario, IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpPost]
        [Route("api/Arqueo/Insert")]
        public HttpResponseMessage Insert(List<NovedadArqueo> modelo)
        {
            var list = _service.ActualizarNovedadArqueo(modelo);
            return list == false ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, "");
        }



    }
}