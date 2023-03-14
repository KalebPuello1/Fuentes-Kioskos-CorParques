using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioPuntos : IRepositorioBase<Puntos>
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();
        IEnumerable<Puntos> ObtenerListaJoin();
        IEnumerable<TipoGeneral> ObtenerxIdTipoPunto(int IdTipoPunto);
        IEnumerable<Puntos> ObtenerxTipoPunto(int IdTipoPunto);
        IEnumerable<Puntos> ObtenerPuntosXusuario(int IdUsuario);
        
        bool EliminarLogica(int id);
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
        
        IEnumerable<ZonaRestaurante> ObtenerZonasRestaurante();
        IEnumerable<Mesa> ObtenerMesas();
        IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaRestaurante();
        IEnumerable<TipoGeneral> ObtenerTipoProductosRestaurante();
        
        IEnumerable<Mesa> ObtenerMesasActivas(int? idUsuario);
        TipoAcompanamiento ObtenerTipoAcompaRestauranteXId(int? idTipoAcompa);
        string EliminarTipoAcompaRestaurante(int? idTipoAcompa);
        
        IEnumerable<TipoAcompanamiento> ObtenerTipoAcompaXProductoRestaurante(int? id);
        IEnumerable<Producto> ObtenerProductoAdminRestaurante(int? id);
        
        IEnumerable<Acompanamiento> ObtenerAcompaXproductoAdmin(int? id);
        
        IEnumerable<AcompanamientoXProducto> ObtenerAcompaXproducto(int idProducto);
        string CerrarMesa(int IdPedido);
        string AnularPedido(int IdPedido);
        
        int ValidarEstadoPedido (int idPedido);
        
        ListaAcomProductos ListarProductosMesa(int IdMesa);
        ListaAcomProductos ListarProductosFactura(int IdFactura);
        

        // RDSH: Retorna las observaciones del mantenimiento.
        string ObservacionesMantenimiento(int intIdPunto);

    }
}
