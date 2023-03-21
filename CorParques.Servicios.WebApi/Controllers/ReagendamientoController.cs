using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Corparques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ReagendamientoController : ApiController
    {
        IServicioReagendamiento _Reagendamiento;
        public ReagendamientoController(IServicioReagendamiento Reagendamiento)
        {
            _Reagendamiento = Reagendamiento;
        }
        [HttpGet]
        [Route("api/Reagendamiento/productoReagendar/{consecutivo}")]
        // GET: Reagendamiento
        public HttpResponseMessage productoReagendar(string consecutivo)
        {
            //Boleteria
            var dato = _Reagendamiento.ObtenerProducto(consecutivo);
            return dato.NombreProducto == "No existe" ? Request.CreateResponse(HttpStatusCode.NotFound) :
                        Request.CreateResponse(HttpStatusCode.OK, dato);
        }
        [HttpPost]
        [Route("api/Reagendamiento/ModificarFecha")]
        public HttpResponseMessage ModificarFecha(CambioFechaBoleta boleteria) 
        {
            var modificado = _Reagendamiento.ModificarFecha(boleteria);
            return modificado != "fallo" ? Request.CreateResponse(HttpStatusCode.OK, modificado) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }
        [HttpPost]
        [Route("api/Reagendamiento/InsertarDetalleReagendamientoFecha")]
        public HttpResponseMessage InsertarDetalleReagendamientoFecha(Reagendamiento boleteria)
        {
            var modificado = _Reagendamiento.InsertarDetalleReagendamientoFecha(boleteria);
            return modificado != "fallo" ? Request.CreateResponse(HttpStatusCode.OK, modificado) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }
        [HttpGet]
        [Route("api/Reagendamiento/ObtenerDetalleeReagendamientoFecha/{consecutivo}")]
        public HttpResponseMessage ObtenerDetalleReagendamientoFecha(string consecutivo)
        {
            var dato = _Reagendamiento.ObtenerDetalleReagendamientoFecha(consecutivo);
            return dato != null ? Request.CreateResponse(HttpStatusCode.OK, dato) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet, Route("api/Reagendamiento/ObtenerFacturaReagendamiento/{CodBarra}")]
        public HttpResponseMessage ObtenerFacturaReagendamiento(string CodBarra)
        {
            var _rta = _Reagendamiento.ObtenerFacturaReagendamiento(CodBarra);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }

    }
}