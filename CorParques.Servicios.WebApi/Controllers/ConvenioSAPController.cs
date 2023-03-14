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
    public class ConvenioSAPController : ApiController
    {
        private readonly IServicioConvenioSAP _service;
        private readonly IServicioConvenio _serviceConvenio;

        public ConvenioSAPController(IServicioConvenioSAP service, IServicioConvenio serviceConvenio)
        {
            _service = service;
            _serviceConvenio = serviceConvenio;
        }

        [HttpGet]
        [Route("api/ConvenioSAP/ObtenerLista")]
        public HttpResponseMessage ObtenerLista()
        {
            var list = _service.ObtenerLista().Where(x => x.EsActivo.Equals(true) && x.FechaInicial <= DateTime.Now && x.FechaFinal >= DateTime.Now).ToList();
            var conveniosExclusion = _serviceConvenio.ObtenerExclusionesConvenio();

            var Returnlist = new List<Convenio>();


            foreach (var item in list)
            {
                bool blnValidacion = true;
                foreach (var item2 in conveniosExclusion.Where(x => x.IdEstado == 1 && x.IdConvenio == item.IdConvenio))
                {
                    if (item2.FechaInicio <= DateTime.Now && item2.FechaFin >= DateTime.Now)
                        blnValidacion = false;
                }

                if (blnValidacion)
                    Returnlist.Add(item);
            }

            return Returnlist == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, Returnlist);
        }

        [HttpGet]
        [Route("api/ConvenioSAP/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _service.ObtenerPorId(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ConvenioSAP/ObtenerDetalleConvenio/{IdConvenio}")]
        public HttpResponseMessage ObtenerDetalleConvenio(int IdConvenio)
        {
            var lista = _service.ObtenerDetalleConvenio(IdConvenio);
            return lista == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, lista);
        }
        [HttpPost]
        [Route("api/ConvenioSAP/ObtenerDetalleConvenioByApp")]
        public HttpResponseMessage ObtenerDetalleConvenioByApp(ProductosConvenio item)
        {
            var lista = _service.ObtenerDetalleConvenioByApp(item.Usuario, string.Join(",", item.CodigoSapProductos.Select(x => x.CodigoSapProductos)), item.OtroConvenio);
            //var lista = _service.ObtenerDetalleConvenioByApp(productosConvenio.Usuario, string.Join(",", productosConvenio.CodigoSapProductos.Select(x => x.CodigoSapProductos)), productosConvenio.OtroConvenio);

            var conveniosExclusion = _serviceConvenio.ObtenerExclusionesConvenio();

            if (conveniosExclusion.Any(x => x.IdEstado == 1 && x.IdConvenio == lista.FirstOrDefault()?.IdConvenio))
                lista = null;
            return lista == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, lista);
        }


        [HttpGet]
        [Route("api/ConvenioSAP/ValidarConvenioDaviplataFan/{Consecutivo}")]
        public HttpResponseMessage ValidarConvenioDaviplata(string Consecutivo)
        {

            Consecutivo = Consecutivo.Contains("@") ? Consecutivo.Replace("|", ".") : Consecutivo;
            var rta = _service.ValidarConvenioDaviplataFan(Consecutivo);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }
        [HttpGet]
        [Route("api/ConvenioSAP/ObtenerDetalleConvenioByConsec/{Consecutivo}")]
        public HttpResponseMessage ObtenerDetalleConvenioByConsec(string Consecutivo)
        {
            var lista = _service.ObtenerDetalleConvenioByConsec(Consecutivo);

            var conveniosExclusion = _serviceConvenio.ObtenerExclusionesConvenio();

            var Returnlist = new List<ConvenioDetalle>();


            foreach (var item in lista)
            {
                bool blnValidacion = true;
                foreach (var item2 in conveniosExclusion.Where(x => x.IdEstado == 1 && x.IdConvenio == item.IdConvenio))
                {
                    if (item2.FechaInicio <= DateTime.Now && item2.FechaFin >= DateTime.Now)
                        blnValidacion = false;
                }

                if (blnValidacion)
                    Returnlist.Add(item);
            }


            return Returnlist == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, Returnlist);
        }

        [HttpPost]
        [Route("api/ConvenioSAP/InsertarConvenioConsecutivos")]
        public HttpResponseMessage InsertarConvenioConsecutivos(List<ConvenioConsecutivos> lista)
        {
            var rta = _service.InsertarConvenioConsecutivos(lista);
            return Request.CreateResponse(HttpStatusCode.OK, rta);
        }

        //[HttpPost]
        //[Route("api/ConvenioDescuento/Insertar")]
        //public HttpResponseMessage Insertar(ConvenioSAP modelo)
        //{
        //    string strError;
        //    var item = _service.Insertar(modelo, out strError);
        //    return item ? Request.CreateResponse(HttpStatusCode.OK, "")
        //                    : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        //}

        //[HttpPut]
        //[Route("api/ConvenioDescuento/Actualizar")]
        //public HttpResponseMessage Actualizar(ConvenioDescuento modelo)
        //{
        //    string strError;
        //    var item = _service.Actualizar(modelo, out strError);
        //    return item ? Request.CreateResponse(HttpStatusCode.OK, "")
        //                       : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        //}

        //[HttpPut]
        //[Route("api/ConvenioDescuento/Eliminar")]
        //public HttpResponseMessage Eliminar(ConvenioDescuento modelo)
        //{
        //    string strError;
        //    var item = _service.Eliminar(modelo, out strError);
        //    return item ? Request.CreateResponse(HttpStatusCode.OK, "")
        //                       : Request.CreateResponse(HttpStatusCode.NotFound, strError);
        //}
    }
}