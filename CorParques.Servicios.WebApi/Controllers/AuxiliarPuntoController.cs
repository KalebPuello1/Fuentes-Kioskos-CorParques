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
    public class AuxiliarPuntoController : ApiController
    {

        private readonly IServicioAuxiliarPunto _service;

        public AuxiliarPuntoController(IServicioAuxiliarPunto service)
        {
            _service = service;
        }

        /// <summary>
        /// RDHS: Retorna la lista de auxiliares sin fecha de modificacion asociados al punto enviado.
        /// </summary>
        /// <param name="IdPunto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/AuxiliarPunto/ObtenerListaAuxiliarPunto/{IdPunto}")]
        public HttpResponseMessage ObtenerListaAuxiliarPunto(int IdPunto)
        {
            var list = _service.ObtenerListaAuxiliarPunto(IdPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        /// <summary>
        /// RDSH: Consulta la informacion de un empleado para agregarlo a los auxiliares de la atraccion.
        /// </summary>
        /// <param name="IdPunto"></param>
        /// <param name="Documento"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/AuxiliarPunto/ObtenerInformacionAuxiliar/{IdPunto}/{Documento}")]
        public HttpResponseMessage ObtenerInformacionAuxiliar(int IdPunto, string Documento)
        {
            var item = _service.ObtenerInformacionAuxiliar(IdPunto,Documento);
            if (item == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }            
        }

        /// <summary>
        /// RDSH: Asocia un auxiliar a una ubicacion dentro de la atraccion (punto).
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/AuxiliarPunto/Insertar")]
        public HttpResponseMessage Insertar(AuxiliarPunto modelo)
        {
            string strError;
            var item = _service.Insertar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }

        /// <summary>
        /// RDSH: Actualiza la fecha de modificacion del auxiliar punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/AuxiliarPunto/Actualizar")]
        public HttpResponseMessage Actualizar(AuxiliarPunto modelo)
        {
            string strError;
            var item = _service.Actualizar(modelo, out strError);
            return item ? Request.CreateResponse(HttpStatusCode.OK, "")
                               : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        }       

    }
}
