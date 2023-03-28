using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioPuntos : IServicioBase<Puntos>
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();
        IEnumerable<TipoGeneral> ObtenerxIdTipoPunto(int IdTipoPunto);
        bool Eliminar(int id);
        IEnumerable<Puntos> ObtenerxTipoPunto(int IdTipoPunto);
        IEnumerable<Puntos> ObtenerPuntosXusuario(int IdUsuario);
        IEnumerable<Puntos> ObtenerPuntosXProducto(int IdProducto);
        
        bool ActualizaHoraIdPunto(string HoraInicio, string HoraFin, int IdTipoPunto);

        IEnumerable<Puntos> ObtenerPuntosRecaudo();

        // RDSH: Retorna todos los puntos activos para almacenarlos en cache.        
        IEnumerable<TipoGeneralValor> ObtenerPuntosCache();

        //RDSH: Obtiene el listado de puntos activos.
        IEnumerable<Puntos> ObtenerTodosPuntosActivos();

        //RDSH: Retorna el listado de destrezas y atracciones.
        IEnumerable<Puntos> ObtenerPuntosDestrezaAtracciones();

        //RDSH: Valida si un punto (atracción) puede ser utilizado para empezar a operar.
        string ValidarPermisoOperativoPunto(int intIdPunto);

        IEnumerable<Puntos> ObtenerPuntosConAlmacen();

        IEnumerable<ProductosMesaCocina> ListarProductosMesaCocina();
        IEnumerable<ProductosMesaCocinaGroup> ListarProductosMesaCocinaGroup();
        IEnumerable<ColorTiempoRestaurante> ListarColorTiempoRestaurante();
        
        string ActualizarDetalleProducto(int IdDetallePedido, int EstadoDetallePedido);


        IEnumerable<TipoAcompanamiento> ListarTipoAcompGroup();

        IEnumerable<Mesa> ObtenerMesas();
        IEnumerable<ZonaRestaurante> ObtenerZonasRestaurante();
        
        IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaRestaurante();
        IEnumerable<TipoGeneral> ObtenerTipoProductosRestaurante();

        IEnumerable<AcompanamientoXProducto> ObtenerAcompaXproducto(int idProducto);

        string CerrarMesa(int idPedido);
        string AnularPedido(int idPedido);

        int ValidarEstadoPedido(int idPedido);

        ListaAcomProductos ListarProductosMesa(int IdMesa);
        ListaAcomProductos ListarProductosFactura(int IdFactura);
        IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaXProductoRestaurante(int? id);
        IEnumerable<Producto> ObtenerProductoAdminRestaurante(int? id);

        
        IEnumerable<Acompanamiento> ObtenerAcompaXproductoAdmin(int? id);
        
        IEnumerable<Mesa> ObtenerMesasActivas(int? idUsuario);

        TipoAcompanamiento ObtenerTipoAcompaRestauranteXId(int? idTipoAcompa);
        string EliminarTipoAcompaRestaurante(int? idTipoAcompa);

        

        // RDSH: Retorna las observaciones del mantenimiento.
        string ObservacionesMantenimiento(int intIdPunto);
    }
}
