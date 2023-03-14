using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioPuntos : RepositorioBase<Puntos>, IRepositorioPuntos
    {
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _cnn.GetList<Puntos>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
        }

        public IEnumerable<TipoGeneral> ObtenerxIdTipoPunto(int IdTipoPunto)
        {
            return _cnn.GetList<Puntos>().Where(x => x.IdTipoPunto == IdTipoPunto).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
        }

        public IEnumerable<Puntos> ObtenerListaJoin()
        {
            var rta = _cnn.QueryMultiple("SP_GetPuntos", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().ToList();
            var status = rta.Read<TipoGeneral>();
            attractions.ForEach(x => x.Estado = status.First(y => y.Id.Equals(x.EstadoId)).Nombre);
            
            return attractions;
        }

        public IEnumerable<Puntos> ObtenerPuntosRecaudo()
        {
            var rta = _cnn.QueryMultiple("SP_GetPuntos", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().Where(x=>
                                x.IdTipoPunto.Equals((int)Enumerador.TiposPuntos.Comida) || 
                                x.IdTipoPunto.Equals((int)Enumerador.TiposPuntos.Taquilla) || 
                                x.IdTipoPunto.Equals((int)Enumerador.TiposPuntos.Destreza)).ToList();
            var status = rta.Read<TipoGeneral>();

            attractions.ForEach(x => x.Estado = status.First(y => y.Id.Equals(x.EstadoId)).Nombre);

            return attractions;
        }

        public IEnumerable<Puntos> ObtenerxTipoPunto(int IdTipoPunto)
        {
            var rta = _cnn.QueryMultiple("SP_GetPuntos", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().ToList();
            var status = rta.Read<TipoGeneral>();
            attractions.ForEach(x => x.Estado = status.First(y => y.Id.Equals(x.EstadoId)).Nombre);
            return attractions.Where(x => x.IdTipoPunto == IdTipoPunto).ToList();             
        }
        public IEnumerable<Puntos> ObtenerPuntosXusuario(int IdUsuario)
        {
            var rta = _cnn.QueryMultiple("SP_GetPuntosXIdUsuario", param: new { IdUsuario = IdUsuario }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().ToList();
            return attractions;
        }
        
        public bool EliminarLogica(int id)
        {
            var item = _cnn.Get<Puntos>(id);
            item.EstadoId = 2;
            return _cnn.Update(item) > 0;
        }

        public bool ActualizaHoraIdPunto(string HoraInicio, string HoraFin, int IdTipoPunto)
        {

            try
            {
                _cnn.Query<bool>("SP_AplicaHoraTodosPunto",
                param: new { HORAINICIO = HoraInicio, HORAFIN = HoraFin, IdTipoPunto = IdTipoPunto },
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        /// <summary>
        /// RDSH: Retorna todos los puntos activos para almacenarlos en cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneralValor> ObtenerPuntosCache()
        {
            try
            {

                var objRecetaProducto = _cnn.Query<TipoGeneralValor>("SP_ObtenerPuntosCache",null, commandType: CommandType.StoredProcedure).ToList();

                return objRecetaProducto;

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ObtenerPuntosCache_RepositorioPuntos: ", ex.Message));
            }
            
        }

        /// <summary>
        /// RDSH: Obtiene el listado de puntos activos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Puntos> ObtenerTodosPuntosActivos()
        {
            var rta = _cnn.QueryMultiple("SP_GetPuntos", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().Where(x => x.EstadoId.Equals((int)Enumerador.Estados.Activo)).ToList();
            var status = rta.Read<TipoGeneral>();
            attractions.ForEach(x => x.Estado = status.First(y => y.Id.Equals(x.EstadoId)).Nombre);
            return attractions;
        }

        /// <summary>
        /// RDSH: Retorna el listado de destrezas y atracciones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Puntos> ObtenerPuntosDestrezaAtracciones()
        {
            var rta = _cnn.QueryMultiple("SP_GetPuntos", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().Where(x =>
                                x.IdTipoPunto.Equals((int)Enumerador.TiposPuntos.Atraccion) ||
                                x.IdTipoPunto.Equals((int)Enumerador.TiposPuntos.Destreza)).ToList();
            var status = rta.Read<TipoGeneral>();

            attractions.ForEach(x => x.Estado = status.First(y => y.Id.Equals(x.EstadoId)).Nombre);

            return attractions;
        }

        /// <summary>
        /// RDSH: Valida si un punto (atracción) puede ser utilizado para empezar a operar.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public string ValidarPermisoOperativoPunto(int intIdPunto)
        {

            string strRespuesta = string.Empty;
               
            try
            {
                strRespuesta = _cnn.Query<string>("SP_ValidarPermisoOperativoPunto", param: new
                {
                    IdPunto = intIdPunto
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                strRespuesta = string.Concat("Error en ValidarPermisoOperativoPunto_RepositorioPuntos: ", ex.Message);
            }

            return strRespuesta;
        }
        /// <summary>
        /// GALD: Obtiene el listado de puntos con almacen.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Puntos> ObtenerPuntosConAlmacen()
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerPuntosconAlmacen", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Puntos>().Where(x => x.EstadoId.Equals((int)Enumerador.Estados.Activo)).ToList();           
            return attractions;
        }
        /// <summary>
        /// GALD: Obtiene el listado mesas cocina.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductosMesaCocinaGroup> ListarProductosMesaCocinaGroup()
        {
            var rta = _cnn.QueryMultiple("SP_ListarProductosMesaCocinaGroup", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<ProductosMesaCocinaGroup>().ToList();
            return attractions;
        }

        public IEnumerable<ColorTiempoRestaurante> ListarColorTiempoRestaurante()
        {
            var rta = _cnn.QueryMultiple("SP_ListarColorTiempoRestaurante", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<ColorTiempoRestaurante>().ToList();
            return attractions;
        }
        public string ActualizarDetalleProducto(int IdDetallePedido, int EstadoDetallePedido)
        {

            string strRespuesta = string.Empty;

            try
            {
                strRespuesta = _cnn.Query<string>("SP_ActualizarDetalleProducto", param: new
                {
                    IdDetallePedido = IdDetallePedido,
                    IdEstado = EstadoDetallePedido
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                strRespuesta = string.Concat("Error en ValidarPermisoOperativoPunto_RepositorioPuntos: ", ex.Message);
            }

            return strRespuesta;
        }
        
        public IEnumerable<TipoAcompanamiento> ListarTipoAcompGroup()
        {
            var rta = _cnn.QueryMultiple("SP_ListarTipoAcompGroup", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<TipoAcompanamiento>().ToList();
            return attractions;
        }
        
        /// <summary>
        /// GALD: Obtiene el listado productos por mesa cocina.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductosMesaCocina> ListarProductosMesaCocina()
        {
            var rta = _cnn.QueryMultiple("SP_ListarProductosMesaCocina", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<ProductosMesaCocina>().ToList();
            return attractions;
        }
        /// <summary>
        /// GALD: Obtiene el listado mesas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Mesa> ObtenerMesas()
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerMesas", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Mesa>().ToList();
            return attractions;
        }
        public IEnumerable<ZonaRestaurante> ObtenerZonasRestaurante()
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerZonasRestaurante", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<ZonaRestaurante>().ToList();
            return attractions;
        }
        
        public IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaRestaurante()
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerTipoAcompaRestaurante", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<TipoAcompanamiento>().ToList();
            return attractions;
        }
        public IEnumerable<TipoGeneral> ObtenerTipoProductosRestaurante()
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerTipoProductosRestaurante", commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<TipoGeneral>().ToList();
            return attractions;
        }
        

        /// <summary>
        /// GALD: Obtiene el listado mesas activas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Mesa> ObtenerMesasActivas(int? idUsuario)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerMesasUsuario", param: new
            {
                IdUsuario = idUsuario
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Mesa>().ToList();
            return attractions;
        }
        public TipoAcompanamiento ObtenerTipoAcompaRestauranteXId(int? idTipoAcompa)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerTipoAcompaRestauranteXId", param: new
            {
                IdTipoAcomp = idTipoAcompa
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<TipoAcompanamiento>().FirstOrDefault();
            return attractions;
        }

        public string EliminarTipoAcompaRestaurante(int? idTipoAcompa)
        {
            var rta = _cnn.QueryMultiple("SP_EliminarTipoAcompaRestaurante", param: new
            {
                IdTipoAcomp = idTipoAcompa
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.ToString();
            return attractions;
        }


        public IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaXProductoRestaurante(int? id)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerTipoAcompaXProductoRestaurante", param: new
            {
                IdProducto = id
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<TipoAcompanamiento>().ToList();
            return attractions;
        }
        public IEnumerable<Producto> ObtenerProductoAdminRestaurante(int? id)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerProductoAdminRestaurante", param: new
            {
                IdProducto = id
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Producto>().ToList();
            return attractions;
        }
        
        public IEnumerable<Acompanamiento> ObtenerAcompaXproductoAdmin(int? id)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerAcompaXproductoAdmin", param: new
            {
                IdProducto = id
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<Acompanamiento>().ToList();
            return attractions;
        }
        

        /// <summary>
        /// GALD: Obtiene el listado mesas activas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AcompanamientoXProducto> ObtenerAcompaXproducto(int idProducto)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerAcompaXproducto", param: new
            {
                IdProducto = idProducto
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<AcompanamientoXProducto>().ToList();
            return attractions;
        }
        public string CerrarMesa(int idPedido)
        {

            string strRespuesta = string.Empty;

            try
            {
                strRespuesta = _cnn.Query<string>(sql: "SP_ConsultarPedidoFactura", param: new
                {
                    IdPedido = idPedido
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
               
                if(strRespuesta != null)
                {
                    strRespuesta = _cnn.Query<string>(sql: "SP_CerrarMesaPedidoRestaurante", param: new
                    {
                        IdPedido = idPedido
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    strRespuesta = "SI";
                }
                else
                {
                    strRespuesta = "NO";
                }
              
                return strRespuesta;
            }
            catch (Exception ex)
            {
                //strRespuesta = string.Concat("Error en CerrarMesaPedidoRestaurante: ", ex.Message);
                strRespuesta = "NO";
            }

            return strRespuesta;


        }
        public string AnularPedido(int idPedido)
        {

            string strRespuesta = string.Empty;

            try
            {
                strRespuesta = _cnn.Query<string>(sql: "SP_ConsultarPedidoFactura", param: new
                {
                    IdPedido = idPedido
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (strRespuesta != null)
                {
                    strRespuesta = _cnn.Query<string>(sql: "SP_AnularPedidoRestaurante", param: new
                    {
                        IdPedido = idPedido
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    strRespuesta = "SI";
                }
                else
                {
                    strRespuesta = "NO";
                }

                return strRespuesta;
            }
            catch (Exception ex)
            {
                //strRespuesta = string.Concat("Error en CerrarMesaPedidoRestaurante: ", ex.Message);
                strRespuesta = "NO";
            }

            return strRespuesta;


        }
        public int ValidarEstadoPedido(int idPedido)
        {
            var rta = _cnn.Query<int>("SP_ValidarEstadoPedido", param: new
            {
                IdPedido = idPedido
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.FirstOrDefault();
            return attractions;
        }
        
        public ListaAcomProductos ListarProductosMesa(int idMesa)
        {
            var listaprodicmesa = new ListaAcomProductos();
            var rta = _cnn.QueryMultiple("SP_ListarProductosMesa", param: new
            {
                IdMesa = idMesa
            }, commandType: System.Data.CommandType.StoredProcedure);
            listaprodicmesa.listaProductos = rta.Read<AcompPedido>().ToList();
            foreach (var item in listaprodicmesa.listaProductos)
            {
                var rta2 = _cnn.QueryMultiple("SP_ListarAcompaProductoPe", param: new
                {
                    IdPedido = item.Id_Pedido
                }, commandType: System.Data.CommandType.StoredProcedure);
                listaprodicmesa.listaAcompa = rta2.Read<AcompPedido>().ToList();
            }
           
            return listaprodicmesa;
        }
        public ListaAcomProductos ListarProductosFactura(int idFactura)
        {
            var listaprodicmesa = new ListaAcomProductos();
            var rta = _cnn.QueryMultiple("SP_ListarProductosFactura", param: new
            {
                idFactura = idFactura
            }, commandType: System.Data.CommandType.StoredProcedure);
            listaprodicmesa.listaProductos = rta.Read<AcompPedido>().ToList();
            foreach (var item in listaprodicmesa.listaProductos)
            {
                var rta2 = _cnn.QueryMultiple("SP_ListarAcompaProductoPe", param: new
                {
                    IdPedido = item.Id_Pedido
                }, commandType: System.Data.CommandType.StoredProcedure);
                listaprodicmesa.listaAcompa = rta2.Read<AcompPedido>().ToList();
            }

            return listaprodicmesa;
        }


        /// <summary>
        /// RDSH: Retorna las observaciones del mantenimiento.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public string ObservacionesMantenimiento(int intIdPunto)
        {

            string strRespuesta = string.Empty;

            try
            {
                strRespuesta = _cnn.Query<string>("SP_ObtenerObservacionesMantenimientoPunto", param: new
                {
                    IdPunto = intIdPunto
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                strRespuesta = string.Concat("Error en ValidarPermisoOperativoPunto_RepositorioPuntos: ", ex.Message);
            }

            return strRespuesta;
        }
    }
}
