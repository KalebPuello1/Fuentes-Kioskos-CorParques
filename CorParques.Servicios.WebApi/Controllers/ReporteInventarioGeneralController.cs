using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReporteInventarioGeneralController : ApiController
    {
        private readonly IServicioReporteInventarioGeneral _service;
        private readonly IServicioMateriales _mat;

        public ReporteInventarioGeneralController(IServicioReporteInventarioGeneral service, IServicioMateriales mat)
        {
            _service = service;
            _mat = mat;
        }

        [HttpGet]
        [Route("api/ReporteInventarioGeneral/set/{fechaInicial}/{fechaFinal}/{idAtraccion}/{idMaterial}/{CB}")]
        public HttpResponseMessage ReporteInventarioGeneral(string fechaInicial, string fechaFinal, string idAtraccion, int idMaterial, int CB)
        {
            ReporteInventarioGeneral modelo = new ReporteInventarioGeneral();
            modelo.fechaInicial = fechaInicial == "null" ? null : fechaInicial;
            modelo.fechaFinal = fechaFinal == "null" ? null : fechaFinal;
            modelo.Almacen = idAtraccion;
            modelo.idMaterial = idMaterial;
            modelo.CB = CB;

            var item = _service.ObtenerTodos(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ReporteInventarioGeneral/Boleteria/{fecha}/{usuario}")]
        public HttpResponseMessage Boleteria(string fecha, string usuario)
        {

            usuario = usuario == "null" ? null : usuario;
            var item = _service.Obtenerboleteria(fecha,usuario);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/ReporteInventarioGeneral/ObtenerEntregasInstitucionales/{fechaEntrega}/{fechaUso}/{pedido}/{asesor}/{cliente}/{producto}")]
        public HttpResponseMessage ObtenerEntregasInstitucionales(string fechaEntrega, string fechaUso, string pedido, string asesor, string cliente, string producto)
        {
            fechaEntrega = fechaEntrega == "null" ? null : fechaEntrega;
            fechaUso = fechaUso == "null" ? null : fechaUso;
            pedido = pedido == "null" ? null : pedido;
            asesor = asesor == "null" ? null : asesor;
            cliente = cliente == "null" ? null : cliente;
            producto = producto == "null" ? null : producto;
            var item = _service.ObtenerEntregasInstitucionales(fechaEntrega, fechaUso, pedido, asesor, cliente, producto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/ReporteInventarioGeneral/ObtenerMaterialesTodos")]
        public HttpResponseMessage ObtenerMaterialesTodos()
        {
            var list = _mat.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}