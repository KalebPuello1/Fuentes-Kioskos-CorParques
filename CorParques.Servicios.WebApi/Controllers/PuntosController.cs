using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorParques.Transversales.Util;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class PuntosController : ApiController
    {
        private readonly IServicioPuntos _servicio;

        public PuntosController(IServicioPuntos servicio)
        {
            _servicio = servicio;
        }
        [HttpGet]
        [Route("api/Puntos/GetAll")]
        public HttpResponseMessage GetAll()
        {
            var list = _servicio.ObtenerTodos();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/ObtenerPuntosRecaudo")]
        public HttpResponseMessage ObtenerPuntosRecaudo()
        {
            var list = _servicio.ObtenerPuntosRecaudo();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/GetAllxTipoPunto/{idTipoPunto}")]
        public HttpResponseMessage GetAllxTipoPunto(int idTipoPunto)
        {
            var list = _servicio.ObtenerxTipoPunto (idTipoPunto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/ObtenerPuntosXusuario/{IdUsuario}")]
        public HttpResponseMessage ObtenerPuntosXusuario(int IdUsuario)
        {
            var list = _servicio.ObtenerPuntosXusuario(IdUsuario);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpGet]
        [Route("api/Puntos/GetAllSimple")]
        public HttpResponseMessage GetAllSimple()
        {
            var list = _servicio.ObtenerListaSimple();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/GetById/{id}")]
        public HttpResponseMessage Obtener(int id)
        {
            var item = _servicio.Obtener(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Puntos/ObtenerxIdTipoPunto/{id}")]
        public HttpResponseMessage ObtenerxIdTipoPunto(int id)
        {
            var item = _servicio.ObtenerxIdTipoPunto(id);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpPost]
        [Route("api/Puntos/Insert")]
        public HttpResponseMessage Crear(Puntos modelo)
        {
            var item = _servicio.Crear(modelo);
            return item == null ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Se presento un error creando el parámetro. Por favor intentelo de nuevo")
                            : Request.CreateResponse(HttpStatusCode.OK, item.Id);
        }
        [HttpPut]
        [Route("api/Puntos/Update")]
        public HttpResponseMessage Actualizar(Puntos modelo)
        {
            var item = _servicio.Actualizar(modelo);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Se presento un error actualizando el parámetro. Por favor intentelo de nuevo")
                            : Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [HttpDelete]
        [Route("api/Puntos/Delete/{id}")]
        public HttpResponseMessage Eliminar(int id)
        {
            var item = _servicio.Eliminar(id);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPut]
        [Route("api/Puntos/ActualizarHora")]
        public HttpResponseMessage ActualizarHora(string HoraInicial, string HoraFin, int IdTipoPunto)
        {
            var item = _servicio.ActualizaHoraIdPunto(HoraInicial, HoraFin, IdTipoPunto);
            return !item ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Se presento un error actualizando el parámetro. Por favor intentelo de nuevo")
                            : Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [HttpGet]
        [Route("api/Puntos/ObtenerPuntosCache")]
        public HttpResponseMessage ObtenerPuntosCache()
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneralValor>>("TodosPuntosCache");
            if (list == null)
            {
                list = _servicio.ObtenerPuntosCache();
                Cache.SetCache("TodosPuntosCache", list, Cache.Long);
            }
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/ObtenerTodosPuntosActivos")]
        public HttpResponseMessage ObtenerTodosPuntosActivos()
        {            
            var list = _servicio.ObtenerTodosPuntosActivos();            
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/ObtenerPuntosDestrezaAtracciones")]
        public HttpResponseMessage ObtenerPuntosDestrezaAtracciones()
        {
            var list = _servicio.ObtenerPuntosDestrezaAtracciones();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/ValidarPermisoOperativoPunto/{IdPunto}")]
        public HttpResponseMessage ValidarPermisoOperativoPunto(int IdPunto)
        {
            string strResultado = string.Empty;
            strResultado = _servicio.ValidarPermisoOperativoPunto(IdPunto);
            return Request.CreateResponse(HttpStatusCode.OK, strResultado);

        }

        [HttpGet]
        [Route("api/Puntos/ObtenerPuntosConAlmacen")]
        public HttpResponseMessage ObtenerPuntosConAlmacen()
        {
            var list = _servicio.ObtenerPuntosConAlmacen ();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ListarProductosMesaCocina")]
        public HttpResponseMessage ListarProductosMesaCocina()
        {
            var list = _servicio.ListarProductosMesaCocina();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ListarProductosMesaCocinaGroup")]
        public HttpResponseMessage ListarProductosMesaCocinaGroup()
        {
            var list = _servicio.ListarProductosMesaCocinaGroup();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ListarColorTiempoRestaurante")]
        public HttpResponseMessage ListarColorTiempoRestaurante()
        {
            var list = _servicio.ListarColorTiempoRestaurante();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        
        [HttpGet, Route("api/Puntos/ActualizarDetalleProducto/{IdDetallePedido}/{EstadoDetallePedido}")]
        public HttpResponseMessage ActualizarDetalleProducto(int IdDetallePedido, int EstadoDetallePedido)
        {
            var _rta = _servicio.ActualizarDetalleProducto(IdDetallePedido, EstadoDetallePedido);
            return Request.CreateResponse(HttpStatusCode.OK, _rta);
        }
        [HttpGet]
        [Route("api/Puntos/ListarTipoAcompGroup")]
        public HttpResponseMessage ListarTipoAcompGroup()
        {
            var list = _servicio.ListarTipoAcompGroup();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        

        [HttpGet]
        [Route("api/Puntos/ObtenerMesas")]
        public HttpResponseMessage ObtenerMesas()
        {
            var list = _servicio.ObtenerMesas();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ObtenerZonasRestaurante")]
        public HttpResponseMessage ObtenerZonasRestaurante()
        {
            var list = _servicio.ObtenerZonasRestaurante();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        
        [HttpGet]
        [Route("api/Puntos/ObtenerTipoAcompaRestaurante")]
        public HttpResponseMessage ObtenerTipoAcompaRestaurante()
        {
            var list = _servicio.ObtenerTipoAcompaRestaurante();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ObtenerTipoProductosRestaurante")]
        public HttpResponseMessage ObtenerTipoProductosRestaurante()
        {
            var list = _servicio.ObtenerTipoProductosRestaurante();
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ObtenerMesasActivas/{idUsuario?}")]
        public HttpResponseMessage ObtenerMesasActivas(int? idUsuario)
        {
            var list = _servicio.ObtenerMesasActivas(idUsuario);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("api/Puntos/ObtenerTipoAcompaRestauranteXId/{idTipoAcompa?}")]
        public HttpResponseMessage ObtenerTipoAcompaRestauranteXId(int? idTipoAcompa)
        {
            var item = _servicio.ObtenerTipoAcompaRestauranteXId(idTipoAcompa);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }
        [HttpGet]
        [Route("api/Puntos/EliminarTipoAcompaRestaurante/{idTipoAcompa?}")]
        public HttpResponseMessage EliminarTipoAcompaRestaurante(int? idTipoAcompa)
        {
            var item = _servicio.EliminarTipoAcompaRestaurante(idTipoAcompa);
            return item == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/Puntos/ObtenerAcompaXproducto/{idProducto}")]
        public HttpResponseMessage ObtenerAcompaXproducto(int idProducto)
        {
            var list = _servicio.ObtenerAcompaXproducto(idProducto);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ObtenerTipoAcompaXProductoRestaurante/{id}")]
        public HttpResponseMessage ObtenerTipoAcompaXProductoRestaurante(int id)
        {
            var list = _servicio.ObtenerTipoAcompaXProductoRestaurante(id);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ObtenerProductoAdminRestaurante/{id}")]
        public HttpResponseMessage ObtenerProductoAdminRestaurante(int id)
        {
            var list = _servicio.ObtenerProductoAdminRestaurante(id);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        
        [HttpGet]
        [Route("api/Puntos/ObtenerAcompaXproductoAdmin/{id}")]
        public HttpResponseMessage ObtenerAcompaXproductoAdmin(int id)
        {
            var list = _servicio.ObtenerAcompaXproductoAdmin(id);
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/CerrarMesa/{idPedido}")]
        public HttpResponseMessage CerrarMesa(int idPedido)
        {

            string strResultado = string.Empty;
            strResultado = _servicio.CerrarMesa(idPedido);
            return Request.CreateResponse(HttpStatusCode.OK, strResultado);
        }
        [HttpGet]
        [Route("api/Puntos/AnularPedido/{idPedido}")]
        public HttpResponseMessage AnularPedido(int idPedido)
        {

            string strResultado = string.Empty;
            strResultado = _servicio.AnularPedido(idPedido);
            return Request.CreateResponse(HttpStatusCode.OK, strResultado);
        }
        
        [HttpGet]
        [Route("api/Puntos/ValidarEstadoPedido/{idPedido}")]
        public HttpResponseMessage ValidarEstadoPedido(int idPedido)
        {
            var list = _servicio.ValidarEstadoPedido(idPedido);
            return list == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                                       : Request.CreateResponse(HttpStatusCode.OK, list);
        }


        [HttpGet]
        [Route("api/Puntos/ListarProductosMesa/{idMesa}")]
        public HttpResponseMessage ListarProductosMesa(int idMesa)
        {
            var list = _servicio.ListarProductosMesa(idMesa);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                              : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Puntos/ListarProductosFactura/{idFactura}")]
        public HttpResponseMessage ListarProductosFactura(int idFactura)
        {
            var list = _servicio.ListarProductosFactura(idFactura);
            return list == null ? Request.CreateResponse(HttpStatusCode.NotFound)
                              : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        

        [HttpGet]
        [Route("api/Puntos/ObservacionesMantenimiento/{IdPunto}")]
        public HttpResponseMessage ObservacionesMantenimiento(int IdPunto)
        {
            string strResultado = string.Empty;
            strResultado = _servicio.ObservacionesMantenimiento(IdPunto);
            return Request.CreateResponse(HttpStatusCode.OK, strResultado);

        }

    }
}
