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
    public class AperturaController : ApiController
    {
        private readonly IServicioApertura _service;

        private readonly IServicioAperturaBase _serviceAperturaBase;
        private readonly IServicioTipoDenominacion _Denominacion;
        private readonly IServicioPuntos _servicePuntos;

        public AperturaController(IServicioApertura service, IServicioAperturaBase serviceBase
                    , IServicioTipoDenominacion serviceDenomicacion, IServicioPuntos servicePuntos)
        {
            _service = service;
            _serviceAperturaBase = serviceBase;
            _Denominacion = serviceDenomicacion;
            _servicePuntos = servicePuntos;
        }


        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosSinApertura")]
        public HttpResponseMessage ObtenerPuntosSinApertura()
        {
            var list = _service.ObtenerPuntosSinApertura();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        //
        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosSurtido")]
        public HttpResponseMessage ObtenerPuntosSurtido()
        {
            var list = _service.ObtenerPuntosSurtido();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        
        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosParaAperturaElementos")]
        public HttpResponseMessage ObtenerPuntosParaAperturaElementos()
        {
            var list = _service.ObtenerPuntosParaAperturaElementos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosSinAperturaFecha/{Fecha}")]
        public HttpResponseMessage ObtenerPuntosSinAperturaFecha(string Fecha)
        {
            var list = _service.ObtenerPuntosSinApertura(DateTime.Parse(Fecha));
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosConApertura")]
        public HttpResponseMessage ObtenerPuntosConApertura()
        {
            var list = _service.ObtenerPuntosConApertura();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosConAperturaFecha/{Fecha}")]
        public HttpResponseMessage ObtenerPuntosConAperturaFecha(string Fecha)
        {
            var list = _service.ObtenerPuntosConApertura(DateTime.Parse(Fecha));
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosAperturaEnProceso")]
        public HttpResponseMessage ObtenerPuntosAperturaEnProceso()
        {
            var list = _service.ObtenerPuntosAperturaEnProceso();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerPuntosConElementosReabastecimiento")]
        public HttpResponseMessage ObtenerPuntosConElementosReabastecimiento()
        {
            var list = _service.ObtenerPuntosConElementosReabastecimiento();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerTipoElementos")]
        public HttpResponseMessage ObtenerTipoElementos()
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneral>>("TiposElementos");
            if (list == null)
            {
                list = _service.ObtenerTipoElementos();
                Cache.SetCache<IEnumerable<TipoGeneral>>("TiposElementos", list, Cache.Long);
            }
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Apertura/Obtener/{Id}")]
        public HttpResponseMessage Obtener(int Id)
        {
            var item = _service.Obtener(Id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpPost]
        [Route("api/Apertura/InsertElementos")]
        public HttpResponseMessage InsertElementos(AperturaElementosInsert modelo)
        {
            var item = _service.InsertElementos(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, string.Empty)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/Apertura/EliminarElementoPorIdAperturaElemento")]
        public HttpResponseMessage EliminarElementoPorIdAperturaElemento(AperturaElementos modelo)
        {
            var item = _service.EliminarElementoPorIdAperturaElemento(modelo);
            return item ? Request.CreateResponse(HttpStatusCode.OK, string.Empty)
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [Route("api/AperturaBase/ObtenerAperturaBase/{IdPunto}")]
        public HttpResponseMessage ObtenerAperturaBase(int IdPunto)
        {
            var item = _serviceAperturaBase.ObtenerAperturaBase(IdPunto);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/AperturaBase/ObtenerAperturaBaseFecha/{IdPunto}/{Fecha}")]
        public HttpResponseMessage ObtenerAperturaBaseFecha(int IdPunto,string Fecha)
        {
            var item = _serviceAperturaBase.ObtenerAperturaBase(IdPunto, DateTime.Parse(Fecha));
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerListaAperturaBrazalete/{IdSupervisor}")]
        public HttpResponseMessage ObtenerListaAperturaBase(int IdSupervisor)
        {
            var item = _service.ObtenerListaAperturaBrazalete(IdSupervisor);
            return item == null || item.Count == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Apertura/DetalleInventario/{puntos}")]
        public HttpResponseMessage ObtenerDetalleInventario(string puntos)
        {
            var item = _serviceAperturaBase.ObtenerDetalleInventario(puntos);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);

        }

        [HttpGet]
        [Route("api/Apertura/ObtenerAperturaBrazalete/{id}")]
        public HttpResponseMessage ObtenerAperturaBrazalete(int id)
        {
            var item = _service.ObtenerAperturaBrazalete(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerAperturaBrazalete")]
        public HttpResponseMessage ObtenerAperturaBrazaleteIdSupervidor(int idSupervidor, int IdBrazalete)
        {
            var item = _service.ObtenerAperturaBrazalete(idSupervidor, IdBrazalete);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerListaApertura/{IdPunto}/{IdEstado}")]
        public HttpResponseMessage ObtenerListaApertura(int IdPunto, int IdEstado)
        {
            var item = _service.ObtenerListaApertura(IdPunto, IdEstado);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpGet]
        [Route("api/Apertura/ObtenerAperturasTaquillero/{IdTaquillero}/{IdEstado}")]
        public HttpResponseMessage ObtenerAperturasTaquillero(int IdTaquillero, int IdEstado)
        {
            var item = _service.ObtenerAperturasTaquillero(IdTaquillero, IdEstado);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/AperturaBase/ValidSupervisorElemento")]
        public HttpResponseMessage ActualizarValidSupervisorElemento(List<AperturaElementos> _listElementos)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.ActualizarValidSupervisorElemento(_listElementos));
        }


        [HttpPost]
        [Route("api/AperturaBase/ValidTaquilleroElemento")]
        public HttpResponseMessage ActualizarValidTaquilleroElemento(List<AperturaElementos> modelo)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.ActualizarValidTaquillaElemento(modelo));
        }


        [HttpPost]
        [Route("api/AperturaBase/Insertar")]
        public HttpResponseMessage Insertar(Apertura Apertura)
        {
            var item = _serviceAperturaBase.InsertarAperturaBase(Apertura);
            return item == string.Empty ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/Apertura/InsertarBrazaletes")]
        public HttpResponseMessage InsertarAperturaBrazalete(List<AperturaBrazalete> AperturaBrazalete)
        {
           return  Request.CreateResponse(HttpStatusCode.OK, _service.InsertarAperturaBrazalete(AperturaBrazalete));           
        }

        [HttpPost]
        [Route("api/Apertura/InsertarBrazaleteDetalle")]
        public HttpResponseMessage InsertarBrazaleteDetalle(AperturaBrazaleteDetalle detalle)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.InsertarAperturaBrazaleteDetalle(detalle));
        }

        [HttpPost]
        [Route("api/AperturaBase/Actualizar")]
        public HttpResponseMessage Actualizar(Apertura Apertura)
        {
            var item = _serviceAperturaBase.ActualizarAperturaBase(Apertura);
            return item == string.Empty ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [Route("api/Apertura/ElementosPorIdPunto/{IdPunto}/{Fecha}")]
        public HttpResponseMessage ElementosPorIdPunto(int IdPunto, string Fecha)
        {
            var item = _service.ElementosPorIdPunto(IdPunto, DateTime.Parse(Fecha));
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Apertura/ObtenerAperturaElementoHeader/{Id}")]
        public HttpResponseMessage ObtenerAperturaElementoHeader(int Id)
        {
            var item = _service.ObtenerAperturaElementoHeader(Id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("api/Apertura/ActualizarAperturaElementoHeader")]
        public HttpResponseMessage ActualizarAperturaElementoHeader(AperturaElementosHeader aperturaElementosHeader)
        {
            var item = _service.ActualizarAperturaElementoHeader(aperturaElementosHeader);
            return item == string.Empty ? Request.CreateResponse(HttpStatusCode.OK, "")
                            : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/Apertura/ActualizarAperturaBrazalete")]
        public HttpResponseMessage ActualizarAperturaBrazalete(List<AperturaBrazalete> AperturaBrazalete)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.ActualizarAperturaBrazalete(AperturaBrazalete));
        }

        #region Reabastecimiento

        /// <summary>
        /// RDSH: Retorna los brazaletes que tiene asignado un supervisor o taquillero para poder realizar la impresion
        /// de reabastecimiento.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="EsSupervisor"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Apertura/ObtenerBoleteriaAsignada/{IdUsuario}/{EsSupervisor}")]
        public HttpResponseMessage ObtenerBoleteriaAsignada(int IdUsuario, bool EsSupervisor)
        {
            var item = _service.ObtenerBoleteriaAsignada(IdUsuario, EsSupervisor);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        /// <summary>
        /// RDSH: Retorna los brazaletes que tiene asignado un supervisor
        /// </summary>
        /// <param name="IdSupervisor"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Apertura/ObtenerApeturaBrazaleteSup/{IdSupervisor}")]
        public HttpResponseMessage ObtenerApeturaBrazaleteSup(int IdSupervisor)
        {
            var item = _serviceAperturaBase.ObtenerApeturaBrazalete(IdSupervisor);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        

        #endregion

    }
}