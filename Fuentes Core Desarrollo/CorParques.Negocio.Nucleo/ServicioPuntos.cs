using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioPuntos : IServicioPuntos
    {
        private readonly IRepositorioPuntos _repositorio;

        public ServicioPuntos(IRepositorioPuntos repositorio)
        {
            _repositorio = repositorio;
        }
        public bool Actualizar(Puntos modelo)
        {
            var exist = _repositorio.Obtener(modelo.Id);
            exist.Nombre = modelo.Nombre;
            exist.Descripcion = modelo.Descripcion;
            exist.HoraInicio = modelo.HoraInicio;
            exist.HoraFin = modelo.HoraFin;
            exist.Imagen = modelo.Imagen;
            exist.EstadoId = modelo.EstadoId;
            exist.IdTipoPunto = modelo.IdTipoPunto;
            exist.UsuarioModicifacion = modelo.UsuarioModicifacion;
            exist.FechaModificacion = DateTime.Now;
            exist.CodigoPunto = modelo.CodigoPunto;
            exist.DescripcionCierre = modelo.DescripcionCierre;
            exist.MontoRecaudo = modelo.MontoRecaudo;
            exist.CordenadaX = modelo.CordenadaX;
            exist.CordenadaY = modelo.CordenadaY;
            exist.Latitud = modelo.Latitud;
            exist.Longitud = modelo.Longitud;
            exist.IntervaloTurno = modelo.IntervaloTurno;
            exist.CantidadCupos = modelo.CantidadCupos;
            return _repositorio.Actualizar(ref exist);
        }

        public Puntos Crear(Puntos modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.EstadoId = (int)Enumerador.Estados.Activo;
            modelo.Id = _repositorio.Insertar(ref modelo);
            if (modelo.Id > 0)
            {
                modelo.Imagen = $"{modelo.Id}.png";
                return Actualizar(modelo) ? modelo : null;
            }
            return null;
        }

        public bool Eliminar(int id)
        {
            return _repositorio.EliminarLogica(id);
        }

        public Puntos Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

        public IEnumerable<Puntos> ObtenerTodos()
        {
            return _repositorio.ObtenerListaJoin();
        }

        public IEnumerable<Puntos> ObtenerxTipoPunto(int IdTipoPunto)
        {
            return _repositorio.ObtenerxTipoPunto(IdTipoPunto);
        }

        public IEnumerable<TipoGeneral> ObtenerxIdTipoPunto(int IdTipoPunto)
        {
            return _repositorio.ObtenerxIdTipoPunto(IdTipoPunto);
        }

        public bool ActualizaHoraIdPunto(string HoraInicio, string HoraFin, int IdTipoPunto)
        {
            return _repositorio.ActualizaHoraIdPunto(HoraInicio, HoraFin, IdTipoPunto);
        }

        public IEnumerable<Puntos> ObtenerPuntosRecaudo()
        {
            return _repositorio.ObtenerPuntosRecaudo(); 
        }

        #region Metodos

        /// <summary>
        /// RDSH: Retorna todos los puntos activos para almacenarlos en cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneralValor> ObtenerPuntosCache()
        {
            return _repositorio.ObtenerPuntosCache();
        }

        public IEnumerable<Puntos> ObtenerTodosPuntosActivos()
        {
            return _repositorio.ObtenerTodosPuntosActivos();
        }

        public IEnumerable<Puntos> ObtenerPuntosDestrezaAtracciones()
        {
            return _repositorio.ObtenerPuntosDestrezaAtracciones();
        }

        /// <summary>
        /// RDSH: Valida si un punto (atracción) puede ser utilizado para empezar a operar.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public string ValidarPermisoOperativoPunto(int intIdPunto)
        {
            return _repositorio.ValidarPermisoOperativoPunto(intIdPunto);
        }

        public IEnumerable<Puntos> ObtenerPuntosConAlmacen()
        {
            return _repositorio.ObtenerPuntosConAlmacen() ;
        }
        public IEnumerable<ProductosMesaCocina> ListarProductosMesaCocina()
        {
            return _repositorio.ListarProductosMesaCocina();
        }
        public IEnumerable<ProductosMesaCocinaGroup> ListarProductosMesaCocinaGroup()
        {
            return _repositorio.ListarProductosMesaCocinaGroup();
        }
        public IEnumerable<ColorTiempoRestaurante> ListarColorTiempoRestaurante()
        {
            return _repositorio.ListarColorTiempoRestaurante();
        }
        
        public string ActualizarDetalleProducto(int IdDetallePedido, int EstadoDetallePedido)
        {
            return _repositorio.ActualizarDetalleProducto(IdDetallePedido, EstadoDetallePedido);
        }
        
        public IEnumerable<TipoAcompanamiento> ListarTipoAcompGroup()
        {
            return _repositorio.ListarTipoAcompGroup();
        }
        
        public IEnumerable<Mesa> ObtenerMesas()
        {
            return _repositorio.ObtenerMesas();
        }
        public IEnumerable<ZonaRestaurante> ObtenerZonasRestaurante()
        {
            return _repositorio.ObtenerZonasRestaurante();
        }
        
        public IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaRestaurante()
        {
            return _repositorio.ObtenerTipoAcompaRestaurante();
        }
        public IEnumerable<TipoGeneral> ObtenerTipoProductosRestaurante()
        {
            return _repositorio.ObtenerTipoProductosRestaurante();
        }
        
        public IEnumerable<Mesa> ObtenerMesasActivas(int? idUsuario)
        {
            return _repositorio.ObtenerMesasActivas(idUsuario);
        }
        public TipoAcompanamiento ObtenerTipoAcompaRestauranteXId(int? idTipoAcompa)
        {
            return _repositorio.ObtenerTipoAcompaRestauranteXId(idTipoAcompa);
        }
        public string EliminarTipoAcompaRestaurante(int? idTipoAcompa)
        {
            return _repositorio.EliminarTipoAcompaRestaurante(idTipoAcompa);
        }
        

        public IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaXProductoRestaurante(int? id)
        {
            return _repositorio.ObtenerTipoAcompaXProductoRestaurante(id);
        }
        public IEnumerable<Producto> ObtenerProductoAdminRestaurante(int? id)
        {
            return _repositorio.ObtenerProductoAdminRestaurante(id);
        }
        
        public IEnumerable<Acompanamiento> ObtenerAcompaXproductoAdmin(int? id)
        {
            return _repositorio.ObtenerAcompaXproductoAdmin(id);
        }
        
        public ListaAcomProductos ListarProductosMesa(int idMesa)
        {
            return _repositorio.ListarProductosMesa(idMesa);
        }
         public ListaAcomProductos ListarProductosFactura(int idFactura)
        {
            return _repositorio.ListarProductosFactura(idFactura);
        }
        public IEnumerable<AcompanamientoXProducto> ObtenerAcompaXproducto(int idProducto)
        {
            return _repositorio.ObtenerAcompaXproducto(idProducto);
        }
        public string CerrarMesa(int idPedido)
        {
            return _repositorio.CerrarMesa(idPedido);
        }
        public string AnularPedido(int idPedido)
        {
            return _repositorio.AnularPedido(idPedido);
        }
        
        public int ValidarEstadoPedido(int idPedido)
        {
            return _repositorio.ValidarEstadoPedido(idPedido);
        }
        
        /// <summary>
        ///RDSH: Retorna las observaciones del mantenimiento. 
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public string ObservacionesMantenimiento(int intIdPunto)
        {
            return _repositorio.ObservacionesMantenimiento(intIdPunto);
        }
        
        #endregion

    }
}
