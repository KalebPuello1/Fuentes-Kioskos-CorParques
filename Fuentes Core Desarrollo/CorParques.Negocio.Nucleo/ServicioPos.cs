using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Contratos;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioPos : IServicioPos
    {
        IRepositorioProducto _repositorio;
        IRepositorioControlParqueadero _repositorioParqueadero;
        IRepositorioFactura _repositorioFactura;
        IRepositorioApertura _repositorioApertura;
        IRepositorioParametros _repositorioParametro;
        IRepositorioConvenioSAP _repositorioConvenioSAP;
        IRepositorioEstructuraEmpleado _repositorioEstructuraEmpleado;
        IRepositorioFidelizacion _repositorioFidelizacion;
        IRepositorioBoleteria _repositorioBoleteria;
        IRepositorioRecoleccion _repositorioRecoleccion;
        IRepositorioTarjetaRecargable _repositorioTarjetaRecargable;
        IEnvioMails _mails;

        public ServicioPos(IRepositorioProducto repositorio, IRepositorioControlParqueadero repositorioParqueadero, IRepositorioFactura repositorioFactura,
                           IRepositorioApertura repositorioApertura, IRepositorioParametros repositorioParametro, IRepositorioConvenioSAP repositorioConvenioSAP,
                           IRepositorioEstructuraEmpleado repositorioEstructuraEmpleado, IEnvioMails mails, IRepositorioFidelizacion repositorioFidelizacion,
                           IRepositorioBoleteria repositorioBoleteria, IRepositorioRecoleccion repositorioRecoleccion,
                           IRepositorioTarjetaRecargable repositorioTarjetaRecargable)
        {
            _repositorio = repositorio;
            _repositorioParqueadero = repositorioParqueadero;
            _repositorioFactura = repositorioFactura;
            _repositorioApertura = repositorioApertura;
            _repositorioParametro = repositorioParametro;
            _repositorioConvenioSAP = repositorioConvenioSAP;
            _repositorioEstructuraEmpleado = repositorioEstructuraEmpleado;
            _repositorioFidelizacion = repositorioFidelizacion;
            _repositorioBoleteria = repositorioBoleteria;
            _repositorioRecoleccion = repositorioRecoleccion;
            _repositorioTarjetaRecargable = repositorioTarjetaRecargable;
            _mails = mails;
        }

        public string InsertarPedidoR(PagoFactura modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                retValue = _repositorio.InsertarPedidoR(modelo);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return retValue;
        }
        public string AgregarProducAraza(PagoFactura modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                retValue = _repositorio.AgregarProducAraza(modelo);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return retValue;
        }
        public string ActualizarTipoAcompaRestaurante(TipoAcompanamiento modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                retValue = _repositorio.ActualizarTipoAcompaRestaurante(modelo);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return retValue;
        }
        public string GuardarAcompaProduAdmin(ProductoxAcompanamiento modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                retValue = _repositorio.GuardarAcompaProduAdmin(modelo);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return retValue;
        }
        public string ActualizarProductoAdminRestaurante(Producto modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                retValue = _repositorio.ActualizarProductoAdminRestaurante(modelo);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return retValue;
        }

        public string UpdatePedidoRestFactura(PagoFactura modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                retValue = _repositorio.UpdatePedidoRestFactura(modelo);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return retValue;
        }


        public string InsertarCompra(PagoFactura modelo, ref string Error)
        {
            string retValue = string.Empty;
            //string rtarecarga = string.Empty;
            string rtadonacion = string.Empty;
            try
            {
                var totaldinero = modelo.listMediosPago != null ? modelo.listMediosPago.Sum(x => x.Valor) : 0;
                var totalventa = modelo.listaProducto.Sum(x => x.Precio);
                var cambiofinal = totaldinero - totalventa;

                if (modelo.listMediosPago == null)
                    modelo.listMediosPago = new List<PagoFacturaMediosPago>();

                //var recarga = _repositorioParametro.ObtenerParametroPorNombre("CodSapRecargaTarjeta").Valor;
                var donacion = _repositorio.ObtenerProductosDonacion().Select(x => x.CodSAP).ToList();

                //en la lista viene una recarga de tarjeta recargable y otros productos
                //if (modelo.listaProducto.Exists(x => x.CodigoSap.Equals(recarga)) && modelo.listaProducto.Exists(x => !x.CodigoSap.Equals(recarga)))
                //{
                //    //en este caso se genera soporte de tarjeta recargable
                //    PagoFactura modelorecarga =  new PagoFactura { CodSapConvenio = modelo.CodSapConvenio, ConsecutivoConvenio = modelo.ConsecutivoConvenio, Donante = modelo.Donante, EsContingencia = modelo.EsContingencia, IdConvenio = modelo.IdConvenio, IdPunto = modelo.IdPunto, IdUsuario = modelo.IdUsuario, listaProducto = modelo.listaProducto, listMediosPago = modelo.listMediosPago } ;
                //    modelorecarga.Donante = null;
                //    modelorecarga.listaProducto = modelorecarga.listaProducto.Where(x => x.CodigoSap.Equals(recarga)).ToList();

                //    modelorecarga.listaProducto.ForEach(x => modelo.listaProducto.Remove(x));
                //    rtarecarga = _repositorio.InsertarFactura(modelorecarga);
                //    if (!string.IsNullOrEmpty(rtarecarga))
                //    {
                //        if (modelorecarga.IdConvenio > 0)
                //        {
                //            if (string.IsNullOrEmpty(modelorecarga.ConsecutivoConvenio))
                //                modelorecarga.ConsecutivoConvenio = null;

                //            _repositorioFactura.ActualizarFacturaConvenio(rtarecarga, modelorecarga.IdConvenio, modelorecarga.ConsecutivoConvenio);

                //        }
                //    }
                //}
                //se genera factura excluyendo la donacion
                var donacionCambio = true;
                var donanteNum = modelo.Donante;
                var productos = modelo.listaProducto;
                if (productos.Exists(x => !donacion.Contains(x.CodigoSap)))
                {
                    modelo.Donante = null;
                    modelo.listaProducto = productos.Where(x => !donacion.Contains(x.CodigoSap)).ToList();

                    totaldinero = modelo.listMediosPago != null ? modelo.listMediosPago.Sum(x => x.Valor) : 0;
                    totalventa = modelo.listaProducto.Sum(x => x.Precio);
                    modelo.Cambio = totaldinero - totalventa;
                    retValue = _repositorio.InsertarFactura(modelo);



                    if (!string.IsNullOrEmpty(retValue))
                    {
                        foreach (var item in productos)
                        {
                            var msj = _repositorio.ValidarAlerta(modelo.IdUsuario, item.IdProducto);
                            if (!string.IsNullOrEmpty(msj))
                            {
                                var body = $"El taquillero {msj.Split('|')[0]} cuenta con {msj.Split('|')[2]} unidades del material {msj.Split('|')[1]}";
                                var correo = _repositorioParametro.ObtenerParametroPorNombre("CorreoTrasladosBoleteria").Valor;
                                _mails.EnviarCorreo(correo, "Alerta reabastecimiento", body, System.Net.Mail.MailPriority.Normal, new List<string>());
                            }
                        }
                        donacionCambio = false;
                        var _tarjetaRecargable = _repositorioParametro.ObtenerParametroPorNombre("CodSapTarjetaRecargable").Valor;
                        var _prodfan = _repositorioParametro.ObtenerParametroPorNombre("CodigoSapProductoClienteFan").Valor;
                        var prodf = modelo.listaProducto.Where(x => x.CodigoSap == _prodfan).ToList();
                        var prod = modelo.listaProducto.Where(x => x.CodigoSap == _tarjetaRecargable).ToList();
                        //enviar correo sin adjunto
                        if (!modelo.listaProducto.Exists(x => x.Nombre.ToLower().Contains("reposic")))
                        {
                            if (prod.Count > 0)
                            {

                                foreach (var item in prod)
                                {
                                    if (prodf.Count > 0)
                                    {
                                        var cliente = _repositorioFidelizacion.ObtenerClienteTarjeta(item.DataExtension.Split('|')[0].Substring(1, item.DataExtension.Split('|')[0].Length - 1));
                                        cliente.Foto = Convert.FromBase64String(item.DataExtension.Split('|')[7].Substring(1, item.DataExtension.Split('|')[7].Length - 1));
                                        _repositorioFidelizacion.Actualizar(ref cliente);
                                        _mails.correoFidelizacion(null, item.DataExtension.Split('|')[1].Substring(1, item.DataExtension.Split('|')[1].Length - 1), item.DataExtension.Split('|')[2].Substring(1, item.DataExtension.Split('|')[2].Length - 1), string.Empty, new List<string> { $"{System.AppDomain.CurrentDomain.BaseDirectory}\\AdjuntosCorreo\\contrato.pdf", $"{System.AppDomain.CurrentDomain.BaseDirectory}\\AdjuntosCorreo\\politicas.pdf" });

                                    }
                                    else
                                    {
                                        _mails.correoFidelizacion(null, item.DataExtension.Split('|')[1].Substring(1, item.DataExtension.Split('|')[1].Length - 1), item.DataExtension.Split('|')[2].Substring(1, item.DataExtension.Split('|')[2].Length - 1), string.Empty, new List<string> { $"{System.AppDomain.CurrentDomain.BaseDirectory}\\AdjuntosCorreo\\politicas.pdf" });
                                    }
                                }
                            }
                        }
                        //Se retira busqueda por id parqueadero 
                        var _parametro = _repositorioParametro.ObtenerParametroPorNombre("CodSapProdParqueadero");

                        //Actualiza informacion de parqueadero
                        foreach (var item in modelo.listaProducto.Where(l => l.CodigoSap == _parametro.Valor))
                        {
                            int iIdControl = 0;
                            if (int.TryParse(item.IdDetalleProducto.ToString(), out iIdControl))
                            {
                                ControlParqueadero objPark = _repositorioParqueadero.Obtener(iIdControl);
                                if (objPark != null)
                                {
                                    objPark.IdEstado = 2;
                                    _repositorioParqueadero.Actualizar(ref objPark);
                                }
                            }
                        }

                        //Actualiza informacion de convenio
                        if (modelo.IdConvenio > 0)
                        {
                            if (string.IsNullOrEmpty(modelo.ConsecutivoConvenio))
                                modelo.ConsecutivoConvenio = null;

                            _repositorioFactura.ActualizarFacturaConvenio(retValue, modelo.IdConvenio, modelo.ConsecutivoConvenio);

                        }
                    }

                }

                //en la lista viene una donacion
                if (productos.Exists(x => donacion.Contains(x.CodigoSap)))
                {
                    var tipos = new List<int>() { 1 };
                    var medios = new List<PagoFacturaMediosPago>();
                    double pago = 0;
                    if (modelo.listMediosPago != null)
                    {
                        medios.Add(modelo.listMediosPago.First(x => tipos.Contains(x.IdMedioPago)));
                        medios[0].Valor = (!donacionCambio ? modelo.Cambio : medios[0].Valor);
                        pago = modelo.Cambio;
                    }
                    //en este caso se genera soporte de donacion
                    var modelodonacion = new PagoFactura
                    {
                        CodSapConvenio = modelo.CodSapConvenio,
                        ConsecutivoConvenio = modelo.ConsecutivoConvenio,
                        Donante = donanteNum,
                        EsContingencia = modelo.EsContingencia,
                        IdConvenio = modelo.IdConvenio,
                        IdPunto = modelo.IdPunto,
                        IdUsuario = modelo.IdUsuario,
                        listaProducto = modelo.listaProducto,
                        listMediosPago = medios,
                        Cambio = modelo.Cambio
                    };

                    modelodonacion.listaProducto = productos.Where(x => donacion.Contains(x.CodigoSap)).ToList();
                    modelodonacion.Cambio = cambiofinal;
                    //modelodonacion.listaProducto.ForEach(x => modelo.listaProducto.Remove(x));
                    rtadonacion = _repositorio.InsertarFactura(modelodonacion);
                }


                retValue += (!string.IsNullOrEmpty(retValue) ? "|" : "");
                //retValue += (!string.IsNullOrEmpty(rtarecarga) ? rtarecarga : "");
                //retValue += (!string.IsNullOrEmpty(retValue) ? (!retValue.EndsWith("|")?"|":"") : "");
                retValue += (!string.IsNullOrEmpty(rtadonacion) ? rtadonacion : "");

                retValue = (retValue.EndsWith("|") ? retValue.Substring(0, retValue.Length - 1) : retValue);
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("error:"))
                {
                    var id = ex.Message.IndexOf("error:");
                    Error = ex.Message.Substring(id + 6, ex.Message.Length - id - 6);
                    retValue = Error;
                }
                else
                {
                    Error = ex.Message;
                    retValue = Error;
                }
            }

            return retValue;
        }

        /// <summary>
        /// Metodo publico que donde se centralizan las validaciones antes de crear la factura
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public string ValidarCompra(PagoFactura modelo)
        {
            string retValue = "";

            try
            {
                /* Cuando no hay pagos el objeto llega null */
                if (modelo.listMediosPago == null)
                    modelo.listMediosPago = new List<PagoFacturaMediosPago>();

                retValue = ValidaFacturaConvenio(modelo);

                if (string.IsNullOrEmpty(retValue))
                    retValue = ValidaCompraNomina(modelo);
            }
            catch (Exception ex)
            {
                retValue = ex.Message;
            }

            return retValue;
        }

        /// <summary>
        /// Metodo Privado que se encarga de validar previamente una factura los productos de convenio
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        private string ValidaFacturaConvenio(PagoFactura modelo)
        {
            string retValue = "";

            //Valida informacion de convenio
            if (modelo.IdConvenio > 0)
            {
                if (string.IsNullOrEmpty(modelo.ConsecutivoConvenio))
                    modelo.ConsecutivoConvenio = null;

                var detalle = _repositorioConvenioSAP.ObtenerDetalleConvenio(modelo.IdConvenio).Where(x => modelo.listaProducto.Select(y => y.CodigoSap).Contains(x.CodSapProducto));//Revisar EDSP 03-01-2018
                //foreach (var item in modelo.listaProducto)
                //{
                //    var prd = detalle.FirstOrDefault(x => x.CodSapProducto.Equals(item.CodigoSap));
                //    if (prd.Cantidad != null && prd.Cantidad > 0)
                //    {
                //        int? currentCantidadDisponible = (prd.CantidadDisponible == null ? prd.Cantidad : prd.CantidadDisponible) - ItemPorFactura.Count();
                //    }
                //}




                foreach (var item in detalle)
                {
                    if (item.Cantidad != null && item.Cantidad > 0)
                    {
                        var prod = _repositorio.ObtenerProductoPorCodSapProd_TipoProd(item.CodSapTipoProducto, item.CodSapProducto);
                        if (prod != null)
                        {
                            var ItemPorFactura = modelo.listaProducto.Where(d => d.IdProducto == prod.IdProducto).ToList();
                            if (ItemPorFactura != null && ItemPorFactura.Count() > 0)
                            {

                                int? currentCantidadDisponible = (item.CantidadDisponible == null ? item.Cantidad : item.CantidadDisponible) - ItemPorFactura.Count();
                                if (currentCantidadDisponible != null && currentCantidadDisponible < 0)
                                {
                                    retValue += prod.Nombre + " - Disponible: " + (item.CantidadDisponible == null ? item.Cantidad : item.CantidadDisponible) + "<br />";
                                }

                            }
                        }
                    }

                    if (item.CantidadxDia != null && item.CantidadxDia > 0)
                    {
                        var prod = _repositorio.ObtenerProductoPorCodSapProd_TipoProd(item.CodSapTipoProducto, item.CodSapProducto);

                        if (prod != null)
                        {
                            var ItemPorFactura = modelo.listaProducto.Where(d => d.IdProducto == prod.IdProducto).ToList();
                            if (ItemPorFactura != null && ItemPorFactura.Count() > 0)
                            {
                                int CantidadVendidasHoy = _repositorioConvenioSAP.ObtieneVentasConvenioProductoHoy(modelo.IdConvenio.ToString(), item.CodSapProducto, item.CodSapTipoProducto);
                                //if ((ItemPorFactura.Count() + CantidadVendidasHoy) > item.CantidadxDia)
                                //{
                                //    retValue += prod.Nombre + " - Disponible por Dia: " + item.CantidadxDia + "<br />";
                                //}
                                //else
                                //{
                                if (modelo.ConsecutivoConvenio != null)
                                {
                                    var cantidad = _repositorio.ObtenerVentasProductoCliente(modelo.ConsecutivoConvenio, prod.IdProducto);
                                    CantidadVendidasHoy = item.CantidadxDia.Value - (cantidad + ItemPorFactura.Count());
                                    if (CantidadVendidasHoy < 0)
                                    {
                                        retValue += prod.Nombre + " - Disponible por Dia: " + item.CantidadxDia + "<br />";
                                    }

                                }
                                //}
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(retValue))
                    retValue = "Cantidad para el convenio superada: <br />" + retValue;
            }

            return retValue;
        }


        /// <summary>
        /// Metodo Privado que se encarga de validar previamente una factura los productos de convenio
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        private string ValidaCompraNomina(PagoFactura modelo)
        {
            string retValue = "";

            var mediosPagoNomina = modelo.listMediosPago.Where(p => p.IdMedioPago == 6);

            if (mediosPagoNomina != null && mediosPagoNomina.Count() > 0)
            {
                if (mediosPagoNomina.Count() > 1)
                {
                    retValue = "Solo puede haber una forma de pago por Nomina";
                }
                else
                {
                    var fpNomina = mediosPagoNomina.FirstOrDefault();
                    if (string.IsNullOrEmpty(fpNomina.NumReferencia))
                    {
                        retValue = "Pago por Nomina invalido referencia vacia";
                    }
                    else
                    {
                        EstructuraEmpleado empleado = new EstructuraEmpleado();
                        empleado = _repositorioEstructuraEmpleado.ObtenerEmpleadoPorDocumento(fpNomina.NumReferencia);
                        if (empleado == null || empleado.IdEstructuraEmpleado == 0)
                        {
                            retValue = "Pago por Nomina invalido " + fpNomina.NumReferencia + " no existe";
                        }
                        else
                        {
                            if (empleado.CupoRestante < (decimal)fpNomina.Valor)
                            {
                                retValue = "No cuenta con cupo suficiente para pago por Nomina. <br /> Cupo Restante: " + empleado.CupoRestante;
                            }
                        }
                    }
                }
            }

            return retValue;
        }


        public Producto ObtenerBrazaleteConsecutivo(string Consecutivo, int taquillero, int recarga)
        {
            return _repositorio.ObtenerBrazaleteConsecutivo(Consecutivo, taquillero, recarga);
        }

        //public bool ActualizarBrazaleteEstado(List<Producto> lista)
        //{
        //    bool resp = false;
        //    resp = _repositorio.ActualizarBrazaleteEstado(lista);

        //    return resp;
        //}

        public Producto ObtenerProducto(int IdProducto)
        {
            return _repositorio.ObtenerProducto(IdProducto);
        }
        public Producto ObtenerProductoPtoEntrega(int IdProducto)
        {
            return _repositorio.ObtenerProductoPtoEntrega(IdProducto);
        }
        public Producto ObtenerProductoPtoFactura(int IdProducto)
        {
            return _repositorio.ObtenerProductoPtoFactura(IdProducto);
        }

        public IEnumerable<TipoGeneral> ObtenerProductosDonacion()
        {
            return _repositorio.ObtenerProductosDonacion();
        }

        public IEnumerable<Producto> ObtenerProductos()
        {
            return _repositorio.ObtenerProductos();
        }
        public IEnumerable<Producto> ObtenerProductosPtoEntrega()
        {
            return _repositorio.ObtenerProductosPtoEntrega();
        }
        public IEnumerable<Producto> ObtenerProductosPtoFactura()
        {
            return _repositorio.ObtenerProductosPtoFactura();
        }
        
        public IEnumerable<TipoGeneral> ObtenerLineaproductos()
        {
            return _repositorio.ObtenerLineaProducto();
        }

        public bool ActualizarProducto(Producto modelo)
        {
            return _repositorio.ActualizarProducto(modelo);
        }
        public bool ActualizarProductoPuntosEntrega(Producto modelo)
        {
            return _repositorio.ActualizarProductoPuntosEntrega(modelo);
        }
        public bool ActualizarProductoPuntosFactura(Producto modelo)
        {
            return _repositorio.ActualizarProductoPuntosFactura(modelo);
        }

        public Factura ObtenerFactura(string codigoFactura)
        {
            return _repositorio.ObtenerFactura(codigoFactura);
        }
        public Factura ObtenerFactura(int idFactura)
        {
            return _repositorio.ObtenerFactura(idFactura);
        }
        public int GuardarNotaCredito(NotaCredito modelo)
        {
            return _repositorio.GuardarNotaCredito(modelo);
        }

        public TipoGeneral ObtenerNotaCredito(int IdUsuario)
        {
            return _repositorio.ObtenerNotaCredito(IdUsuario);
        }

        public TipoGeneral ObtenerAnulaciones(int IdUsuario)
        {
            return _repositorio.ObtenerAnulaciones(IdUsuario);
        }

        public IEnumerable<AnulacionFactura> ObtenerFacturasAnular(int idPunto)
        {
            return _repositorioFactura.ObtenerFacturasAnular(idPunto);
        }

        public IEnumerable<AnulacionFacturaRedeban> ObtenerFacturasRedebanAnular(int idPunto)
        {
            return _repositorioFactura.ObtenerFacturasRedebanAnular(idPunto);
        }

        public string ValidaPermiteAnular(int idPunto)
        {
            string PermiteAnular = "1";
            var resp = _repositorioApertura.ObtenerLista($"WHERE IdPunto = {idPunto} AND [IdEstado] = 4 AND DATEADD(dd, 0, DATEDIFF(dd, 0, [Fecha])) = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))");

            if (resp != null)
            {
                if (resp.Count() >= 1)
                {
                    PermiteAnular = "0";
                }
            }

            return PermiteAnular;
        }

        /// <summary>
        /// RDSH: Valida si taquillero tiene una apertura activa.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public string ValidaPermiteAnularUsuario(int IdUsuario)
        {
            return _repositorioApertura.ValidaPermiteAnularUsuario(IdUsuario);
        }

        public bool AnularFacturas(IEnumerable<AnulacionFactura> modelo)
        {
            return _repositorioFactura.AnularFacturas(modelo);
        }

        public IEnumerable<MedioPagoFactura> ValidarTipoFactura(int idFactura)
        {
            return _repositorioFactura.ValidarTipoFactura(idFactura);
        }

        public IEnumerable<Producto> ObtenerProductoPorTipoProducto(string CodSapTipoProducto)
        {
            return _repositorio.ObtenerProductoPorTipoProducto(CodSapTipoProducto);
        }

        public LineaProducto ObtenerCodSapPorTipoProducto(int IdTipoProducto)
        {
            return _repositorio.ObtenerCodSapPorTipoProducto(IdTipoProducto);
        }

        public string ValidaReservaParqueadero(string CodigoBarrasBoletaControl)
        {
            string TieneReservaPark = "";
            TieneReservaPark = _repositorio.ValidaBoletaControlProducto(CodigoBarrasBoletaControl, "SAP_15");
            return TieneReservaPark;
        }

        public DescargueBoletaControl ObtenerProductosBoletaControl(string CodBarra, int usuario)
        {
            return _repositorio.ObtenerProductosBoletaControl(CodBarra, usuario);
        }

        public DescargueBoletaControl ObtenerListaDescargue(string CodBarra)
        {
            return _repositorio.ObtenerListaDescargue(CodBarra);
        }

        public DescargueBoletaControl ObtenerProductoInstitucional(string CodBarra)
        {
            return _repositorio.ObtenerProductoInstitucional(CodBarra);
        }

        public DescargueBoletaControl ObtenerProductosInstitucional(string CodBarra1, string CodBarra2)
        {
            return _repositorio.ObtenerProductosInstitucional(CodBarra1, CodBarra2);
        }

        public string DescargarProductosInstitucional(ImprimirBoletaControl modelo)
        {
            return _repositorio.DescargarProductosInstitucional(modelo);
        }

        public string DescargueBoletaFactura(List<Producto> listaProductos)
        {
            return _repositorio.DescargueBoletaFactura(listaProductos);
        }

        public IEnumerable<Producto> ObtenerTodosProductos()
        {
            return _repositorio.ObtenerTodosProductos();
        }
        public IEnumerable<Producto> ObtenerTodosProductosRestaurante()
        {
            return _repositorio.ObtenerTodosProductosRestaurante();
        }

        public RedencionBoletaControl RedencionBoletaControl(ImprimirBoletaControl modelo)
        {
            return _repositorio.RedencionBoletaControl(modelo);
        }

        public FacturaImprimir ObtenerFacturaImprimir(int IdFactura)
        {
            var prodDonacion = _repositorio.ObtenerProductosDonacion();
            FacturaImprimir objFactura = _repositorio.ObtenerFacturaImprimir(IdFactura);
            if (objFactura == null) objFactura = new FacturaImprimir();

            Parametro _paramFacturaPosTextoHead1 = _repositorioParametro.ObtenerParametroPorNombre("FacturaPosTextoHead1");
            if (_paramFacturaPosTextoHead1 == null) _paramFacturaPosTextoHead1 = new Parametro();
            Parametro _paramFacturaPosTextoFoot1 = _repositorioParametro.ObtenerParametroPorNombre("FacturaPosTextoFoot1");
            if (_paramFacturaPosTextoFoot1 == null) _paramFacturaPosTextoFoot1 = new Parametro();
            Parametro _paramFacturaPosTextoFoot2 = _repositorioParametro.ObtenerParametroPorNombre("FacturaPosTextoFoot2");
            if (_paramFacturaPosTextoFoot2 == null) _paramFacturaPosTextoFoot2 = new Parametro();
            Parametro _paramFacturaPosTextoFoot4 = _repositorioParametro.ObtenerParametroPorNombre("FacturaPosTextoFoot4");
            if (_paramFacturaPosTextoFoot4 == null) _paramFacturaPosTextoFoot4 = new Parametro();
            Parametro _paramFacturaPosTextoFoot3 = _repositorioParametro.ObtenerParametroPorNombre("FacturaPosTextoFoot3");
            if (_paramFacturaPosTextoFoot3 == null) _paramFacturaPosTextoFoot3 = new Parametro();
            Parametro _paramFacturaPosTextoFootFinal = _repositorioParametro.ObtenerParametroPorNombre("FacturaPosTextoFootFinal");
            if (_paramFacturaPosTextoFootFinal == null) _paramFacturaPosTextoFootFinal = new Parametro();
            Parametro _paramPuntosTextoPropinas = _repositorioParametro.ObtenerParametroPorNombre("PuntosTextoPropinas");
            if (_paramPuntosTextoPropinas == null) _paramPuntosTextoPropinas = new Parametro();

            objFactura.TextoHead1 = _paramFacturaPosTextoHead1.Valor;
            //  objFactura.TextoFoot1 = string.Format(_paramFacturaPosTextoFoot1.Valor,objFactura.ConsecutivoPunto, objFactura.ConsecutivoPunto);
            if (objFactura.CodigoFactura.StartsWith(objFactura.ConsecutivoInicialPunto.Substring(0, 3)))
            {
                objFactura.TextoFoot1 = string.Format(_paramFacturaPosTextoFoot1.Valor, objFactura.ConsecutivoInicialPunto, objFactura.ConsecutivoFinalPunto, objFactura.ResolucionPunto, objFactura.FechaResolucion.ToString("dd/MM/yyyy"), objFactura.FechaFinalResolucion.ToString("dd/MM/yyyy"));
                objFactura.TextoFoot2 = _paramFacturaPosTextoFoot2.Valor;
            }
            else
            {
                //presenta texto de donaciones
                if (objFactura.CodigoFactura.StartsWith("DO"))
                {
                    objFactura.TextoFoot2 = string.Empty;
                    objFactura.TextoFoot1 = prodDonacion.First(x => x.CodSAP.Equals(objFactura.ListaProdSap.First())).Nombre;
                }
                else
                {
                    objFactura.TextoFoot1 = string.Format(_paramFacturaPosTextoFoot1.Valor, objFactura.ConsecutivoInicialPunto, objFactura.ConsecutivoFinalPunto, objFactura.ResolucionPunto, objFactura.FechaResolucion.ToString("dd/MM/yyyy"), objFactura.FechaFinalResolucion.ToString("dd/MM/yyyy"));
                    objFactura.TextoFoot2 = _paramFacturaPosTextoFoot2.Valor;
                }

            }
            objFactura.TextoFoot4 = _paramFacturaPosTextoFoot4.Valor;
            objFactura.PuntosTextoPropinas = _paramPuntosTextoPropinas.Valor;

            objFactura.TextoFoot3 = _paramFacturaPosTextoFoot3.Valor;
            objFactura.TextoFootFinal = _paramFacturaPosTextoFootFinal.Valor;

            return objFactura;
        }

        public Factura ObtenerUltimaFactura(int IdPunto)
        {
            return _repositorio.ObtenerUltimaFactura(IdPunto);
        }

        public FacturaImprimir ObtenerFacturaNotaCredImprimir(int IdFactura, int IdNotaCredito)
        {
            FacturaImprimir objFactura = _repositorio.ObtenerFacturaNotaCredImprimir(IdFactura, IdNotaCredito);
            if (objFactura == null) objFactura = new FacturaImprimir();

            return objFactura;
        }

        public ProductoBoleta BuscarBoleta(string CodBarraInicio, string CodBarraFinal, string Codproducto, int usuario)
        {
            return _repositorio.BuscarBoleta(CodBarraInicio, CodBarraFinal, Codproducto, usuario);
        }

        public int GuardarLogVenta(LogVentaPunto modelo)
        {
            return _repositorio.GuardarLogVenta(modelo);
        }

        public EstructuraEmpleado ObtenerEmpleadoPorConsecutivo(string Consecutivo)
        {
            return _repositorioEstructuraEmpleado.ObtenerEmpleadoPorConsecutivo(Consecutivo);
        }

        /// <summary>
        /// RDSH: Retorna los productos que se van a entregar desde destrezas.
        /// </summary>
        /// <param name="CodSapTipoProducto"></param>
        /// <returns></returns>
        public IEnumerable<Producto> ObtenerPremiosDestrezas()
        {
            return _repositorio.ObtenerPremiosDestrezas();
        }

        public CortesiasEmpleado ObtenerCortesiaEmpleado(string documento)
        {
            return _repositorioEstructuraEmpleado.ObtenerCortesiaEmpleado(documento);
        }

        public string GuardarCortesiaEmpleado(GuardarCortesiaEmpleado modelo)
        {
            return _repositorioEstructuraEmpleado.GuardarCortesiaEmpleado(modelo);
        }

        public IEnumerable<ProductosPedidos> ObtenerProductosPedidosDia()
        {
            return _repositorio.ObtenerProductosPedidosDia();
        }

        public FacturaRespuesta RegistarCompraTienda(IEnumerable<ProductosTienda> productosTienda, List<PagoFacturaMediosPago> mediosPago, int IdUsuario, int idPunto)
        {

            FacturaRespuesta facturaRespuesta = new FacturaRespuesta();
            string _tipoPasaporte = _repositorioParametro.ObtenerParametroPorNombre("Uso_BrazaletesPOS").Valor;
            string _tipoAyB = _repositorioParametro.ObtenerParametroPorNombre("CodigoSapTipoProductoAyB").Valor;
            string _tipoSouvenir = _repositorioParametro.ObtenerParametroPorNombre("SouveniresPOS").Valor;
            string codSapFan = _repositorioParametro.ObtenerParametroPorNombre("CodigoSapProductoClienteFan").Valor;
            string resultadoIdConvenioFan = _repositorioParametro.ObtenerParametroPorNombre("IdClientefan").Valor;
            int idConvenioFan = string.IsNullOrWhiteSpace(resultadoIdConvenioFan) ? 0 : int.Parse(resultadoIdConvenioFan);
            string resultadoIdFilaExpress = _repositorioParametro.ObtenerParametroPorNombre("IdFilaExpress").Valor;
            int idFilaExpress = string.IsNullOrWhiteSpace(resultadoIdFilaExpress) ? 0 : int.Parse(resultadoIdFilaExpress);

            foreach (var productoTienda in productosTienda)
            {
                Producto producto = _repositorio.ObtenerProducto(productoTienda.IdProducto);
                if (producto == null)
                {
                    facturaRespuesta.MensajeRespuesta = string.Concat("El codigo de producto ", productoTienda.IdProducto, " no existe");
                    return facturaRespuesta;
                }
                else if (producto.CodSapTipoProducto == _tipoPasaporte)
                {
                    productoTienda.CodigoSap = producto.CodigoSap;
                    productoTienda.Nombre = producto.Nombre;
                    productoTienda.Entregado = true;
                    productoTienda.AplicaBrazalete = true;
                }
                else
                {
                    if (producto.CodSapTipoProducto == _tipoAyB || producto.CodSapTipoProducto == _tipoSouvenir)
                    {
                        productoTienda.Entregado = false;
                    }
                    productoTienda.CodigoSap = producto.CodigoSap;
                    productoTienda.Nombre = producto.Nombre;
                    productoTienda.AplicaBrazalete = false;
                }
            }

            string consecutivo = string.Empty;

            foreach (ProductosTienda productoBrazalete in productosTienda.Where(x => x.AplicaBrazalete == true))
            {
                /*string resultado = _repositorioBoleteria.InsertarBoleteriaExterna((int)Enumerador.LineaProducto.Brazaletes, productoBrazalete.IdProducto, productoBrazalete.Precio,
                                                                                 productoBrazalete.FechaUso, (int)Enumerador.OrigenBrazalete.Pagina);*/
                string resultado = _repositorioBoleteria.InsertarBoleteriaExterna((int)Enumerador.LineaProducto.Brazaletes, productoBrazalete.IdProducto, productoBrazalete.Precio,
                                                                                 productoBrazalete.FechaUso, (int)Enumerador.OrigenBrazalete.Pagina, IdUsuario);
                if (resultado.Contains("Error"))
                {
                    facturaRespuesta.MensajeRespuesta = string.Concat("Se presento un error al generar el brazalete del codigo de producto ", productoBrazalete.IdProducto,
                                                                       " no existe");
                    return facturaRespuesta;
                }

                productoBrazalete.IdBoleteria = int.Parse(resultado.Split('|')[0]);
                productoBrazalete.Consecutivo = resultado.Split('|')[1];
            }

            List<Producto> productos = new List<Producto>();

            foreach (ProductosTienda item in productosTienda)
            {
                Producto producto = new Producto();
                producto.IdProducto = item.IdProducto;
                producto.CodigoSap = item.CodigoSap;
                producto.Nombre = item.Nombre;
                producto.Cantidad = 1;
                producto.Precio = item.Precio;
                producto.IdDetalleProducto = item.IdBoleteria;
                producto.Entregado = item.Entregado;
                producto.DataExtension = string.Empty;

                productos.Add(producto);
            }

            PagoFactura pagoFactura = new PagoFactura();
            pagoFactura.listMediosPago = mediosPago;
            pagoFactura.listaProducto = productos;
            pagoFactura.Cambio = 0;
            pagoFactura.EsContingencia = false;
            pagoFactura.IdUsuario = IdUsuario;
            pagoFactura.IdPunto = idPunto;

            string error = string.Empty;
            string respuesta = InsertarCompra(pagoFactura, ref error);
            if (respuesta.Contains("Error"))
            {
                facturaRespuesta.MensajeRespuesta = string.Concat("Se presento un error al generar la factura.");
                return facturaRespuesta;
            }

            Factura factura = ObtenerFactura(int.Parse(respuesta));

            foreach (DetalleFactura item in factura.DetalleFactura)
            {
                ProductosTiendaRespuesta productosTiendaRespuesta = new ProductosTiendaRespuesta();
                if (item.IdDetalleProducto > 0 && item.Id_Producto != idFilaExpress)
                {
                    Boleteria boleteria = _repositorioBoleteria.Obtener(item.IdDetalleProducto);
                    productosTiendaRespuesta.Consecutivo = boleteria.Consecutivo;
                    productosTiendaRespuesta.IdProducto = item.Id_Producto;
                    productosTiendaRespuesta.Precio = item.Precio;
                    productosTiendaRespuesta.FechaUso = boleteria.FechaInicioEvento;

                }
                else
                {
                    productosTiendaRespuesta.IdProducto = item.Id_Producto;
                    productosTiendaRespuesta.Precio = item.Precio;
                }

                if (facturaRespuesta.Productos == null)
                {
                    facturaRespuesta.Productos = new List<ProductosTiendaRespuesta>();
                }

                facturaRespuesta.Productos.Add(productosTiendaRespuesta);

            }

            List<ConvenioConsecutivos> convenios = new List<ConvenioConsecutivos>();

            foreach (ProductosTienda item in productosTienda.Where(x => x.CodigoSap.Equals(codSapFan)))
            {
                var convenio = new ConvenioConsecutivos
                {
                    IdConvenio = idConvenioFan,
                    Consecutivo = item.CorreoUsuario,
                    FechasEspeciales = "TIENDA",
                    FechaInicial = DateTime.Now,
                    FechaFinal = DateTime.Now.AddYears(1)
                };

                convenios.Add(convenio);
            }

            if (convenios.Count() > 0)
            {

                _repositorioConvenioSAP.InsertarConvenioConsecutivos(convenios);
            }


            facturaRespuesta.ConsecutivoFactura = factura.CodigoFactura;

            return facturaRespuesta;

        }//ACABA EL METODO



        public string RegistrarCodigoBoleteria(int idProducto, double precio, int usuarioCreacion)
        {
            return _repositorioBoleteria.InsertarBoleteriaExterna((int)Enumerador.LineaProducto.Brazaletes, idProducto, precio,
                                                                                 DateTime.Now, (int)Enumerador.OrigenBrazalete.ImpresionLinea, usuarioCreacion);
        }

        public string RegistroRolloImpresionLinea(Producto producto)
        {
            return _repositorioBoleteria.RegistroRolloImpresionLinea(producto);
        }

        public IEnumerable<Producto> ObtenerPasaportesActivos()
        {
            List<Producto> productos = _repositorio.ObtenerProductoPorTipoProducto(_repositorioParametro.ObtenerParametroPorNombre("CodigoSapUsoBrazaletes").Valor, true).ToList();
            foreach (Producto producto in productos)
            {
                producto.ArchivoProducto = _repositorio.ObtenerArchivoBrazalete(producto.IdProducto);
            }

            return productos.AsEnumerable();
        }

        public IEnumerable<PuntoBrazalete> ObtenerPasaporteXPunto(int idPunto)
        {
            return _repositorio.ObtenerPasaporteXpunto(idPunto);
        }

        public bool ActualizarPasaporteXPunto(List<PuntoBrazalete> puntoBrazalete)
        {
            return _repositorio.ActualizarPasaporteXPunto(puntoBrazalete);
        }

        public bool ActualizarArchivoBrazalete(ArchivoBrazalete archivoBrazalete)
        {
            return _repositorio.ActualizarArchivoBrazalete(archivoBrazalete);
        }

        public FacturaRespuesta RegistarReagendamientoTienda(IEnumerable<ProductosTienda> productosTienda, List<PagoFacturaMediosPago> mediosPago, int IdUsuario, int idPunto)
        {

            FacturaRespuesta facturaRespuesta = new FacturaRespuesta();
            string _tipoPasaporte = _repositorioParametro.ObtenerParametroPorNombre("Uso_BrazaletesPOS").Valor;
            string _tipoAyB = _repositorioParametro.ObtenerParametroPorNombre("CodigoSapTipoProductoAyB").Valor;
            string _tipoSouvenir = _repositorioParametro.ObtenerParametroPorNombre("SouveniresPOS").Valor;
            string codSapCambioTarifaAyB = _repositorioParametro.ObtenerParametroPorNombre("codSapCambioTarifaAyB").Valor;
            string codSapCambioTarifaPasaporte = _repositorioParametro.ObtenerParametroPorNombre("codSapCambioTarifaPasaporte").Valor;
            string codSapCambioTarifaSouvenir = _repositorioParametro.ObtenerParametroPorNombre("codSapCambioTarifaSouvenir").Valor;

            Producto productoCambioTarifaAyB = _repositorio.ObtenerProductoPorCodSap(codSapCambioTarifaAyB);
            productoCambioTarifaAyB.Precio = 0;
            Producto productoCambioTarifaPasaporte = _repositorio.ObtenerProductoPorCodSap(codSapCambioTarifaPasaporte);
            productoCambioTarifaPasaporte.Precio = 0;
            Producto productoCambioTarifaSouvenir = _repositorio.ObtenerProductoPorCodSap(codSapCambioTarifaPasaporte);
            productoCambioTarifaSouvenir.Precio = 0;

            foreach (var productoTienda in productosTienda)
            {
                Producto producto = _repositorio.ObtenerProducto(productoTienda.IdProducto);
                if (producto == null)
                {
                    facturaRespuesta.MensajeRespuesta = string.Concat("El codigo de producto ", productoTienda.IdProducto, " no existe");
                    return facturaRespuesta;
                }
                else if (producto.CodSapTipoProducto == _tipoAyB)
                {
                    productoCambioTarifaAyB.Precio += productoTienda.Precio;

                }
                else if (producto.CodSapTipoProducto == _tipoSouvenir)
                {
                    productoCambioTarifaSouvenir.Precio += productoTienda.Precio;
                }
                else
                {
                    productoCambioTarifaPasaporte.Precio += productoTienda.Precio;
                    if (producto.CodSapTipoProducto == _tipoPasaporte)
                        productoTienda.AplicaBrazalete = true;
                }
            }

            string consecutivo = string.Empty;

            foreach (ProductosTienda productoBrazalete in productosTienda.Where(x => x.AplicaBrazalete == true))
            {
                Boleteria resultado = _repositorioBoleteria.ObtenerPorConsecutivo(productoBrazalete.Consecutivo);
                if (resultado != null)
                {
                    resultado.FechaInicioEvento = Utilidades.FormatoFechaSQLHora(productoBrazalete.FechaUso.ToString("dd-MM-yyyy"), "00:00");
                    resultado.FechaUsoInicial = Utilidades.FormatoFechaSQLHora(productoBrazalete.FechaUso.ToString("dd-MM-yyyy"), "00:00");
                    resultado.FechaFinEvento = Utilidades.FormatoFechaSQLHora(productoBrazalete.FechaUso.ToString("dd-MM-yyyy"), "23:59");
                    resultado.FechaUsoFinal = Utilidades.FormatoFechaSQLHora(productoBrazalete.FechaUso.ToString("dd-MM-yyyy"), "23:59");
                    _repositorioBoleteria.Actualizar(ref resultado);
                }
                else
                {
                    facturaRespuesta.MensajeRespuesta = string.Concat("El pasaporte enviado no existe.");
                    return facturaRespuesta;
                }

            }

            List<Producto> productos = new List<Producto>();

            if (productoCambioTarifaAyB.Precio > 0)
            {
                productos.Add(productoCambioTarifaAyB);
            }
            if (productoCambioTarifaPasaporte.Precio > 0)
            {
                productos.Add(productoCambioTarifaPasaporte);
            }
            if (productoCambioTarifaSouvenir.Precio > 0)
            {
                productos.Add(productoCambioTarifaSouvenir);
            }

            PagoFactura pagoFactura = new PagoFactura();
            pagoFactura.listMediosPago = mediosPago;
            pagoFactura.listaProducto = productos;
            pagoFactura.Cambio = 0;
            pagoFactura.EsContingencia = false;
            pagoFactura.IdUsuario = IdUsuario;
            pagoFactura.IdPunto = idPunto;

            string error = string.Empty;
            string respuesta = InsertarCompra(pagoFactura, ref error);
            if (respuesta.Contains("Error"))
            {
                facturaRespuesta.MensajeRespuesta = string.Concat("Se presento un error al generar la factura.");
                return facturaRespuesta;
            }

            Factura factura = ObtenerFactura(int.Parse(respuesta));

            foreach (DetalleFactura item in factura.DetalleFactura)
            {
                ProductosTiendaRespuesta productosTiendaRespuesta = new ProductosTiendaRespuesta();
                if (item.IdDetalleProducto > 0)
                {
                    Boleteria boleteria = _repositorioBoleteria.Obtener(item.IdDetalleProducto);
                    productosTiendaRespuesta.Consecutivo = boleteria.Consecutivo;
                    productosTiendaRespuesta.IdProducto = item.Id_Producto;
                    productosTiendaRespuesta.Precio = item.Precio;
                    productosTiendaRespuesta.FechaUso = boleteria.FechaInicioEvento;

                }
                else
                {
                    productosTiendaRespuesta.IdProducto = item.Id_Producto;
                    productosTiendaRespuesta.Precio = item.Precio;
                }

                if (facturaRespuesta.Productos == null)
                {
                    facturaRespuesta.Productos = new List<ProductosTiendaRespuesta>();
                }

                facturaRespuesta.Productos.Add(productosTiendaRespuesta);

            }

            facturaRespuesta.ConsecutivoFactura = factura.CodigoFactura;

            return facturaRespuesta;

        }

        public IEnumerable<FacturaValidaUsoRespuesta> ValidarUsoFactura(string CodigoUsoFactura)
        {

            string consecutivo = string.Empty;

            IEnumerable<FacturaValidaUsoRespuesta> facturas = _repositorioFactura.ValidarUsoFactura(CodigoUsoFactura);

            return facturas;

        }

        public IEnumerable<BrazaleteReimpresion> ObtenerBrazaleteReimpresion(int idPunto)
        {
            return _repositorio.ObtenerBrazaleteReimpresion(idPunto);
        }

        public IEnumerable<PuntoBrazalete> ObtenerPasaporteXPuntoPOS(int idpunto, int idUsuario)
        {
            return _repositorio.ObtenerPasaporteXPuntoPOS(idpunto, idUsuario);
        }

        public int ControlBoleteria(int idBoleta)
        {
            int data = _repositorioRecoleccion.ControlBoleteria(idBoleta);
            return data;
        }

        public string ModificarControlBoleteria(int idBoleta, int NumBoletasRestantes)
        {
            string dato = _repositorioRecoleccion.ModificarControlBoleteria(idBoleta, NumBoletasRestantes);
            return dato;
        }

        public Producto ValidarImpresionEnLinea(int idBoleteria)
        {
            var dato = _repositorioRecoleccion.ValidarImpresionEnLinea(idBoleteria);
            return dato;
        }

        public IEnumerable<Producto> NumBoletasControlBoleteria()
        {
            var dato = _repositorioRecoleccion.NumBoletasControlBoleteria();
            return dato;
        }

        public IEnumerable<Producto> VerPasaportesCodigoPedido(string codigoPedido)
        {
            var dato = _repositorioRecoleccion.VerPasaportesCodigoPedido(codigoPedido);
            return dato;
        }

        public IEnumerable<PlantillaBrazalete> ObtenerPlantillas()
        {
            return _repositorio.ObtenerPlantillaBrazalete();
        }

        public bool InsertarPlantillaBrazalete(PlantillaBrazalete plantilla)
        {
            return _repositorio.InsertarPlanillaBrazalete(plantilla);
        }

        //public string ConsultarVencimientoTarjeta(string tarjeta)
        //{
        //    return _repositorioTarjetaRecargable.ConsultarVencimientoTarjeta(tarjeta);
        //}
    }
}
