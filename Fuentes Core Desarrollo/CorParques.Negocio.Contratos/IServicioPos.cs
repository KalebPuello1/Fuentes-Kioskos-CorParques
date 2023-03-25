using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioPos
    {
        Producto ObtenerBrazaleteConsecutivo(string Consecutivo,int taquillero, int recarga);
        //bool ActualizarBrazaleteEstado(List<Producto> lista);
        string InsertarPedidoR(PagoFactura modelo, ref string Error);

        string AgregarProducAraza(PagoFactura modelo, ref string Error);
        
        string ActualizarTipoAcompaRestaurante(TipoAcompanamiento modelo, ref string Error);

        
        string GuardarAcompaProduAdmin(ProductoxAcompanamiento modelo, ref string Error);

        string ActualizarProductoAdminRestaurante(Producto modelo, ref string Error);
        
        string UpdatePedidoRestFactura(PagoFactura modelo, ref string Error);
        
        string InsertarCompra(PagoFactura modelo, ref string Error);
        string ValidarCompra(PagoFactura modelo);
        Producto ObtenerProducto(int IdProducto);
        Producto ObtenerProductoPtoEntrega(int IdProducto);
        Producto ObtenerProductoPtoFactura(int IdProducto);

        IEnumerable<Producto> ObtenerProductos();
        IEnumerable<Producto> ObtenerProductosPtoEntrega();
        IEnumerable<Producto> ObtenerProductosPtoFactura();
        
        IEnumerable<TipoGeneral> ObtenerLineaproductos();
        IEnumerable<TipoGeneral> ObtenerProductosDonacion();
        bool ActualizarProducto(Producto modelo);
        bool ActualizarProductoPuntosEntrega(Producto modelo);
        bool ActualizarProductoPuntosFactura(Producto modelo);
        Factura ObtenerFactura(string codigoFactura);
        Factura ObtenerFactura(int idFactura);
        int GuardarNotaCredito(NotaCredito modelo);
        TipoGeneral ObtenerNotaCredito(int IdUsuario);
        TipoGeneral ObtenerAnulaciones(int IdUsuario);
        IEnumerable<AnulacionFactura> ObtenerFacturasAnular(int idPunto);
        IEnumerable<AnulacionFacturaRedeban> ObtenerFacturasRedebanAnular(int idPunto);
        string ValidaPermiteAnular(int idPunto);
        bool AnularFacturas(IEnumerable<AnulacionFactura> modelo);
        IEnumerable<MedioPagoFactura> ValidarTipoFactura(int idFactura);
        IEnumerable<Producto> ObtenerProductoPorTipoProducto(string CodSapTipoProducto);
        LineaProducto ObtenerCodSapPorTipoProducto(int IdTipoProducto);
        string ValidaReservaParqueadero(string CodigoBarrasBoletaControl);
        DescargueBoletaControl ObtenerProductosBoletaControl(string CodBarra,int usuario);
        DescargueBoletaControl ObtenerListaDescargue(string CodBarra);
        DescargueBoletaControl ObtenerProductoInstitucional(string CodBarra);
        DescargueBoletaControl ObtenerProductosInstitucional(string CodBarra1, string CodBarra2);
        string DescargarProductosInstitucional(ImprimirBoletaControl modelo);
        string DescargueBoletaFactura(List<Producto> listaProductos);
        IEnumerable<Producto> ObtenerTodosProductos();

        IEnumerable<Producto> ObtenerProductosXPuntoSurtido();

        
        IEnumerable<Producto> ObtenerTodosProductosRestaurante();
        
        RedencionBoletaControl RedencionBoletaControl(ImprimirBoletaControl modelo);
        FacturaImprimir ObtenerFacturaImprimir(int IdFactura);
        Factura ObtenerUltimaFactura(int IdPunto);
        FacturaImprimir ObtenerFacturaNotaCredImprimir(int IdFactura, int IdNotaCredito);
        ProductoBoleta BuscarBoleta(string CodBarraInicio, string CodBarraFinal, string Codproducto, int usuario);
        int GuardarLogVenta(LogVentaPunto modelo);

        EstructuraEmpleado ObtenerEmpleadoPorConsecutivo(string Consecutivo);

        // RDSH: Retorna los productos que se van a entregar desde destrezas.
        IEnumerable<Producto> ObtenerPremiosDestrezas();

        CortesiasEmpleado ObtenerCortesiaEmpleado(string documento);

        string GuardarCortesiaEmpleado(GuardarCortesiaEmpleado modelo);

        //RDSH: Valida si taquillero tiene una apertura activa.
        string ValidaPermiteAnularUsuario(int IdUsuario);
        /// <summary>
        /// EDSP Obtener pedidos y productos del día
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductosPedidos> ObtenerProductosPedidosDia();

        FacturaRespuesta RegistarCompraTienda(IEnumerable<ProductosTienda> productosTienda, List<PagoFacturaMediosPago> mediosPago, int IdUsuario, int idPunto);

        IEnumerable<Producto> ObtenerPasaportesActivos();

        IEnumerable<PuntoBrazalete> ObtenerPasaporteXPunto(int idPunto);

        IEnumerable<PuntoBrazalete> ObtenerPasaporteXPuntoPOS(int idpunto, int idUsuario);

        bool ActualizarPasaporteXPunto(List<PuntoBrazalete> puntoBrazalete);

        bool ActualizarArchivoBrazalete(ArchivoBrazalete archivoBrazalete);

        FacturaRespuesta RegistarReagendamientoTienda(IEnumerable<ProductosTienda> productosTienda, List<PagoFacturaMediosPago> mediosPago, int IdUsuario, int idPunto);

        IEnumerable<FacturaValidaUsoRespuesta> ValidarUsoFactura(string CodigoUsoFactura);

        string RegistrarCodigoBoleteria(int idProducto, double precio, int usuarioCreacion);

        string RegistroRolloImpresionLinea(Producto producto);
        IEnumerable<BrazaleteReimpresion> ObtenerBrazaleteReimpresion(int idPunto);

        int ControlBoleteria(int idBoleta);

        string ModificarControlBoleteria(int idBoleta, int NumBoletasRestantes);

        Producto ValidarImpresionEnLinea(int idBoleteria);

        IEnumerable<Producto> NumBoletasControlBoleteria();

        IEnumerable<Producto> VerPasaportesCodigoPedido(string codigoPedido);

        bool InsertarPlantillaBrazalete(PlantillaBrazalete plantilla);

        IEnumerable<PlantillaBrazalete> ObtenerPlantillas();
    }
}
