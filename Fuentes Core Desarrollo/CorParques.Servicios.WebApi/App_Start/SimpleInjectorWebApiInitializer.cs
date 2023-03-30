[assembly: WebActivator.PostApplicationStartMethod(typeof(CorParques.Servicios.WebApi.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace CorParques.Servicios.WebApi.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using Negocio.Contratos;
    using Negocio.Nucleo;
    using Datos.Contratos;
    using Datos.Dapper;
    using Transversales.Contratos;
    using Transversales.Util;
    using SignalR;
    using Corparques.Datos.Contratos;

    public static class SimpleInjectorWebApiInitializer
    {

        public static Container container = new Container();
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
        private static void InitializeContainer(Container container)
        {
            #region Negocio

            container.Register<IServicioGruposNotificacion, ServicioGruposNotificacion>(Lifestyle.Scoped);
            container.Register<IServicioUsuario, ServicioUsuario>(Lifestyle.Scoped);
            container.Register<IServicioParametros, ServicioParametros>(Lifestyle.Scoped);
            container.Register<IServicioNotificacion, ServicioNotificacion>(Lifestyle.Scoped);
            container.Register<IServicioPrioridadCorreo, ServicioPrioridadCorreo>(Lifestyle.Scoped);
            container.Register<IServicioPaciente, ServicioPaciente>(Lifestyle.Scoped);
            container.Register<IServicioProcedimiento, ServicioProcedimiento>(Lifestyle.Scoped);
            container.Register<IServicioListaPrecio, ServicioListaPrecio>(Lifestyle.Scoped);
            container.Register<IServicioTipoBrazalete, ServicioTipoBrazalete>(Lifestyle.Scoped);
            container.Register<IServicioPuntos, ServicioPuntos>(Lifestyle.Scoped);
            container.Register<IServicioCargueBrazalete, ServicioCargueBrazalete>(Lifestyle.Scoped);
            container.Register<IServicioEstado, ServicioEstado>(Lifestyle.Scoped);
            container.Register<IServicioGestionMantenimiento, ServicioGestionMantenimiento>(Lifestyle.Scoped);
            container.Register<IServicioGestionMantenimientoControl, ServicioGestionMantenimientoControl>(Lifestyle.Scoped);
            container.Register<IServicioGestionMantenimientoDetalle, ServicioGestionMantenimientoDetalle>(Lifestyle.Scoped);
            container.Register<IServicioTarifasParqueadero, ServicioTarifasParqueadero>(Lifestyle.Scoped);
            container.Register<IServicioTipoTarifaParqueadero, ServicioTipoTarifaParqueadero>(Lifestyle.Scoped);
            container.Register<IServicioControlParqueadero, ServicioControlParqueadero>(Lifestyle.Scoped);
            container.Register<IServicioUsuarioGrupo, ServicioUsuarioGrupo>(Lifestyle.Scoped);
            container.Register<IServicioCentroMedico, ServicioCentroMedico>(Lifestyle.Scoped);
            container.Register<IServicioTipoVehiculoPorParqueadero, ServicioTipoVehiculoPorParqueadero>(Lifestyle.Scoped);
            container.Register<IServicioTipoVehiculo, ServicioTipoVehiculo>(Lifestyle.Scoped);
            container.Register<IServicioPerfil, ServicioPerfil>(Lifestyle.Scoped);
            container.Register<IServicioTipoConvenioParqueadero, ServicioTipoConvenioParqueadero>(Lifestyle.Scoped);
            container.Register<IServicioConvenioParqueadero, ServicioConvenioParqueadero>(Lifestyle.Scoped);
            container.Register<IServicioTipoDocumento, ServicioTipoDocumento>(Lifestyle.Scoped);
            container.Register<IServicioTipoPuntos, ServicioTipoPuntos>(Lifestyle.Scoped);
            container.Register<IServicioMediosPago, ServicioMediosPago>(Lifestyle.Scoped);
            container.Register<IServicioMenu, ServicioMenu>(Lifestyle.Scoped);
            container.Register<IServicioTipoCliente, ServicioTipoCliente>(Lifestyle.Scoped);
            container.Register<IServicioPos, ServicioPos>(Lifestyle.Scoped);
            container.Register<IServicioCortesiaPunto, ServicioCortesiaPunto>(Lifestyle.Scoped);
            container.Register<IServicioCortesiaDestreza, ServicioCortesiaDestreza>(Lifestyle.Scoped);
            container.Register<IServicioPasaporteUso, ServicioPasaporteUso>(Lifestyle.Scoped);
            container.Register<IServicioReservaSkycoaster, ServicioReservaSkycoaster>(Lifestyle.Scoped);
            container.Register<IServicioTipoDenominacion, ServicioTipoDenominacion>(Lifestyle.Scoped);
            container.Register<IServicioRecoleccion, ServicioRecoleccion>(Lifestyle.Scoped);
            container.Register<IServicioConvenioSAP, ServicioConvenioSAP>(Lifestyle.Scoped);
            container.Register<IServicioApertura, ServicioApertura>(Lifestyle.Scoped);
            container.Register<IServicioAperturaBase, ServicioAperturaBase>(Lifestyle.Scoped);
            container.Register<IServicioArea, ServicioArea>(Lifestyle.Scoped);
            container.Register<IServicioRecoleccionSupervisor, ServicioRecoleccionSupervisor>(Lifestyle.Scoped);
            container.Register<IServicioNotaCredito, ServicioNotaCredito>(Lifestyle.Scoped);
            container.Register<IServicioObservacionRecoleccion, ServicioObservacionRecoleccion>(Lifestyle.Scoped);
            container.Register<IServicioArqueo, ServicioArqueo>(Lifestyle.Scoped);
            container.Register<IServicioCierrePuntos, ServicioCierrePunto>(Lifestyle.Scoped);
            container.Register<IServicioRecambio, ServicioRecambio>(Lifestyle.Scoped);
            container.Register<IServicioFranquicia, ServicioFranquicia>(Lifestyle.Scoped);
            container.Register<IServicioReimpresion, ServicioReimpresion>(Lifestyle.Scoped);
            container.Register<IServicioAlistamientoPendiente, ServicioAlistamientoPendiente>(Lifestyle.Scoped);
            container.Register<IServicioBitacoraDia, ServicioBitacoraDia>(Lifestyle.Scoped);
            container.Register<IServicioClima, ServicioClima>(Lifestyle.Scoped);
            container.Register<IServicioSegmentoDia, ServicioSegmentoDia>(Lifestyle.Scoped);
            container.Register<IServicioAuxiliarPunto, ServicioAuxiliarPunto>(Lifestyle.Scoped);
            container.Register<IServicioUbicacionPunto, ServicioUbicacionPunto>(Lifestyle.Scoped);
            container.Register<IServicioSerieCodBarra, ServicioSerieCodBarra>(Lifestyle.Scoped);
            container.Register<IServicioBoleteria, ServicioBoleteria>(Lifestyle.Scoped);
            container.Register<IServicioCargueBoleteria, ServicioCargueBoleteria>(Lifestyle.Scoped);
            container.Register<IServicioCodigodeBarras, ServicioCodigodeBarras>(Lifestyle.Scoped);
            container.Register<IServicioInventario, ServicioInventario>(Lifestyle.Scoped);
            container.Register<IServicioMateriales, ServicioMateriales>(Lifestyle.Scoped);
            container.Register<IServicioRegistroTorniquete, ServicioRegistroTorniquete>(Lifestyle.Scoped);
            container.Register<IServicioCentroImpresion, ServicioCentroImpresion>(Lifestyle.Scoped);
            container.Register<IServicioOrdenMantenimiento, ServicioOrdenMantenimiento>(Lifestyle.Scoped);
            container.Register<IServicioOperacion, ServicioOperacion>(Lifestyle.Scoped);
            container.Register<IServicioRetorno, ServicioRetorno>(Lifestyle.Scoped);
            container.Register<IServicioHistoricoBoleta, ServicioHistoricoBoleta>(Lifestyle.Scoped);
            container.Register<IServicioReservaEspacios, ServicioReservaEspacios>(Lifestyle.Scoped);
            container.Register<IServicioPlaneacion, ServicioPlaneacion>(Lifestyle.Scoped);
            container.Register<IServicioRegistroFallas, ServicioRegistroFallas>(Lifestyle.Scoped);
            container.Register<IServicioDashBoard, ServicioDashBoard>(Lifestyle.Scoped);
            container.Register<IServicioReporteVentasDirectas, ServicioReporteVentasDirectas>(Lifestyle.Scoped);
            container.Register<IServicioReporteFallaAtraccion, ServicioReporteFallaAtraccion>(Lifestyle.Scoped);
            container.Register<IServicioOrden, ServicioOrden>(Lifestyle.Scoped);
            container.Register<IServicioReporteRedFechaAbierta, ServicioReporteRedFechaAbierta>(Lifestyle.Scoped);
            container.Register<IServicioReporteCostoProducto, ServicioReporteCostoProducto>(Lifestyle.Scoped);
            container.Register<IServicioReportePasajerosAtracciones, ServicioReportePasajerosAtracciones>(Lifestyle.Scoped);
            container.Register<IServicioReporteVentasPorHora, ServicioReporteVentasPorHora>(Lifestyle.Scoped);
            container.Register<IServicioReporteVentasPorProducto, ServicioReporteVentasPorProducto>(Lifestyle.Scoped);
            container.Register<IServicioReporteDestreza, ServicioReporteDestreza>(Lifestyle.Scoped);
            container.Register<IServicioAdicionPedidos, ServicioAdicionPedidos>(Lifestyle.Scoped);
            container.Register<IServicioReporteRecaudosVentas, ServicioReporteRecaudosVentas>(Lifestyle.Scoped);
            container.Register<IServicioReporteInventarioGeneral, ServicioReporteInventarioGeneral>(Lifestyle.Scoped);
            container.Register<IServicioReporteCuadreDiarioFlujoCajasTaq, ServicioReporteCuadreDiarioFlujoCajasTaq>(Lifestyle.Scoped);
            container.Register<IServicioReporteControlCaja, ServicioReporteControlCaja>(Lifestyle.Scoped);
            container.Register<IServicioReporteMovimientoInventario, ServicioReporteMovientoInventario>(Lifestyle.Scoped);
            container.Register<IServicioReporteReservaEspacio, ServicioReporteReservaEspacio>(Lifestyle.Scoped);
            container.Register<IServicioReporteAprovechamientoFA, ServicioReporteAprovechamientoFA>(Lifestyle.Scoped);
            container.Register<IServicioReporteFANVendidas, ServicioReporteFANVendidas>(Lifestyle.Scoped);
            container.Register<IServicioReporteCortesias, ServicioReporteCortesias>(Lifestyle.Scoped);

            container.Register<IServicioConvenio, ServicioConvenio>(Lifestyle.Scoped);
            container.Register<IServicioTipoProducto, ServicioTipoProducto>(Lifestyle.Scoped);
            container.Register<IServicioReporteNotificaciones, ServicioReporteNotificaciones>(Lifestyle.Scoped);
            container.Register<IServicioCliente, ServicioCliente>(Lifestyle.Scoped);
            container.Register<IServicioFactura, ServicioFactura>(Lifestyle.Scoped);
            container.Register<IServicioReporteVentasDiarias, ServicioReporteVentasDiarias>(Lifestyle.Scoped);
            container.Register<IServicioReporteDonaciones, ServicioReporteDonaciones>(Lifestyle.Scoped);
            container.Register<IServicioMatrizPuntos, ServicioMatrizPuntos>(Lifestyle.Scoped);
            container.Register<IServicioTarjetaRecargable, ServicioTarjetaRecargable>(Lifestyle.Scoped);
            container.Register<IServicioFidelizacion, ServicioFidelizacion>(Lifestyle.Scoped);
            container.Register<IServicioReporteTarjetaRecargable, ServicioReporteTarjetaRecargable>(Lifestyle.Scoped);
            container.Register<IServicioProductosApp, ServicioProductosApp>(Lifestyle.Scoped);
            container.Register<IServicioRedeban, ServicioRedeban>(Lifestyle.Scoped);
            container.Register<IServicioResolucionFactura, ServicioResolucionFactura>(Lifestyle.Scoped);

            container.Register<IServicioCodigoFechaAbierta, ServicioCodigoFechaAbierta>(Lifestyle.Scoped);
            container.Register<IServicioConsumoDeEmpleados, ServicioConsumoDeEmpleados>(Lifestyle.Scoped);

            container.Register<IServicioCargos, ServicioCargos>(Lifestyle.Scoped);
            container.Register<IServicioCortesia, ServicioCortesia>(Lifestyle.Scoped);
            container.Register<IServicioReporteRecoleccion, ServicioReporteRecoleccion>(Lifestyle.Scoped);
            container.Register<IServicioReporteSoporteIT, ServicioReporteSoporteIT>(Lifestyle.Scoped);
            container.Register<IServicioReporteUsuarioApp, ServicioReporteUsuarioApp>(Lifestyle.Scoped);

            container.Register<IServicioVuycoom, ServicioVuycoom>(Lifestyle.Scoped);

            container.Register<IServicioReporte_SI_Consumo_SF_FechaAbierta, ServicioReporte_SI_Consumo_SF_FechaAbierta>(Lifestyle.Scoped);

            container.Register<IServicioAlmacen, ServicioAlmacen>(Lifestyle.Scoped);
            container.Register<IServicioReimpresionReciboCajaV, ServicioReimpresionReciboCajaV>(Lifestyle.Scoped);
            container.Register<IServicioReagendamiento, ServicioReagendamiento>(Lifestyle.Scoped);

            container.Register<IServicioReporteVentasPorConvenio, ServicioReporteVentasPorConvenio>(Lifestyle.Scoped);


            #endregion

            #region Datos

            container.Register(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            container.Register<IRepositorioGrupoNotificacion, RepositorioGruposNotificacion>(Lifestyle.Scoped);
            container.Register<IRepositorioUsuario, RepositorioUsuario>(Lifestyle.Scoped);
            container.Register<IRepositorioPlazoCortesia, RepositorioPlazoCortesia>(Lifestyle.Scoped);
            container.Register<IRepositorioComplejidadCortesia, RepositorioComplejidadCortesia>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoRedencionCortesia, RepositorioTipoRedencionCortesia>(Lifestyle.Scoped);
            container.Register<IRepositorioParametros, RepositorioParametros>(Lifestyle.Scoped);
            container.Register<IRepositorioNotificacion, RepositorioNotificacion>(Lifestyle.Scoped);
            container.Register<IRepositorioPrioridadCorreo, RepositorioPrioridadCorreo>(Lifestyle.Scoped);
            container.Register<IRepositorioPaciente, RepositorioPaciente>(Lifestyle.Scoped);
            container.Register<IRepositorioProcedimiento, RepositorioProcedimiento>(Lifestyle.Scoped);
            container.Register<IRepositorioTiposVehiculos, RepositorioTiposVehiculos>(Lifestyle.Scoped);
            container.Register<IRepositorioListaPrecio, RepositorioListaPrecio>(Lifestyle.Scoped);
            container.Register<IRepositorioPuntos, RepositorioPuntos>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoBrazalete, RepositorioTipoBrazalete>(Lifestyle.Scoped);
            container.Register<IRepositorioCargueBrazalete, RepositorioCargueBrazalete>(Lifestyle.Scoped);
            container.Register<IRepositorioEstados, RepositorioEstado>(Lifestyle.Scoped);
            container.Register<IRepositorioGestionMantenimiento, RepositorioGestionMantenimiento>(Lifestyle.Scoped);
            container.Register<IRepositorioGestionMantenimientoControl, RepositorioGestionMantenimientoControl>(Lifestyle.Scoped);
            container.Register<IRepositorioGestionMantenimientoDetalle, RepositorioGestionMantenimientoDetalle>(Lifestyle.Scoped);
            container.Register<IRepositorioTarifasParqueadero, RepositorioTarifasParqueadero>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoTarifaParqueadero, RepositorioTipoTarifaParqueadero>(Lifestyle.Scoped);
            container.Register<IRepositorioControlParqueadero, RepositorioControlParqueadero>(Lifestyle.Scoped);
            container.Register<IRepositorioUsuarioGrupo, RepositorioUsuarioGrupo>(Lifestyle.Scoped);
            container.Register<IRepositorioCentroMedico, RepositorioCentroMedico>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoVehiculoPorParqueadero, RepositorioTipoVehiculoPorParqueadero>(Lifestyle.Scoped);
            container.Register<IRepositorioPerfil, RepositorioPerfil>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoConvenioParqueadero, RepositorioTipoConvenioParqueadero>(Lifestyle.Scoped);
            container.Register<IRepositorioConvenioParqueadero, RepositorioConvenioParqueadero>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoDocumento, RepositorioTipoDocumento>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoPuntos, RepositorioTipoPuntos>(Lifestyle.Scoped);
            container.Register<IRepositorioMenu, RepositorioMenu>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoPaciente, RepositorioTipoPaciente>(Lifestyle.Scoped);
            container.Register<IRepositorioCategoriaAtencion, RepositorioCategoriaAtencion>(Lifestyle.Scoped);
            container.Register<IRepositorioMediosPagos, RepositorioMediosPago>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoCliente, RepositorioTipoCliente>(Lifestyle.Scoped);
            container.Register<IRepositorioProducto, RepositorioProducto>(Lifestyle.Scoped);
            container.Register<IRepositorioCortesiaPunto, RepositorioCortesiaPunto>(Lifestyle.Scoped);
            container.Register<IRepositorioCortesiaDestreza, RepositorioCortesiaDestreza>(Lifestyle.Scoped);
            container.Register<IRepositorioPasaporteUso, RepositorioPasaporteUso>(Lifestyle.Scoped);
            container.Register<IRepositorioReservaSkycoaster, RepositorioReservaSkycoaster>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoDenominacion, RepositorioTipoDenominacion>(Lifestyle.Scoped);
            container.Register<IRepositorioRecoleccion, RepositorioRecoleccion>(Lifestyle.Scoped);
            container.Register<IRepositorioDetalleRecoleccionMonetaria, RepositorioDetalleRecoleccionMonetaria>(Lifestyle.Scoped);
            container.Register<IRepositorioDetalleRecoleccionDocumento, RepositorioDetalleRecoleccionDocumento>(Lifestyle.Scoped);
            container.Register<IRepositorioConvenioSAP, RepositorioConvenioSAP>(Lifestyle.Scoped);
            container.Register<IRepositorioApertura, RepositorioApertura>(Lifestyle.Scoped);
            container.Register<IRepositorioAperturaBase, RepositorioAperturaBase>(Lifestyle.Scoped);
            container.Register<IRepositorioArea, RepositorioArea>(Lifestyle.Scoped);
            container.Register<IRepositorioAutorizacionVehiculo, RepositorioAutorizacionVehiculo>(Lifestyle.Scoped);
            container.Register<IRepositorioAperturaBrazalete, RepositorioAperturaBrazalete>(Lifestyle.Scoped);
            container.Register<IRepositorioRecoleccionSupervisor, RepositorioRecoleccionSupervisor>(Lifestyle.Scoped);
            container.Register<IRepositorioDetalleRecoleccionBrazalete, RepositorioDetalleRecoleccionBrazalete>(Lifestyle.Scoped);
            container.Register<IRepositorioNotaCredito, RepositorioNotaCredito>(Lifestyle.Scoped);
            container.Register<IRepositorioObservacionRecoleccion, RepositorioObservacionRecoleccion>(Lifestyle.Scoped);
            container.Register<IRepositorioFactura, RepositorioFactura>(Lifestyle.Scoped);
            container.Register<IRepositorioArqueo, RepositorioArqueo>(Lifestyle.Scoped);
            container.Register<IRepositorioCierrePunto, RepositorioCierrePunto>(Lifestyle.Scoped);
            container.Register<IRepositorioRecambio, RepositorioRecambio>(Lifestyle.Scoped);
            container.Register<IRepositorioFranquicia, RepositorioFranquicia>(Lifestyle.Scoped);
            container.Register<IRepositorioReimpresion, RepositorioReimpresion>(Lifestyle.Scoped);
            container.Register<IRepositorioBitacoraDia, RepositorioBitacoraDia>(Lifestyle.Scoped);
            container.Register<IRepositorioAlistamientoPendiente, RepositorioAlistamientoPendiente>(Lifestyle.Scoped);
            container.Register<IRepositorioClima, RepositorioClima>(Lifestyle.Scoped);
            container.Register<IRepositorioSegmentoDia, RepositorioSegmentoDia>(Lifestyle.Scoped);
            container.Register<IRepositorioDetalleRecoleccionNovedad, RepositorioDetalleRecoleccionNovedad>(Lifestyle.Scoped);
            container.Register<IRepositorioAuxiliarPunto, RepositorioAuxiliarPunto>(Lifestyle.Scoped);
            container.Register<IRepositorioUbicacionPunto, RepositorioUbicacionPunto>(Lifestyle.Scoped);
            container.Register<IRepositorioInventario, RepositorioInventario>(Lifestyle.Scoped);
            container.Register<IRepositorioMateriales, RepositorioMateriales>(Lifestyle.Scoped);
            container.Register<IRepositorioSerieCodBarra, RepositorioSerieCodBarra>(Lifestyle.Scoped);
            container.Register<IRepositorioBoleteria, RepositorioBoleteria>(Lifestyle.Scoped);
            container.Register<IRepositorioCargueBoleteria, RepositorioCargueBoleteria>(Lifestyle.Scoped);
            container.Register<IRepositorioCodigodeBarras, RepositorioCodigodeBarras>(Lifestyle.Scoped);
            container.Register<IRepositorioRegistroTorniquete, RepositorioRegistroTorniquete>(Lifestyle.Scoped);
            container.Register<IRepositorioCentroImpresion, RepositorioCentroImpresion>(Lifestyle.Scoped);
            container.Register<IRepositorioOrdenMantenimiento, RepositorioOrdenMantenimiento>(Lifestyle.Scoped);
            container.Register<IRepositorioOperacion, RepositorioOperacion>(Lifestyle.Scoped);
            container.Register<IRepositorioRetorno, RepositorioRetorno>(Lifestyle.Scoped);
            container.Register<IRepositorioHistoricoBoleta, RepositorioHistoricoBoleta>(Lifestyle.Scoped);
            container.Register<IRepositorioReservaEspacios, RepositorioReservaEspacios>(Lifestyle.Scoped);
            container.Register<IRepositorioPlaneacion, RepositorioPlaneacion>(Lifestyle.Scoped);
            container.Register<IRepositorioRegistroFallas, RepositorioRegistroFallas>(Lifestyle.Scoped);
            container.Register<IRepositorioDashBoard, RepositorioDashBoard>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteVentasDirectas, RepositorioReporteVentasDirectas>(Lifestyle.Scoped);
            container.Register<IRepositorioEstructuraEmpleado, RepositorioEstructuraEmpleado>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteFallaAtraccion, RepositorioReporteFallaAtraccion>(Lifestyle.Scoped);
            container.Register<IRepositorioOrden, RepositorioOrden>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteRedFechaAbierta, RepositorioReporteRedFechaAbierta>(Lifestyle.Scoped);
            container.Register<IRepositorioReportePasajerosAtracciones, RepositorioReportePasajerosAtracciones>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteCostoProducto, RepositorioReporteCostoProducto>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteVentasPorHora, RepositorioReporteVentasPorHora>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteVentasPorProducto, RepositorioReporteVentasPorProducto>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteDestreza, RepositorioReporteDestreza>(Lifestyle.Scoped);
            container.Register<IRepositorioAdicionPedidos, RepositorioAdicionPedidos>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteRecaudosVentas, RepositorioReporteRecaudosVentas>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteInventarioGeneral, RepositorioReporteInventarioGeneral>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteCuadreDiarioFlujoCajasTaq, RepositorioReporteCuadreDiarioFlujoCajasTaq>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteControlCaja, RepositorioReporteControlCaja>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteMovimientoInventario, RepositorioReporteMovimientoInventario>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteReservaEspacio, RepositorioReporteReservaEspacio>(Lifestyle.Scoped);
            container.Register<IRepositorioConvenio, RepositorioConvenio>(Lifestyle.Scoped);
            container.Register<IRepositorioTipoProducto, RepositorioTipoProducto>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteNotificaciones, RepositorioReporteNotificaciones>(Lifestyle.Scoped);
            container.Register<IRepositorioCliente, RepositorioCliente>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteAprovechamientoFA, RepositorioReporteAprovechamientoFA>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteFANVendidas, RepositorioReporteFANVendidas>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteCortesias, RepositorioReporteCortesias>(Lifestyle.Scoped);

            container.Register<IRepositorioCodigoFechaAbierta, RepositorioCodigoFechaAbierta>(Lifestyle.Scoped);
            container.Register<IRepositorioConsumoDeEmpleado, RepositorioConsumoDeEmpleado>(Lifestyle.Scoped);

            container.Register<IRepositorioReporteVentasDiarias, RepositorioReporteVentasDiarias>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteDonaciones, RepositorioReporteDonaciones>(Lifestyle.Scoped);
            container.Register<IRepositorioMatrizPuntos, RepositorioMatrizPuntos>(Lifestyle.Scoped);
            container.Register<IRepositorioTarjetaRecargable, RepositorioTarjetaRecargable>(Lifestyle.Scoped);
            container.Register<IRepositorioFidelizacion, RepositorioFidelizacion>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteTarjetaRecargable, RepositorioReporteTarjetaRecargable>(Lifestyle.Scoped);
            container.Register<IRepositorioProductosApp, RepositorioProductosApp>(Lifestyle.Scoped);
            container.Register<IRepositorioLogRedebanRespuesta, RepositorioLogRedebanRespuesta>(Lifestyle.Scoped);
            container.Register<IRepositorioLogRedebanSolicitud, RepositorioLogRedebanSolicitud>(Lifestyle.Scoped);
            container.Register<IRepositorioResolucionFactura, RepositorioResolucionFactura>(Lifestyle.Scoped);
            container.Register<IRepositorioCargos, RepositorioCargos>(Lifestyle.Scoped);
            container.Register<IRepositorioLogRedebanSolicitudAnulacion, RepositorioLogRedebanSolicitudAnulacion>(Lifestyle.Scoped);
            container.Register<IRepositorioCortesia, RepositorioCortesia>(Lifestyle.Scoped);
            container.Register<IRepositorioUsuarioVisitante, RepositorioUsuarioVisitante>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteRecoleccion, RepositorioReporteRecoleccion>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteSoporteIT, RepositorioReporteSoporteIT>(Lifestyle.Scoped);
            container.Register<IRepositorioReporteUsuarioApp, RepositorioReporteUsuarioApp>(Lifestyle.Scoped);

            container.Register<IRepositorioVuycoom, RepositorioVuycoom>(Lifestyle.Scoped);
            container.Register<IRepositorioReporte_SI_Consumo_SF_FechaAbierta, RepositorioReporte_SI_Consumo_SF_FechaAbierta>(Lifestyle.Scoped);

            container.Register<IRepositorioAlmacen, RepositorioAlmacen>(Lifestyle.Scoped);
            container.Register<IRepositorioReimpresionReciboCajaV, RepositorioReimpresionReciboCajaV>(Lifestyle.Scoped);
            container.Register<IRepositorioReagendamiento, RepositorioReagendamiento>(Lifestyle.Scoped);

            container.Register<IRepositorioReporteVentasPorConvenio, RepositorioReporteVentasPorConvenio>(Lifestyle.Scoped);

            #endregion

            #region Transversales

            container.Register<IEnvioMails, EnvioMails>(Lifestyle.Scoped);

            #endregion
        }
    }
}