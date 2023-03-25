using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioProducto : IRepositorioBase<Producto>, IRepositorioFactura, IRepositorioLineaProducto
	{
        string InsertarPedidoR(PagoFactura model);
        string AgregarProducAraza(PagoFactura model);
        
        string ActualizarTipoAcompaRestaurante(TipoAcompanamiento model);
        
        string GuardarAcompaProduAdmin(ProductoxAcompanamiento model);
        string ActualizarProductoAdminRestaurante(Producto model);
        
        string UpdatePedidoRestFactura(PagoFactura model);
        
        Producto ObtenerBrazaleteConsecutivo(string Consecutivo,int taquillero, int recarga);

        //bool ActualizarBrazaleteEstado(List<Producto> lista);

        Producto ObtenerProducto(int idProducto);
        Producto ObtenerProductoPtoEntrega(int idProducto);
        Producto ObtenerProductoPtoFactura(int idProducto);

        IEnumerable<Producto> ObtenerProductos();

        IEnumerable<Producto> ObtenerProductosPtoEntrega();
        IEnumerable<Producto> ObtenerProductosPtoFactura();

        

        bool ActualizarProducto(Producto modelo);
        bool ActualizarProductoPuntosEntrega(Producto modelo);

        bool ActualizarProductoPuntosFactura(Producto modelo);

        int GuardarNotaCredito(NotaCredito modelo);

        TipoGeneral ObtenerNotaCredito(int IdUsuario);

        TipoGeneral ObtenerAnulaciones(int IdUsuario);

        IEnumerable<TipoGeneral> ObtenerProductosDonacion();

        IEnumerable<Producto> ObtenerProductoPorTipoProducto(string CodSapTipoProducto, bool activo= false);

        LineaProducto ObtenerCodSapPorTipoProducto(int IdTipoProducto);

        string ValidaBoletaControlProducto(string CodigoBarrasBoletaControl, string CodSapProducto);

        DescargueBoletaControl ObtenerProductosBoletaControl(string CodBarra, int usuario);

        DescargueBoletaControl ObtenerListaDescargue(string CodBarra);

        DescargueBoletaControl ObtenerProductoInstitucional(string CodBarra);

        DescargueBoletaControl ObtenerProductosInstitucional(string CodBarra1, string CodBarra2);

        string DescargarProductosInstitucional(ImprimirBoletaControl modelo);

        string DescargueBoletaFactura(List<Producto> listaProductos);

        IEnumerable<Producto> ObtenerTodosProductos();
        IEnumerable<Producto> ObtenerProductosXPuntoSurtido();

        
        IEnumerable<Producto> ObtenerTodosProductosRestaurante();
        
        RedencionBoletaControl RedencionBoletaControl(ImprimirBoletaControl modelo);

        Factura ObtenerUltimaFactura(int IdPunto);

        FacturaImprimir ObtenerFacturaImprimir(int IdFactura);

        FacturaImprimir ObtenerFacturaNotaCredImprimir(int IdFactura, int IdNotaCredito);

        ProductoBoleta BuscarBoleta(string CodBarraInicio, string CodBarraFinal, string Codproducto, int usuario);

        Producto ObtenerProductoPorCodSapProd_TipoProd(string CodSapTipoProducto, string CodSapProducto);

        int GuardarLogVenta(LogVentaPunto modelo);

        /// <summary>
        /// RDSH: Retorna los productos que se van a entregar desde destrezas.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Producto> ObtenerPremiosDestrezas();

        /// <summary>
        /// EDSP: Obtener pedidos y productos del día - venta institucional
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductosPedidos> ObtenerProductosPedidosDia();
        IEnumerable<ProductosPedidos> ObtenerPedidosTraslado();
        IEnumerable<SolicitudRetorno> ObtenerPedidosEntregaAsesor(int idPunto);
        IEnumerable<PuntoBrazalete> ObtenerPasaporteXpunto(int idPunto);
        IEnumerable<PuntoBrazalete> ObtenerPasaporteXPuntoPOS(int idpunto, int idUsuario);
        bool ActualizarPasaporteXPunto(List<PuntoBrazalete> puntoBrazalete);
        ArchivoBrazalete ObtenerArchivoBrazalete(int idProducto);
        bool ActualizarArchivoBrazalete(ArchivoBrazalete archivoBrazalete);
        Producto ObtenerProductoPorCodSap(string CodSapProducto);
        IEnumerable<BrazaleteReimpresion> ObtenerBrazaleteReimpresion(int idPunto);
        List<Producto> ObtenerListaa(string[] Cod, List<string> tipo);
        bool InsertarPlanillaBrazalete(PlantillaBrazalete plantilla);
        IEnumerable<PlantillaBrazalete> ObtenerPlantillaBrazalete();

    }
}
