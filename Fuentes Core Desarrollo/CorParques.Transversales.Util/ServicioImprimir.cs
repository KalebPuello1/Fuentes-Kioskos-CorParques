using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using CorParques.Transversales.Contratos;
using System.Configuration;

namespace CorParques.Transversales.Util
{
    public class ServicioImprimir : IServicioImprimir
    {

        #region Constructor

        public ServicioImprimir()
        {

        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Para generar impresión de cortesías.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        public string ImprimirTicketCortesias(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.Usuario = objTicket.Usuario;
                strRetorno = objRecibo.ImprimirTicketCortesias();
                if (strRetorno.Trim().Length > 0)
                {
                    throw new ArgumentException(strRetorno);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera impresión de alistamiento recoleccion.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <param name="blnAlistamiento"></param>
        /// <returns></returns>
        public string ImprimirTicketRecoleccion(TicketImprimir objTicket, bool blnAlistamiento, bool blnDataTable)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                if (!blnAlistamiento)
                {
                    //Si no es alistamiento, se trata de un proceso de recoleccion de supervisor o de nido, por eso se envian
                    //las firmas en el pie de pagina para q sean impresas.
                    objRecibo.AdicionarFirma(objTicket.PieDePagina);
                }
                else
                {
                    objRecibo.AdicionarContenido(objTicket.PieDePagina);
                }
                objRecibo.Usuario = objTicket.Usuario;
                if (blnDataTable)
                {
                    objRecibo.TablaDetalle = objTicket.TablaDetalle;
                    objRecibo.ColumnasMoneda = objTicket.ColumnasMoneda;
                    objRecibo.ColumnaTotalizar = objTicket.ColumnaTotalizar;

                    strRetorno = objRecibo.ImprimirTicketTabla();
                }
                else
                {
                    strRetorno = objRecibo.ImprimirTicket();
                }
                if (strRetorno.Trim().Length > 0)
                {
                    throw new ArgumentException(strRetorno);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketRecoleccion_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// GALD: Para generar impresíon de apertura.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        public string ImprimirTicketApertura(TicketImprimir objTicket)
        {
            TicketFactura objRecibo = new TicketFactura();
            FacturaImprimir FacturaImprimir = new FacturaImprimir();
            string strRetorno = string.Empty;

            try
            {

                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.Documento(true);
                objRecibo.AdicionarContenidoHeader("");
                objRecibo.AdicionarContenidoHeader(objTicket.AdicionarContenidoHeader);
                FacturaImprimir.Fecha = System.DateTime.Now;
                objRecibo._FacturaImprimir = FacturaImprimir;
                objRecibo.AdicionarContenido("");
                objRecibo.AdicionarContenido("Observaciones");
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.AdicionarContenido("");
                objRecibo.AdicionarFirma(objTicket.Firma);
                objRecibo.NombrePunto = objTicket.NombrePunto;
                objRecibo.ImprimirTicketGrupo();

                return strRetorno;
            }
            catch (Exception ex)
            {
                return (string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }


        }

        public string ImprimirTicketPreFactura(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.AdicionarContenido(objTicket.AdicionarContenidoHeader);
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;

                //objRecibo.ImprimirTicket();
                objRecibo.ImprimirTicketPreFactura();
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketPreFactura_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }

            return strRetorno;

        }

        /// <summary>
        /// RDSH: Impresion de varios tickets en un solo recibo.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <param name="blnFirma"></param>
        public string ImprimirTicketMasivo(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.AdicionarFirma(objTicket.Firma);
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.ListaTickets = objTicket.ListaTickets;

                objRecibo.ImprimirTicketMasivo();

            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketMasivo_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }
            //ImprimirTicketMasivo(objTicket);
            ImprimirTicketMasivocierre(objTicket);
            return strRetorno;
            //return ImprimirTicketMasivo(objTicket);
        }

        public string ImprimirTicketMasivocierre(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.AdicionarFirma(objTicket.Firma);
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.ListaTickets = objTicket.ListaTickets;

                objRecibo.ImprimirTicketMasivo();

            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketMasivo_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }
            //ImprimirTicketMasivo(objTicket);
            return strRetorno;
            //return ImprimirTicketMasivo(objTicket);
        }

        /// <summary>
        /// MAnuel Ochoa Imprimir factura
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        public string ImprimirTicketPosFactura(FacturaImprimir _FacturaImprimir)
        {
            TicketFactura objTicketFactura = new TicketFactura();
            string strRetorno = string.Empty;

            try
            {
                int bandera = 0;
                if (_FacturaImprimir != null)
                {


                    foreach (string[] objMetodosPago in _FacturaImprimir.MetodosPago)
                    {
                        if (objMetodosPago[0].Equals("VALOR B") && !objMetodosPago[0].Equals("Consumo B") && !objMetodosPago[0].Equals("SALDO B"))
                        {
                            bandera = 1;
                            if (_FacturaImprimir.CodigoFactura == "")
                            {
                                _FacturaImprimir.BanderaBonoRegalo = true;
                            }

                        }

                    }
                }
                objTicketFactura._FacturaImprimir = _FacturaImprimir;
                if (_FacturaImprimir.CodigoFactura == "" && bandera == 1)
                {
                    objTicketFactura.CodigoBarrasProp = _FacturaImprimir.Id_Factura;
                }
                else
                {
                    objTicketFactura.CodigoBarrasProp = _FacturaImprimir.CodigoFactura;
                }
                objTicketFactura.AdicionarContenidoHeader("");
                objTicketFactura.AdicionarContenidoHeader(_FacturaImprimir.TextoHead1);
                objTicketFactura.AdicionarContenidoHeader("");
                objTicketFactura.TituloColumnas = $"{(_FacturaImprimir.CodigoFactura.StartsWith("RE") ? "RECARGA" : "PRODUCTO")}|CANTIDAD|VALOR";
                objTicketFactura.AdicionarContenido("");
                if (_FacturaImprimir.BanderaBonoRegalo == false)
                {
                    objTicketFactura.AdicionarContenido(_FacturaImprimir.TextoFoot1);
                    objTicketFactura.AdicionarContenido("");
                    objTicketFactura.AdicionarContenido(_FacturaImprimir.TextoFoot2);
                    objTicketFactura.AdicionarContenido("");
                }

                string[] strsplit;
                bool existepunto = false;
                string codPunto = ConfigurationManager.AppSettings["IdPunto"].ToString();
                strsplit = _FacturaImprimir.PuntosTextoPropinas.Split('|');
                for (int i = 0; i < strsplit.Length; i++)
                {
                    if (codPunto == strsplit[i].ToString())
                    {
                        existepunto = true;
                        break;
                    }
                }
                if (existepunto)
                {
                    objTicketFactura.AdicionarContenido(_FacturaImprimir.TextoFoot4);
                    objTicketFactura.AdicionarContenido("");
                }
                if (!_FacturaImprimir.CodigoFactura.StartsWith("DO"))
                {
                    if (_FacturaImprimir.BanderaBonoRegalo == false)
                    {
                        objTicketFactura.AdicionarContenido(_FacturaImprimir.TextoFoot3);
                        objTicketFactura.AdicionarContenido("");
                    }
                }
                objTicketFactura.AdicionarContenido(_FacturaImprimir.TextoFootFinal);
                objTicketFactura.AdicionarContenido("");

                strRetorno = objTicketFactura.ImprimirTicket();
                if (strRetorno.Trim().Length > 0)
                {
                    throw new ArgumentException(strRetorno);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objTicketFactura = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// MAnuel Ochoa Imprimir factura notacredito
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        public string ImprimirTicketPosFacturaNotaCredito(FacturaImprimir _FacturaImprimir)
        {
            TicketFactura objTicketFactura = new TicketFactura();
            string strRetorno = string.Empty;

            try
            {
                objTicketFactura._FacturaImprimir = _FacturaImprimir;
                objTicketFactura.TituloColumnas = "PRODUCTO|CANTIDAD|VALOR";
                objTicketFactura.AdicionarContenido("");
                objTicketFactura.AdicionarFirma(string.Concat("SUPERVISOR: ", _FacturaImprimir.Supervisor, "|CLIENTE"));
                objTicketFactura.AdicionarContenido("");
                objTicketFactura.AdicionarContenido("");
                strRetorno = objTicketFactura.ImprimirTicket();
                if (strRetorno.Trim().Length > 0)
                {
                    throw new ArgumentException(strRetorno);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objTicketFactura = null;
            }

            return strRetorno;
        }

        public void imprimirCambioBoleta()
        {
            Ticket objRecibido = new Ticket();
            try
            {
                objRecibido.Usuario = "Mary";

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// MAnuel Ochoa Imprimir factura
        /// </summary>
        public void ImprimirCupoDebito(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();

            try
            {
                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.ImprimirTicket();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }
        }

        /// <summary>
        /// RDSH: Genera impresión de elementos para el cierre de punto.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <param name="blnAlistamiento"></param>
        /// <returns></returns>
        public string ImprimirTicketCierrePunto(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.AdicionarFirma(objTicket.Firma);
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.TablaDetalle = objTicket.TablaDetalle;
                strRetorno = objRecibo.ImprimirTicketTabla();

                if (strRetorno.Trim().Length > 0)
                {
                    throw new ArgumentException(strRetorno);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketCierrePunto_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// MAnuel Ochoa Imprimir factura
        /// </summary>
        public void ImprimirParqueadero(TicketImprimir objTicket, string Placa)
        {
            Ticket objRecibo = new Ticket();

            try
            {

                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                //Utilidades.RegistrarError(new Exception("Se envio Codigo de barras" + objTicket.CodigoBarrasProp.ToString()), "Se envio Codigo de barras" + objTicket.CodigoBarrasProp.ToString());
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.LineaParqueadero = objTicket.AdicionarContenidoHeader;
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.PlacaParqueadero = Placa;
                objRecibo.IdInterno = objTicket.IdInterno;
                objRecibo.ImprimirTicketParqueadero();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "No genero para agregar codigo de barras");
                throw new ArgumentException(string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }
        }


        public void ImprimirCambioboleta(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();

            try
            {
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.NombrePunto = objTicket.NombrePunto;
                objRecibo.ImprimirTicketCambioBoleta();

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "No genero para agregar codigo de barras");
                throw new ArgumentException(string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }
        }


        /// <summary>
        /// EDSP: Imprime recipo cupo empleado
        /// </summary>
        /// <param name="ticket"></param>
        public void ImprimirReciboCupoEmpleado(TicketImprimir ticket, FacturaImprimir ticketFactura)
        {
            TicketFactura objRecibo = new TicketFactura();

            try
            {
                objRecibo._FacturaImprimir = ticketFactura;
                objRecibo.CodigoBarrasProp = ticket.CodigoBarrasProp;
                objRecibo.TituloRecibo = ticket.TituloRecibo;
                objRecibo.Documento(false);
                objRecibo.AdicionarFirma(ticket.Firma);
                objRecibo.ImprimirTicketGrupoCupo();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ImprimirReciboCupoEmpleado_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }
        }
        /// <summary>
        /// DANR: Imprime recibo de soporte tarjeta recargable
        /// </summary>
        /// <param name="ticket"></param>
        public void ImprimirReciboTarjetaRecargable(TicketImprimir ticket, FacturaImprimir ticketFactura)
        {
            TicketFactura objRecibo = new TicketFactura();

            try
            {
                objRecibo._FacturaImprimir = ticketFactura;
                objRecibo.CodigoBarrasProp = ticket.CodigoBarrasProp;
                objRecibo.TituloRecibo = ticket.TituloRecibo;
                objRecibo.Documento(false);
                //objRecibo.AdicionarFirma(ticket.Firma);
                objRecibo.ImprimirTicketTarjetaRecargable();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ImprimirReciboCupoEmpleado_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }
        }
        /// <summary>
        /// GALD: Para generar impresíon de apertura a taquillero.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        public string ImprimirTicketAperturaTaquillero(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {

                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.AdicionarContenidoHeader("");
                objRecibo.AdicionarContenidoHeader(objTicket.AdicionarContenidoHeader);
                objRecibo.AdicionarContenido("");
                objRecibo.AdicionarContenido("Observaciones");
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.AdicionarContenido("");
                objRecibo.AdicionarFirma(objTicket.Firma);
                objRecibo.ImprimirTicketGrupo();

                return strRetorno;
            }
            catch (Exception ex)
            {
                return (string.Concat("Error en ImprimirTicketCortesias_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }


        }

        /// <summary>
        /// RDSH: Genera la impresion de las boletas de adicion de pedido.
        /// </summary>
        /// <param name="objTicket"></param>
        public string ImprimirAdicionPedido(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;
                strRetorno = objRecibo.ImprimirTicket();
                if (strRetorno.Trim().Length > 0)
                {
                    throw new ArgumentException(strRetorno);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirAdicionPedido_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }

            return strRetorno;
        }

        /// <summary>
        /// RDSH: Genera impresión para las boletas de uso destreza y atracción.
        /// </summary>
        /// <param name="objTicket"></param>
        public void ImprimirUsoAtraccionDestreza(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();

            try
            {
                objRecibo.CodigoBarrasProp = objTicket.CodigoBarrasProp;
                objRecibo.TituloRecibo = objTicket.TituloRecibo;
                objRecibo.TituloColumnas = objTicket.TituloColumnas;
                objRecibo.ListaArticulos = objTicket.ListaArticulos;
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.ImprimirTicket();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ImprimirUsoAtraccionDestreza_ServicioImprimir ", ex.Message));
            }
            finally
            {
                objRecibo = null;
            }
        }

        /// <summary>
        /// RDSH: Impresion de varios tickets en un solo recibo.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <param name="blnFirma"></param>
        public string ImprimirTicketArqueo(TicketImprimir objTicket)
        {
            Ticket objRecibo = new Ticket();
            string strRetorno = string.Empty;

            try
            {
                objRecibo.AdicionarContenido(objTicket.PieDePagina);
                objRecibo.AdicionarFirma(objTicket.Firma);
                objRecibo.Usuario = objTicket.Usuario;
                objRecibo.ListaTickets = objTicket.ListaTickets;
                objRecibo.ImprimirTicketArqueo();
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error en ImprimirTicketMasivo_ServicioImprimir ", ex.Message);
            }
            finally
            {
                objRecibo = null;
            }

            return strRetorno;

        }

        #endregion

        #region Codigo Comentareado

        ///// <summary>
        ///// GALD: Para generar impresíon de arqueo..
        ///// </summary>
        ///// <param name="objTicket"></param>
        ///// <returns></returns>
        //public string ImprimirTicketArqueo(TicketImprimir objTicket)
        //{

        //    Ticket objRecibo = new Ticket();
        //    string strRetorno = string.Empty;

        //    try
        //    {
        //        objRecibo.TituloRecibo = objTicket.TituloRecibo;
        //        objRecibo.TituloColumnas = objTicket.TituloColumnas;
        //        objRecibo.ListaArticulos = objTicket.ListaArticulos;
        //        objRecibo.AdicionarFirma(objTicket.PieDePagina);
        //        objRecibo.Usuario = objTicket.Usuario;

        //        objRecibo.TablaDetalle = objTicket.TablaDetalle;
        //        objRecibo.ColumnasMoneda = objTicket.ColumnasMoneda;
        //        objRecibo.ColumnaTotalizar = objTicket.ColumnaTotalizar;

        //        //strRetorno = objRecibo.ImprimirTicketTabla();
        //        strRetorno = objRecibo.ImprimirTicketMasivo();
        //        if (strRetorno.Trim().Length > 0)
        //        {
        //            throw new ArgumentException(strRetorno);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return  (string.Concat("Error en ImprimirTicketArqueo_ServicioImprimir ", ex.Message));
        //    }
        //    finally
        //    {
        //        objRecibo = null;
        //    }

        //    return strRetorno;
        //}

        #endregion
    }
}
