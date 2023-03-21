using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Drawing.Printing;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Data;


namespace CorParques.Transversales.Util
{
    public class TicketFactura
    {
        #region Declaraciones

        private int count;
        private int countTemp;
        private string fontName = "Lucida Console";
        private int fontSize = 9;
        private ArrayList footerLines = new ArrayList();
        private Graphics gfx;
        private ArrayList headerLines = new ArrayList();
        private int imageHeight;
        private ArrayList items = new ArrayList();
        private float leftMargin;
        private string line;
        private int maxChar = 0x23;
        private SolidBrush myBrush = new SolidBrush(Color.Black);
        private Font printFont;
        private ArrayList subHeaderLines = new ArrayList();
        private float topMargin = 3f;
        private ArrayList totales = new ArrayList();
        private float UltimaPosicionY = 0;
        private ArrayList Lineas = new ArrayList();

        private ArrayList LineasHeader = new ArrayList(); //Manuel Ochoa

        private string ticket = "";
        private string parte1, parte2;
        private int max, cort;


        private string strImpresora = string.Empty;
        private string strUsuario = string.Empty;
        private string strNombrePunto = string.Empty;
        private int intNumeroColumnas = 0;
        private string strTituloColumnas = string.Empty;
        private IList<Articulo> objListaArticulos;
        private string strCodigoBarras = string.Empty;
        private string strTituloTicket = string.Empty;
        private string strLogoParque = string.Empty;
        private string strPosicionTitulos;

        private DataTable objTablaDetalle;
        private string strColumnaTotalizar = string.Empty;
        private string strColumnasMoneda = string.Empty;

        private IList<TicketImprimir> objListaTickets;
        private bool blnEsDataTable;
        private string strFirma = string.Empty;
        //Se Adiciona para saber si es Factura o documento GALD1 
        private bool blnDocumento = false;


        #endregion

        #region Constructor

        public TicketFactura()
        {
            strImpresora = ConfigurationManager.AppSettings["NombreImpresora"].ToString();
            strLogoParque = ConfigurationManager.AppSettings["RutaLogoImprimir"].ToString();
        }

        #endregion

        #region Propiedades     

        public int SinGUardar { get; set; }
        public string Usuario
        {
            get { return strUsuario; }
            set { strUsuario = value; }
        }

        public string NombrePunto
        {
            get { return strNombrePunto; }
            set { strNombrePunto = value; }
        }

        private int NumeroColumnas
        {
            get { return intNumeroColumnas; }
            set { intNumeroColumnas = value; }
        }

        public string TituloColumnas
        {
            get { return strTituloColumnas; }
            set { strTituloColumnas = value; }
        }

        public IList<Articulo> ListaArticulos
        {
            get { return objListaArticulos; }
            set { objListaArticulos = value; }
        }

        public string CodigoBarrasProp
        {
            get { return strCodigoBarras; }
            set { strCodigoBarras = value; }
        }

        public string TituloRecibo
        {
            get { return strTituloTicket; }
            set { strTituloTicket = value; }
        }

        public DataTable TablaDetalle
        {
            get { return objTablaDetalle; }
            set { objTablaDetalle = value; }
        }

        /// <summary>
        /// RDSH: Totaliza por el nombre de esta columna.
        /// </summary>
        public string ColumnaTotalizar
        {
            get { return strColumnaTotalizar; }
            set { strColumnaTotalizar = value; }
        }

        /// <summary>
        /// RDSH: Recibe las columnas que se deben tener formato moneda.
        /// </summary>
        public string ColumnasMoneda
        {
            get { return strColumnasMoneda; }
            set { strColumnasMoneda = value; }
        }

        /// <summary>
        /// RDSH: Se crea esta propiedad para manejar una coleccion de ticket para que todos salgan en el mismo recibo en una sola impresión.
        /// </summary>
        public IList<TicketImprimir> ListaTickets
        {
            get { return objListaTickets; }
            set { objListaTickets = value; }
        }

        /// <summary>
        /// RDSH: Indica si el ticket usa un data table para su procesamiento.
        /// </summary>
        public bool EsDataTable
        {
            get { return blnEsDataTable; }
            set { blnEsDataTable = value; }
        }

        public string Firma
        {
            get { return strFirma; }
            set { strFirma = value; }
        }

        public FacturaImprimir _FacturaImprimir { get; set; }

        #endregion

        #region Metodos

        private void GenerarEncabezadoTemp()
        {
            var d = 0;
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagenTemp(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                    d = 191;
                }

                AgregarLineaTemp(TextoCentro("C O R P A R Q U E S"));
                d = 195;
                AgregarLineaTemp(TextoCentro("830008059-1"));
                d = 197;
                AgregarLineaTemp(TextoCentro("CRA. 71D 1 - 14 SUR"));
                d = 199;
                AgregarLineaTemp(TextoCentro("Tel 4142700"));
                d = 201;
                AgregarLineaTemp("");
                d = 203;


                if (this.LineasHeader.Count > 0)
                {
                    d = 206;
                    AgregarMultilineaHeaderTemp();
                    d = 208;
                }
                d = 209;
                AgregarLineaTemp(TextoExtremos("Fecha ", this._FacturaImprimir.Fecha.ToString("dd/MM/yyyy")));
                d = 211;
                AgregarLineaTemp(TextoExtremos("Hora ", this._FacturaImprimir.Fecha.ToString("HH:mm")));
                d = 213;
                //Se agrega para validar si es documento o factura para no crear un metodo nuevo GALD1
                if (!blnDocumento)
                {
                    if (_FacturaImprimir.CodigoFactura.StartsWith("DO"))
                    {
                        d = 219;
                        AgregarLineaTemp(TextoExtremos("RECIBO DONACION No:", this._FacturaImprimir.CodigoFactura));
                        d = 221;
                    }
                    else if (_FacturaImprimir.CodigoFactura.StartsWith("RE"))
                    {
                        d = 225;
                        AgregarLineaTemp(TextoExtremos("RECIBO RECARGA No: ", this._FacturaImprimir.CodigoFactura));
                        d = 227;
                    }
                    else
                    {
                        d = 231;
                        AgregarLineaTemp(TextoExtremos("FACTURA POS No: ", this._FacturaImprimir.CodigoFactura));
                        d = 233;
                    }
                }
                if (_FacturaImprimir.CodigoFactura.StartsWith("DO") || _FacturaImprimir.CodigoFactura.StartsWith("RE"))
                {
                    d = 238;
                    AgregarLineaTemp(TextoExtremos("Identificacion cliente:", this._FacturaImprimir.IdentificacionCliente));
                    d = 240;
                }
                if (_FacturaImprimir.CodigoFactura.StartsWith("RE"))
                {
                    d = 244;
                    AgregarLineaTemp(TextoExtremos("Nombre cliente:", this._FacturaImprimir.NombreCliente));
                    d = 246;
                }

                if (!string.IsNullOrEmpty(this._FacturaImprimir.ConsecutivoNotaCredito))
                {
                    d = 251;
                    AgregarLineaTemp(TextoExtremos("Devolución No: ", this._FacturaImprimir.ConsecutivoNotaCredito));
                    d = 253;
                }

                AgregarLineaTemp("");
                //if (strUsuario.Trim().Length > 0)
                //{
                //    AgregarLinea(TextoIzquierda(strUsuario));
                //}
                if (strNombrePunto.Trim().Length > 0)
                {
                    AgregarLineaTemp(TextoCentro(strNombrePunto), objFontBold);
                    EspacioTemp();
                }

                //AgregarLinea(TextoCentro(strTituloTicket));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat($"Error en AgregarImagen_Ticket 249: {d} ", ex.Message));
            }

        }
        /// <summary>
        /// RDSH: Genera encabezado del recibo.
        /// </summary>
        private void GenerarEncabezado()
        {
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("830008059-1"));
                AgregarLinea(TextoCentro("CRA. 71D 1 - 14 SUR"));
                AgregarLinea(TextoCentro("Tel 4142700"));
                AgregarLinea("");


                if (this.LineasHeader.Count > 0)
                {
                    AgregarMultilineaHeader();
                }
                if (_FacturaImprimir.CodigoFactura != null)
                {
                    if (_FacturaImprimir.CodigoFactura.StartsWith("DO"))
                    {
                        AgregarLinea(TextoCentro("Recibo de caja Donacion"), objFontBold);
                    }
                }
                if (this._FacturaImprimir.BanderaBonoRegalo == true)
                {
                    AgregarLinea(TextoCentro("CANJE BONO REGALO"), objFontBold);
                    AgregarLinea("");
                }
                AgregarLinea(TextoExtremos("Fecha ", this._FacturaImprimir.Fecha.ToString("dd/MM/yyyy")));
                AgregarLinea(TextoExtremos("Hora ", this._FacturaImprimir.Fecha.ToString("HH:mm")));
                //Se agrega para validar si es documento o factura para no crear un metodo nuevo GALD1
                if (!blnDocumento)
                {
                    if (_FacturaImprimir.CodigoFactura.StartsWith("DO"))
                    {
                        AgregarLinea(TextoExtremos("RECIBO DONACION No:", this._FacturaImprimir.CodigoFactura));
                    }
                    else if (_FacturaImprimir.CodigoFactura.StartsWith("RE"))
                    {
                        AgregarLinea(TextoExtremos("RECIBO RECARGA No: ", this._FacturaImprimir.CodigoFactura));
                    }
                    else
                    {
                        if (this._FacturaImprimir.BanderaBonoRegalo == true)
                        {
                            AgregarLinea(TextoExtremos("DOCUMENTO No: ", this._FacturaImprimir.Id_Factura));
                        }
                        else
                        {
                            AgregarLinea(TextoExtremos("FACTURA POS No: ", this._FacturaImprimir.CodigoFactura));
                        }

                    }
                }
                if (_FacturaImprimir.CodigoFactura != null)
                {
                    if (_FacturaImprimir.CodigoFactura.StartsWith("DO") || _FacturaImprimir.CodigoFactura.StartsWith("RE"))
                    {
                        AgregarLinea(TextoExtremos("Identificacion cliente:", this._FacturaImprimir.IdentificacionCliente));
                    }
                    if (_FacturaImprimir.CodigoFactura.StartsWith("RE"))
                    {
                        AgregarLinea(TextoExtremos("Nombre cliente:", this._FacturaImprimir.NombreCliente));
                    }
                }
                if (!string.IsNullOrEmpty(this._FacturaImprimir.ConsecutivoNotaCredito))
                    AgregarLinea(TextoExtremos("Devolución No: ", this._FacturaImprimir.ConsecutivoNotaCredito));

                AgregarLinea("");
                //if (strUsuario.Trim().Length > 0)
                //{
                //    AgregarLinea(TextoIzquierda(strUsuario));
                //}
                if (strNombrePunto.Trim().Length > 0)
                {
                    AgregarLinea(TextoCentro(strNombrePunto), objFontBold);
                    Espacio();
                }

                //AgregarLinea(TextoCentro(strTituloTicket));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 329: ", ex.Message));
            }

        }

        private void GenerarDetalleTemp()
        {
#pragma warning disable CS0219 // La variable 'dblTotal' está asignada pero su valor nunca se usa
            double dblTotal = 0;
#pragma warning restore CS0219 // La variable 'dblTotal' está asignada pero su valor nunca se usa
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            try
            {
                this.strTituloColumnas = "PRODUCTO|CANT|VALOR";
                AgregarLineaTemp(GenerarTitulosColumnas(), objFontBold);

                foreach (string[] objArticulo in this._FacturaImprimir.ListaProductos)
                {
                    AgregarLineaTemp(AgregaArticulos(objArticulo[1], int.Parse(objArticulo[0]), double.Parse(objArticulo[2]), ""), objFont);
                    //if (objArticulo.Precio > 0)
                    //{
                    //    dblTotal = dblTotal + objArticulo.Precio;
                    //}
                }

                //if (dblTotal > 0)
                //{
                //    AgregarLinea(LineasTotales(), objFont);
                //    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                //}
                //AgregarLinea(LineasGuion());

                AgregarLineaTemp("");
                foreach (string[] objPropina in this._FacturaImprimir.Propina)
                {
                    AgregarLineaTemp(AgregaArticulos(objPropina[0], 0, double.Parse(objPropina[1]), ""), objFont);

                }

                AgregarLineaTemp("");
                foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
                {
                    if (!objMetodosPago[0][0].Equals("PUNTOS") && !objMetodosPago[0].Equals("VALOR B") && !objMetodosPago[0].Equals("Consumo B") && !objMetodosPago[0].Equals("SALDO B"))
                    {
                        int cantEspacios = (30 - objMetodosPago[0].Length) / 2;
                        string strEspacio = Utilidades.Replicar(cantEspacios, " ");
                        if (double.Parse(objMetodosPago[1]) < 0)
                        {
                            AgregarLineaTemp(TextoCentro(objMetodosPago[0]), objFontBold);
                        }
                        else
                        {
                            //AgregarLinea(AgregaArticulos(strEspacio + objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                            AgregarLineaTemp(AgregaArticulos(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                        }
                    }
                }
                AgregarLineaTemp("");

                int contadorbonos = 0;
                foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
                {
                    if (objMetodosPago[0].Equals("VALOR B") || objMetodosPago[0].Equals("Consumo B") || objMetodosPago[0].Equals("SALDO B"))
                    {
                        if (contadorbonos == 0)
                        {
                            AgregarLineaTemp(TextoCentro("BONO REGALO"));
                            contadorbonos = 1;
                        }
                        int cantEspacios = (30 - objMetodosPago[0].Length) / 2;
                        string strEspacio = Utilidades.Replicar(cantEspacios, " ");


                        AgregarLineaTemp(AgregaArticulos(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);

                    }
                }




                AgregarLineaTemp("");


                AgregarLineaTemp(TextoCentro("RESUMEN IMPUESTOS"));
                //AgregarLinea(GenerarTitulosColumnas(), objFontBold);
                //AgregarLinea(GenerarTitulosColumnas("PRODUCTO            CANTIDAD      VALOR"), objFontBold);
                this.strTituloColumnas = "TARIFA|BASE|IMPUESTO";
                AgregarLineaTemp(GenerarTitulosColumnas(), objFontBold);

                foreach (string[] objImpuesto in this._FacturaImprimir.Impuestos)
                {
                    AgregarLineaTemp(AgregaArticulosImpuesto(objImpuesto[0], double.Parse(objImpuesto[1]), double.Parse(objImpuesto[2]), "").Trim(), objFont);
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "TicketFactura_GenerarDetalle");
                throw new ArgumentException(string.Concat("Error en TicketFactura_GenerarDetalle: ", ex.Message));
            }


        }

        /// <summary>
        /// RDSH: Genera el detalle de un recibo.
        /// </summary>
        private void GenerarDetalle()
        {
#pragma warning disable CS0219 // La variable 'dblTotal' está asignada pero su valor nunca se usa
            double dblTotal = 0;
#pragma warning restore CS0219 // La variable 'dblTotal' está asignada pero su valor nunca se usa
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            try
            {
                this.strTituloColumnas = "PRODUCTO|CANT|VALOR";
                AgregarLinea(GenerarTitulosColumnas(), objFontBold);

                foreach (string[] objArticulo in this._FacturaImprimir.ListaProductos)
                {
                    AgregarLinea(AgregaArticulos(objArticulo[1], int.Parse(objArticulo[0]), double.Parse(objArticulo[2]), ""), objFont);
                    //if (objArticulo.Precio > 0)
                    //{
                    //    dblTotal = dblTotal + objArticulo.Precio;
                    //}
                }

                //if (dblTotal > 0)
                //{
                //    AgregarLinea(LineasTotales(), objFont);
                //    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                //}
                //AgregarLinea(LineasGuion());

                AgregarLinea("");
                foreach (string[] objPropina in this._FacturaImprimir.Propina)
                {
                    AgregarLinea(AgregaArticulos(objPropina[0], 0, double.Parse(objPropina[1]), ""), objFont);

                }

                AgregarLinea("");
                foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
                {
                    if (!objMetodosPago[0].Equals("PUNTOS") && !objMetodosPago[0].Equals("NUEVO SALDO") && !objMetodosPago[0].Equals("VENCIMIENTO TARJETA") && !objMetodosPago[0].Equals("VALOR B") && !objMetodosPago[0].Equals("Consumo B") && !objMetodosPago[0].Equals("SALDO B"))
                    {
                        int cantEspacios = (30 - objMetodosPago[0].Length) / 2;
                        string strEspacio = Utilidades.Replicar(cantEspacios, " ");
                        if (double.Parse(objMetodosPago[1]) < 0)
                        {
                            AgregarLinea(TextoCentro(objMetodosPago[0]), objFontBold);
                        }
                        else
                        {
                            //AgregarLinea(AgregaArticulos(strEspacio + objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                            AgregarLinea(AgregaArticulos(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                        }
                    }
                }

                AgregarLinea("");

                int contadorbonos = 0;
                foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
                {
                    if (objMetodosPago[0].Equals("VALOR B") || objMetodosPago[0].Equals("Consumo B") || objMetodosPago[0].Equals("SALDO B"))
                    {
                        if (contadorbonos == 0)
                        {
                            AgregarLinea(TextoCentro("BONO REGALO"));
                            contadorbonos = 1;
                        }
                        int cantEspacios = (30 - objMetodosPago[0].Length) / 2;
                        string strEspacio = Utilidades.Replicar(cantEspacios, " ");


                        AgregarLinea(AgregaArticulos(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);

                    }
                }


                AgregarLinea("");

                if (!this._FacturaImprimir.CodigoFactura.StartsWith("DO"))
                {
                    AgregarLinea(TextoCentro("RESUMEN IMPUESTOS"));
                    //AgregarLinea(GenerarTitulosColumnas(), objFontBold);
                    //AgregarLinea(GenerarTitulosColumnas("PRODUCTO            CANTIDAD      VALOR"), objFontBold);
                    this.strTituloColumnas = "TARIFA|BASE|IMPUESTO";
                    AgregarLinea(GenerarTitulosColumnas(), objFontBold);

                    foreach (string[] objImpuesto in this._FacturaImprimir.Impuestos)
                    {
                        AgregarLinea(AgregaArticulosImpuesto(objImpuesto[0], double.Parse(objImpuesto[1]), double.Parse(objImpuesto[2]), "").Trim(), objFont);
                    }
                }
                if (_FacturaImprimir.MetodosPago.Exists(x => x[0].ToUpper().Equals("VENCIMIENTO TARJETA")))
                {
                    if (_FacturaImprimir.MetodosPago.Exists(x => x[0].Equals("PUNTOS")))
                    {
                        AgregarLinea("");
                        AgregarLinea($"Cuentas con {_FacturaImprimir.MetodosPago.First(x => x[0].Equals("PUNTOS"))[1]} puntos disponibles para ser redimidos");
                    }
                    if (_FacturaImprimir.MetodosPago.Exists(x => x[0].Equals("NUEVO SALDO")))
                    {
                        AgregarLinea("");
                        AgregarLinea($"Tu saldo es de {double.Parse(_FacturaImprimir.MetodosPago.First(x => x[0].Equals("NUEVO SALDO"))[1]).ToString("C")}");

                    }
                    if (_FacturaImprimir.MetodosPago.Exists(x => x[0].Equals("VENCIMIENTO TARJETA")))
                    {
                        AgregarLinea("");
                        var fecha = _FacturaImprimir.MetodosPago.First(x => x[0].Equals("VENCIMIENTO TARJETA"))[1];
                        AgregarLinea($"Tu tarjeta vence el {fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "TicketFactura_GenerarDetalle");
                throw new ArgumentException(string.Concat("Error en TicketFactura_GenerarDetalle: ", ex.Message));
            }


        }

        /// <summary>
        /// RDSH: Genera el emcabezado de las columnas del ticket.
        /// </summary>
        private string GenerarTitulosColumnas()
        {
            string strEncabezados = string.Empty;
            string[] strsplit;
            int intEspacioEntreColumnas = 0;

            strsplit = strTituloColumnas.Split('|');

            intNumeroColumnas = strsplit.Length;
            intEspacioEntreColumnas = CalcularEspacios(strTituloColumnas, intNumeroColumnas);

            switch (strsplit.Length)
            {
                case 1:
                    strEncabezados = strsplit[0].ToString();
                    strPosicionTitulos = string.Empty;
                    break;
                case 2:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[1].ToString());
                    strPosicionTitulos = (strsplit[0].ToString().Length + intEspacioEntreColumnas).ToString();
                    break;
                case 3:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar((intEspacioEntreColumnas + 6), " "), strsplit[1].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[2].ToString());
                    strPosicionTitulos = string.Concat((strsplit[0].ToString().Length + (intEspacioEntreColumnas + 6)), "|", (strsplit[1].ToString().Length + intEspacioEntreColumnas));
                    break;
                case 4:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar((intEspacioEntreColumnas + 6), " "), strsplit[1].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[2].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[3].ToString());
                    strPosicionTitulos = string.Concat((strsplit[0].ToString().Length + (intEspacioEntreColumnas + 6)), "|", (strsplit[1].ToString().Length + intEspacioEntreColumnas), "|", (strsplit[2].ToString().Length + intEspacioEntreColumnas));
                    break;
            }

            return strEncabezados + "\n";

        }

        private string GenerarTitulosColumnas(string localTitulos)
        {
            string strEncabezados = string.Empty;
            string[] strsplit;
            int intEspacioEntreColumnas = 0;

            //strsplit = strTituloColumnas.Split('|');
            strsplit = localTitulos.Split('|');

            intNumeroColumnas = strsplit.Length;
            //intEspacioEntreColumnas = CalcularEspacios(strTituloColumnas, intNumeroColumnas);
            intEspacioEntreColumnas = CalcularEspacios(localTitulos, intNumeroColumnas);

            switch (strsplit.Length)
            {
                case 1:
                    strEncabezados = strsplit[0].ToString();
                    strPosicionTitulos = string.Empty;
                    break;
                case 2:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[1].ToString());
                    strPosicionTitulos = (strsplit[0].ToString().Length + intEspacioEntreColumnas).ToString();
                    break;
                case 3:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar((intEspacioEntreColumnas + 6), " "), strsplit[1].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[2].ToString());
                    strPosicionTitulos = string.Concat((strsplit[0].ToString().Length + (intEspacioEntreColumnas + 6)), "|", (strsplit[1].ToString().Length + intEspacioEntreColumnas));
                    break;
                case 4:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar((intEspacioEntreColumnas + 6), " "), strsplit[1].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[2].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[3].ToString());
                    strPosicionTitulos = string.Concat((strsplit[0].ToString().Length + (intEspacioEntreColumnas + 6)), "|", (strsplit[1].ToString().Length + intEspacioEntreColumnas), "|", (strsplit[2].ToString().Length + intEspacioEntreColumnas));
                    break;
            }

            return strEncabezados + "\n";

        }


        public void GenerarCodigoBarrasTemp()
        {

            string strRutaCodigoBarras = string.Empty;

            try
            {
                if (strCodigoBarras.Trim().Length > 0)
                {
                    strRutaCodigoBarras = CodigoBarras.GenerarCodigoDeBarras(strCodigoBarras, 50, 2);

                    if (strRutaCodigoBarras.IndexOf("Error") < 0)
                    {
                        AgregarImagenTemp(Utilidades.RetornarImagen(strRutaCodigoBarras), 42, 15, 18);
                        AgregarLineaTemp("");
                        AgregarLineaTemp("");
                        AgregarLineaTemp(TextoCentro(strCodigoBarras));
                        AgregarLineaTemp(LineasGuion());
                    }

                    AgregarLineaTemp("");
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "TicketFactura_GenerarCodigoDeBarras");
                throw new ArgumentException(string.Concat("Error en GenerarCodigoBarras_Ticket: ", ex.Message));
            }

        }
        /// <summary>
        /// RDSH: Genera la imagen del codigo de barras y lo adjunta al recibo.
        /// </summary>
        public void GenerarCodigoBarras()
        {

            string strRutaCodigoBarras = string.Empty;

            try
            {
                if (strCodigoBarras.Trim().Length > 0)
                {
                    strRutaCodigoBarras = CodigoBarras.GenerarCodigoDeBarras(strCodigoBarras, 50, 2);

                    if (strRutaCodigoBarras.IndexOf("Error") < 0)
                    {
                        AgregarImagen(Utilidades.RetornarImagen(strRutaCodigoBarras), 42, 15, 18);
                        AgregarLinea("");
                        AgregarLinea("");
                        AgregarLinea(TextoCentro(strCodigoBarras));
                        AgregarLinea(LineasGuion());
                    }

                    AgregarLinea("");
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "TicketFactura_GenerarCodigoDeBarras");
                throw new ArgumentException(string.Concat("Error en GenerarCodigoBarras_Ticket: ", ex.Message));
            }

        }

        private void GenerarPiePaginaTemp()
        {
            try
            {
                if (!blnDocumento)
                {
                    AgregarLineaTemp("");
                    AgregarLineaTemp("ATENDIDO POR: " + this._FacturaImprimir.Usuario);
                    AgregarLineaTemp("PUNTO: " + this._FacturaImprimir.Punto);
                    AgregarLineaTemp("");
                    AgregarLineaTemp("");
                }
                if (this.Lineas.Count > 0)
                {
                    AgregarMultilineaTemp();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarPiePagina_Ticket: ", ex.Message));
            }
        }
        /// <summary>
        /// RDSH: Imprime texto al final del recibo.
        /// </summary>
        private void GenerarPiePagina()
        {
            try
            {
                if (!blnDocumento)
                {
                    AgregarLinea("");
                    AgregarLinea("ATENDIDO POR: " + this._FacturaImprimir.Usuario);
                    AgregarLinea("PUNTO: " + this._FacturaImprimir.Punto);
                    AgregarLinea("");
                    AgregarLinea("");
                }
                if (this.Lineas.Count > 0)
                {
                    AgregarMultilinea();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarPiePagina_Ticket: ", ex.Message));
            }
        }

        private int CalcularEspacios(string strTituloColumnas, int intColumnas)
        {

            int intEspacios = 0;
            int intLongitudMaxima = 42;

            try
            {

                intEspacios = (strTituloColumnas.Trim().Length - intColumnas);
                intEspacios = (intLongitudMaxima - intEspacios);
                intEspacios = (intEspacios / intColumnas);
            }
            catch (Exception)
            {
                throw;
            }

            return intEspacios;
        }

        private string AgregaArticulos(string strArticulo, int intCantidad, double dblPrecio, string strOtro)
        {

            string[] strSplit;
            string strLinea = string.Empty;
            strSplit = strPosicionTitulos.Split('|');


            switch (intNumeroColumnas)
            {
                case 1:
                    strLinea = strArticulo;
                    break;
                case 2:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 24);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad);
                    break;
                case 3:
                    if (blnDocumento)
                    {
                        strArticulo = Utilidades.RecortarTexto(strArticulo, 24);
                    }
                    else
                    {
                        strArticulo = Utilidades.RecortarTexto(strArticulo, 20);
                    }
                    if (intCantidad > 0 && dblPrecio > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0 && strOtro.Trim().Length == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), "");
                    }
                    else if (intCantidad == 0 && dblPrecio > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), " ", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad == 0 && dblPrecio == 0 && strOtro.Trim().Length == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), " ", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0 && strOtro.Trim().Length > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }
                    else if (intCantidad == 0 && dblPrecio == 0 && strOtro.Trim().Length > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }
                    else if (intCantidad < 0 && dblPrecio == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }
                    break;
                case 4:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 16);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"), Utilidades.Replicar((int.Parse(strSplit[2].ToString()) - dblPrecio.ToString("C0").Length), " "), strOtro);
                    break;
            }

            return strLinea + "\n";

        }

        private string AgregaArticulosImpuesto(string strArticulo, double intCantidad, double dblPrecio, string strOtro)
        {

            string[] strSplit;
            string strLinea = string.Empty;
            strSplit = strPosicionTitulos.Split('|');


            switch (intNumeroColumnas)
            {
                case 1:
                    strLinea = strArticulo;
                    break;
                case 2:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 24);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad);
                    break;
                case 3:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 20);
                    if (intCantidad > 0 && dblPrecio > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad.ToString("C0"), Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), "");
                    }
                    else if (intCantidad == 0 && dblPrecio > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), "", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad == 0 && dblPrecio == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), "", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    break;
                case 4:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 16);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"), Utilidades.Replicar((int.Parse(strSplit[2].ToString()) - dblPrecio.ToString("C0").Length), " "), strOtro);
                    break;
            }

            return strLinea + "\n";

        }
        private void AgregarImagenTemp(Image objImagen, int intWidth, int intHeight, int intLeft)
        {
            countTemp += 6;
        }

        private void AgregarImagen(Image objImagen, int intWidth, int intHeight, int intLeft)
        {
            if (objImagen != null)
            {
                try
                {
                    UltimaPosicionY = this.YPosition();
                    this.gfx.DrawImage(objImagen, new Rectangle(new Point(intLeft, (int)this.YPosition()), new Size(intWidth, intHeight)));
                    double a = (((double)objImagen.Height) / 58.0) * 8.0;
                    if (this.imageHeight > 0)
                    {
                        this.imageHeight = this.imageHeight + intHeight;
                    }
                    else
                    {
                        this.imageHeight = ((int)Math.Round(a)) + 3;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 834: ", ex.Message));
                }
            }
        }

        private float YPosition()
        {
            float Calculo = 0;
            Calculo = (this.topMargin + ((this.count * this.printFont.GetHeight(this.gfx)) + this.imageHeight));
            //if (UltimaPosicionY > Calculo)
            //{
            //    //Calculo = UltimaPosicionY + (this.count * 2);                           
            //}
            return Calculo;
        }

        private void EspacioTemp()
        {
            this.countTemp++;
        }
        private void Espacio()
        {
            this.line = "";
            this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
            this.count++;
        }
        private void AgregarLineaTemp(string strLinea, Font objFuente = null)
        {
            this.countTemp++;
        }
        private void AgregarLinea(string strLinea, Font objFuente = null)
        {
            this.line = strLinea;
            if (objFuente != null)
            {
                this.gfx.DrawString(this.line, objFuente, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
            }
            else
            {
                this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
            }

            this.count++;
        }

        private void AgregarMultilineaTemp()
        {
            foreach (string str in this.Lineas)
            {
                if (str.Length > this.maxChar)
                {
                    int startIndex = 0;
                    for (int i = str.Length; i > this.maxChar; i -= this.maxChar)
                    {
                        this.line = str;
                        //this.gfx.DrawString(this.line.Substring(startIndex, this.maxChar), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                        //this.count++;
                        AgregarLineaTemp(this.line.Substring(startIndex, this.maxChar));
                        startIndex += this.maxChar;
                    }
                    this.line = str;
                    AgregarLineaTemp(this.line.Substring(startIndex, this.line.Length - startIndex));
                    //this.gfx.DrawString(this.line.Substring(startIndex, this.line.Length - startIndex), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                }
                else
                {
                    this.line = str;
                    //this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                    AgregarLineaTemp(this.line);
                }
            }
            this.leftMargin = 0f;
            this.EspacioTemp();
        }
        private void AgregarMultilinea()
        {
            foreach (string str in this.Lineas)
            {

                if (str.Length > this.maxChar)
                {
                    int startIndex = 0;
                    for (int i = str.Length; i > this.maxChar; i -= this.maxChar)
                    {
                        this.line = str;
                        //this.gfx.DrawString(this.line.Substring(startIndex, this.maxChar), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                        //this.count++;
                        AgregarLinea(this.line.Substring(startIndex, this.maxChar));
                        startIndex += this.maxChar;
                    }
                    this.line = str;
                    AgregarLinea(this.line.Substring(startIndex, this.line.Length - startIndex));
                    //this.gfx.DrawString(this.line.Substring(startIndex, this.line.Length - startIndex), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                }
                else
                {
                    this.line = str;
                    //this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                    AgregarLinea(this.line);
                }
            }
            this.leftMargin = 0f;
            this.Espacio();
        }

        //Manuel Ochoa - Imresion de factura en POS,debe tener un texto en la cabecera
        private void AgregarMultilineaHeader()
        {
            foreach (string str in this.LineasHeader)
            {
                if (str.Length > this.maxChar)
                {
                    int startIndex = 0;
                    for (int i = str.Length; i > this.maxChar; i -= this.maxChar)
                    {
                        this.line = str;
                        //this.gfx.DrawString(this.line.Substring(startIndex, this.maxChar), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                        //this.count++;
                        AgregarLinea(this.line.Substring(startIndex, this.maxChar));
                        startIndex += this.maxChar;
                    }
                    this.line = str;
                    AgregarLinea(this.line.Substring(startIndex, this.line.Length - startIndex));
                    //this.gfx.DrawString(this.line.Substring(startIndex, this.line.Length - startIndex), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                }
                else
                {
                    this.line = str;
                    //this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                    AgregarLinea(this.line);
                }
            }
            this.leftMargin = 0f;
            this.EspacioTemp();
        }
        private void AgregarMultilineaHeaderTemp()
        {
            foreach (string str in this.LineasHeader)
            {
                if (str.Length > this.maxChar)
                {
                    int startIndex = 0;
                    for (int i = str.Length; i > this.maxChar; i -= this.maxChar)
                    {
                        this.line = str;
                        //this.gfx.DrawString(this.line.Substring(startIndex, this.maxChar), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                        //this.count++;
                        AgregarLineaTemp(this.line.Substring(startIndex, this.maxChar));
                        startIndex += this.maxChar;
                    }
                    this.line = str;
                    AgregarLineaTemp(this.line.Substring(startIndex, this.line.Length - startIndex));
                    //this.gfx.DrawString(this.line.Substring(startIndex, this.line.Length - startIndex), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                }
                else
                {
                    this.line = str;
                    //this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                    AgregarLineaTemp(this.line);
                }
            }
            this.leftMargin = 0f;
            this.EspacioTemp();
        }

        private string TextoCentro(string par1)
        {

            ticket = "";
            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);          // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = (int)(40 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********

            return ticket += parte1 + "\n";

        }

        private string TextoIzquierda(string par1)                          // agrega texto a la izquierda
        {

            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);        // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********


            return ticket = parte1 + "\n";
        }

        private string TextoDerecha(string par1)
        {

            ticket = "";
            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);           // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = 40 - par1.Length;                     // obtiene la cantidad de espacios para llegar a 40
            for (int i = 0; i < max; i++)
            {
                ticket += " ";                          // agrega espacios para alinear a la derecha
            }

            return ticket += parte1 + "\n";                    //Agrega el texto

        }

        private string TextoExtremos(string par1, string par2)
        {
            max = par1.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;                             // agrega el primer parametro
            max = par2.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
            }
            else { parte2 = par2; }
            max = 35 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********

            return ticket += parte2 + "\n";                     // agrega el segundo parametro al final

        }

        public bool PrinterExists(string impresora)
        {
            foreach (string str in PrinterSettings.InstalledPrinters)
            {
                if (impresora == str)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// RDSH: Generacion de impresion de ticket. Retorna string con error.
        /// </summary>
        /// <returns></returns>
        public string ImprimirTicket()
        {

            string strRetorno = string.Empty;

            try
            {
                if (PrinterExists(strImpresora))
                {
                    this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
                    PrintDocument document = new PrintDocument
                    {
                        PrinterSettings = { PrinterName = strImpresora }
                    };
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPage);
                    if (ConfigurationManager.AppSettings["alturaImpresion"] != null)
                    {
                        if (ConfigurationManager.AppSettings["alturaImpresion"] == "1")
                        {
                            this.GenerarEncabezadoTemp();
                            this.GenerarDetalleTemp();
                            this.GenerarPiePaginaTemp();
                            this.GenerarCodigoBarrasTemp();
                            this.AgregarLineaTemp("");
                            this.AgregarLineaTemp("");
                            document.DefaultPageSettings.PaperSize = new PaperSize("First custom size", document.DefaultPageSettings.PaperSize.Width + 30, (countTemp * 12));
                        }
                    }
                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error ImprimirTicket: ", ex.Message);
            }

            return strRetorno;

        }

        private void pr_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            //this.GenerarPiePagina();
            this.GenerarEncabezado();
            this.GenerarDetalle();
            this.GenerarPiePagina();
            this.GenerarCodigoBarras();
            this.AgregarLinea("");
            this.AgregarLinea("");

        }

        private string LineasGuion()
        {
            return "----------------------------------------\n";   // agrega lineas separadoras -

        }
        private string LineasAsterisco()
        {
            return "****************************************\n";   // agrega lineas separadoras *            
        }
        private string LineasIgual()
        {
            return "========================================\n";   // agrega lineas separadoras =

        }
        private string LineasTotales()
        {
            return "                          -----------\n"; ;   // agrega lineas de total

        }

        private string AgregaTotales(string par1, double total)
        {
            max = par1.Length;
            if (max > 25)                                 // **********
            {
                cort = max - 25;
                parte1 = par1.Remove(25, cort);          // si es mayor que 25 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;
            parte2 = total.ToString("C0");
            max = 35 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios para poner el valor de moneda al final
            }                                            // **********

            return ticket += parte2 + "\n";

        }

        public void AdicionarContenido(string strLinea)
        {
            this.Lineas.Add(strLinea);
        }

        //Manuel Ochoa - Agregar contenido a la cabecera
        public void AdicionarContenidoHeader(string strLinea)
        {
            this.LineasHeader.Add(strLinea);
        }

        #endregion
        /*
            string strArticulo, 
            int intCantidad, 
            double dblPrecio, 
            string strOtro
        */

        #region NuevosMetodos

        /// <summary>
        /// RDSH Imprime detalle con data table.
        /// </summary>
        /// <returns></returns>
        public string ImprimirTicketTabla()
        {

            string strRetorno = string.Empty;

            try
            {
                if (PrinterExists(strImpresora))
                {
                    this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
                    PrintDocument document = new PrintDocument
                    {
                        PrinterSettings = { PrinterName = strImpresora }
                    };
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageTabla);
                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error ImprimirTicket: ", ex.Message);
            }

            return strRetorno;

        }

        private void pr_PrintPageTabla(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezado();
            this.GenerarDetalleTabla();
            this.GenerarCodigoBarras();
            this.GenerarPiePagina();
        }

        /// <summary>
        /// RDSH: Genera el detalle de un recibo.
        /// </summary>
        private void GenerarDetalleTabla()
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            string strParametros = string.Empty;
            string strValor = string.Empty;


            try
            {
                foreach (DataColumn objDataColumn in objTablaDetalle.Columns)
                {
                    if (strTituloColumnas.Trim().Length == 0)
                    {
                        strTituloColumnas = objDataColumn.Caption;
                    }
                    else
                    {
                        strTituloColumnas = string.Concat(strTituloColumnas, "|", objDataColumn.Caption);
                    }
                }
                AgregarLinea(GenerarTitulosColumnas(), objFont);

                foreach (DataRow objDataRow in objTablaDetalle.Rows)
                {
                    strParametros = string.Empty;
                    foreach (DataColumn objDataColumn in objTablaDetalle.Columns)
                    {

                        if (strColumnasMoneda.IndexOf(objDataColumn.Caption) >= 0)
                        {
                            strValor = Utilidades.FormatoMoneda(double.Parse(objDataRow[objDataColumn.Caption].ToString()));
                        }
                        else
                        {
                            strValor = objDataRow[objDataColumn.Caption].ToString();
                        }

                        if (strColumnaTotalizar.IndexOf(objDataColumn.Caption) >= 0)
                        {
                            dblTotal = dblTotal + double.Parse(objDataRow[objDataColumn.Caption].ToString());
                        }

                        if (strParametros.Trim().Length == 0)
                        {
                            strParametros = strValor;
                        }
                        else
                        {
                            strParametros = string.Concat(strParametros, "|", strValor);
                        }
                    }
                    AgregarLinea(AgregaArticulosTabla(strParametros), objFont);
                }

                if (dblTotal > 0)
                {
                    AgregarLinea(LineasTotales(), objFont);
                    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                }
                AgregarLinea(LineasGuion());
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalleTabla_Ticket: ", ex.Message));
            }

        }

        private string AgregaArticulosTabla(string strParametros)
        {

            string[] strSplit;
            string strLinea = string.Empty;
            strSplit = strParametros.Split('|');
            string strParam1 = string.Empty;
            string strParam2 = string.Empty;
            string strParam3 = string.Empty;
            string strParam4 = string.Empty;
            string[] strSplitTitulos;
            strSplitTitulos = strPosicionTitulos.Split('|');

            switch (intNumeroColumnas)
            {
                case 1:
                    strParam1 = strSplit[0].ToString();
                    strLinea = strParam1;
                    break;
                case 2:
                    strParam1 = strSplit[0].ToString();
                    strParam2 = strSplit[1].ToString();

                    strParam1 = Utilidades.RecortarTexto(strParam1, 24);
                    strLinea = string.Concat(strParam1, Utilidades.Replicar((int.Parse(strSplitTitulos[0].ToString()) - strParam1.Length), " "), strParam2);
                    break;
                case 3:
                    strParam1 = strSplit[0].ToString();
                    strParam2 = strSplit[1].ToString();
                    strParam3 = strSplit[2].ToString();

                    strParam1 = Utilidades.RecortarTexto(strParam1, 20);
                    strLinea = string.Concat(strParam1, Utilidades.Replicar((int.Parse(strSplitTitulos[0].ToString()) - strParam1.Length), " "), strParam2, Utilidades.Replicar(((int.Parse(strSplitTitulos[1].ToString()) - 3) - strParam2.Length), " "), strParam3);

                    break;
                case 4:
                    strParam1 = strSplit[0].ToString();
                    strParam2 = strSplit[1].ToString();
                    strParam3 = strSplit[2].ToString();
                    strParam4 = strSplit[3].ToString();

                    strParam1 = Utilidades.RecortarTexto(strParam1, 16);
                    strLinea = string.Concat(strParam1, Utilidades.Replicar((int.Parse(strSplitTitulos[0].ToString()) - strParam1.Length), " "), strParam2, Utilidades.Replicar(((int.Parse(strSplitTitulos[1].ToString()) - 3) - strParam2.ToString().Length), " "), strParam3, Utilidades.Replicar((int.Parse(strSplitTitulos[2].ToString()) - strParam3.Length), " "), strParam4);
                    break;
            }

            return strLinea + "\n";

        }

        #region ImprimirApertura

        private void GenerarDetalleGrupo()
        {
            double dblTotal = 0;
            string Grupo = "";
            string subGrupo = "";
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            try
            {

                foreach (Articulo objArticulo in objListaArticulos)
                {

                    if (Grupo != objArticulo.Grupo)
                    {
                        if (Grupo != "")
                            AgregarLinea("");

                        if (dblTotal > 0 && objListaArticulos.Count() > 1)
                        {
                            AgregarLinea(LineasTotales(), objFont);
                            AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                            AgregarLinea("");
                            dblTotal = 0;
                        }

                        AgregarLinea(objArticulo.Grupo, objFontBold);
                        if (!(string.IsNullOrEmpty(objArticulo.subGrupo)))
                            AgregarLinea(TextoCentro(objArticulo.subGrupo), objFontBold);

                        AgregarLinea(GenerarTitulosColumnasGrupo(objArticulo), objFont);

                    }
                    else if (subGrupo != objArticulo.subGrupo)
                    {
                        if (dblTotal > 0)
                        {
                            AgregarLinea(LineasTotales(), objFont);
                            AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                            AgregarLinea("");
                            dblTotal = 0;
                        }
                        if (!(string.IsNullOrEmpty(objArticulo.subGrupo)))
                            AgregarLinea(TextoCentro(objArticulo.subGrupo), objFontBold);

                        AgregarLinea(GenerarTitulosColumnasGrupo(objArticulo), objFont);
                    };

                    AgregarLinea(AgregaArticulos(objArticulo.Nombre, objArticulo.Cantidad, objArticulo.Precio, objArticulo.Otro), objFont);
                    if (objArticulo.Precio > 0)
                    {
                        dblTotal = dblTotal + objArticulo.Precio;
                    }

                    if (objListaArticulos.Count() == 1)
                    {
                        dblTotal = objArticulo.Precio;

                        if (dblTotal > 0)
                        {
                            AgregarLinea("");
                            AgregarLinea(LineasTotales(), objFont);
                            AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                            dblTotal = 0;
                        }

                    }

                    Grupo = objArticulo.Grupo;
                    subGrupo = objArticulo.subGrupo;
                }
                AgregarLinea("");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalle_Ticket: ", ex.Message));
            }


        }

        private string GenerarTitulosColumnasGrupo(Articulo _Articulo)
        {
            string strEncabezados = string.Empty;
            string[] strsplit;
            int intEspacioEntreColumnas = 0;

            strsplit = _Articulo.TituloColumnas.Split('|');

            intNumeroColumnas = strsplit.Length;
            intEspacioEntreColumnas = CalcularEspacios(_Articulo.TituloColumnas, intNumeroColumnas);

            switch (strsplit.Length)
            {
                case 1:
                    strEncabezados = strsplit[0].ToString();
                    strPosicionTitulos = string.Empty;
                    break;
                case 2:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[1].ToString());
                    strPosicionTitulos = (strsplit[0].ToString().Length + intEspacioEntreColumnas).ToString();
                    break;
                case 3:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar((intEspacioEntreColumnas + 6), " "), strsplit[1].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[2].ToString());
                    strPosicionTitulos = string.Concat((strsplit[0].ToString().Length + (intEspacioEntreColumnas + 6)), "|", (strsplit[1].ToString().Length + intEspacioEntreColumnas));
                    break;
                case 4:
                    strEncabezados = string.Concat(strsplit[0].ToString(), Utilidades.Replicar((intEspacioEntreColumnas + 6), " "), strsplit[1].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[2].ToString(), Utilidades.Replicar(intEspacioEntreColumnas, " "), strsplit[3].ToString());
                    strPosicionTitulos = string.Concat((strsplit[0].ToString().Length + (intEspacioEntreColumnas + 6)), "|", (strsplit[1].ToString().Length + intEspacioEntreColumnas), "|", (strsplit[2].ToString().Length + intEspacioEntreColumnas));
                    break;
            }

            return strEncabezados + "\n";

        }

        private string LineasContinuas()
        {
            return "_________________________________\n";
        }

        public void AdicionarFirma(string Linea)
        {
            string[] strsplit;
            strsplit = Linea.Split('|');

            foreach (string item in strsplit)
            {
                this.Lineas.Add(item);
                this.Lineas.Add("");
                this.Lineas.Add(LineasContinuas());

            }
        }

        public void Documento(bool _documento)
        {
            this.blnDocumento = _documento;
        }

        public void ImprimirTicketGrupo()
        {
            this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
            PrintDocument document = new PrintDocument
            {
                PrinterSettings = { PrinterName = strImpresora }
            };
            document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageGrupo);
            document.Print();
        }

        private void pr_PrintPageGrupo(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            //this.GenerarPiePagina();
            this.GenerarEncabezado();
            this.GenerarDetalleGrupo();
            this.GenerarPiePagina();
        }

        public void ImprimirTicketGrupoCupo()
        {
            this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
            PrintDocument document = new PrintDocument
            {
                PrinterSettings = { PrinterName = strImpresora }
            };
            document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageCupo);
            document.Print();
        }

        public void ImprimirTicketTarjetaRecargable()
        {
            this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
            PrintDocument document = new PrintDocument
            {
                PrinterSettings = { PrinterName = strImpresora }
            };
            document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageTarjetaRecargable);
            document.Print();
        }

        private void pr_PrintPageTarjetaRecargable(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            //this.GenerarPiePagina();
            this.GenerarEncabezado();
            this.GenerarDetalleTarjetaRecargable();
            //this.GenerarDetalleGrupo();
            this.GenerarPiePagina();
        }

        private void GenerarDetalleTarjetaRecargable()
        {
            AgregarLinea(TextoCentro(strTituloTicket), new Font(this.fontName, 8, FontStyle.Bold));

            AgregarLinea("");
            AgregarLinea("");
            foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
            {
                //AgregarLinea(AgregaArticulos(strEspacio + objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                if (objMetodosPago[0].ToUpper().Equals("TARJETA RECARGABLE"))
                    AgregarLinea(string.Concat("Valor:   ", double.Parse(objMetodosPago[1]).ToString("C0")));

            };
        }

        private void pr_PrintPageCupo(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            //this.GenerarPiePagina();
            this.GenerarEncabezado();
            this.GenerarDetalleCupo();
            //this.GenerarDetalleGrupo();
            this.GenerarPiePagina();
        }

        private void GenerarDetalleCupo()
        {
            AgregarLinea(TextoCentro(strTituloTicket), new Font(this.fontName, 8, FontStyle.Bold));

            AgregarLinea("");
            AgregarLinea("");
            foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
            {
                //AgregarLinea(AgregaArticulos(strEspacio + objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                if (objMetodosPago[0].ToUpper().Equals("TOTAL"))
                    AgregarLinea(string.Concat("Valor:   ", double.Parse(objMetodosPago[1]).ToString("C0")));

            };
        }

        #endregion

        #region ImpresionCierreTaquilla

        /// <summary>
        /// RDSH: Imprime una coleccion de tickets en un solo recibo.
        /// </summary>
        /// <returns></returns>
        public string ImprimirTicketMasivo()
        {

            string strRetorno = string.Empty;

            try
            {
                if (PrinterExists(strImpresora))
                {
                    this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
                    PrintDocument document = new PrintDocument
                    {
                        PrinterSettings = { PrinterName = strImpresora }
                    };
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageMasivo);
                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket-ImprimirTicketMasivo");
                strRetorno = string.Concat("Error ImprimirTicketMasivo: ", ex.Message);
            }

            return strRetorno;

        }

        private void pr_PrintPageMasivo(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezado();
            this.GenerarTicketMasivo();
            this.GenerarCodigoBarras();
            this.GenerarPiePagina();
        }

        /// <summary>
        /// RDSH: Recorre una coleccion de tickets y segun su tipo genera una impresion para cada uno en un solo recibo.
        /// </summary>
        private void GenerarTicketMasivo()
        {
            try
            {
                foreach (TicketImprimir objTicketImprimir in objListaTickets)
                {

                    strCodigoBarras = objTicketImprimir.CodigoBarrasProp;
                    strTituloTicket = objTicketImprimir.TituloRecibo;
                    strTituloColumnas = string.Empty;
                    if (objTicketImprimir.EsDataTable)
                    {
                        GenerarDetalleTablaMasivo(objTicketImprimir);
                    }
                    else
                    {
                        GenerarDetalleMasivo(objTicketImprimir);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(string.Concat("Error en GenerarTicketMasivo_Ticket: ", ex.Message));
            }
        }

        private void GenerarDetalleMasivo(TicketImprimir objTicketImprimir)
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);

            try
            {
                strTituloColumnas = objTicketImprimir.TituloColumnas;
                AgregarLinea(TextoCentro(strTituloTicket), objFontBold);
                AgregarLinea(GenerarTitulosColumnas(), objFont);

                foreach (Articulo objArticulo in objTicketImprimir.ListaArticulos)
                {
                    AgregarLinea(AgregaArticulos(objArticulo.Nombre, objArticulo.Cantidad, objArticulo.Precio, objArticulo.Otro), objFont);
                    if (objArticulo.Precio > 0)
                    {
                        dblTotal = dblTotal + objArticulo.Precio;
                    }
                }
                if (dblTotal > 0)
                {
                    AgregarLinea(LineasTotales(), objFont);
                    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                }
                AgregarLinea(LineasGuion());
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalle_Ticket: ", ex.Message));
            }


        }

        private void GenerarDetalleTablaMasivo(TicketImprimir objTicketImprimir)
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            string strParametros = string.Empty;
            string strValor = string.Empty;


            try
            {

                AgregarLinea(TextoCentro(strTituloTicket), objFontBold);
                objTablaDetalle = objTicketImprimir.TablaDetalle.Copy();

                foreach (DataColumn objDataColumn in objTablaDetalle.Columns)
                {
                    if (strTituloColumnas.Trim().Length == 0)
                    {
                        strTituloColumnas = objDataColumn.Caption;
                    }
                    else
                    {
                        strTituloColumnas = string.Concat(strTituloColumnas, "|", objDataColumn.Caption);
                    }
                }
                AgregarLinea(GenerarTitulosColumnas(), objFont);

                foreach (DataRow objDataRow in objTablaDetalle.Rows)
                {
                    strParametros = string.Empty;
                    foreach (DataColumn objDataColumn in objTablaDetalle.Columns)
                    {

                        if (strColumnasMoneda.IndexOf(objDataColumn.Caption) >= 0)
                        {
                            strValor = Utilidades.FormatoMoneda(double.Parse(objDataRow[objDataColumn.Caption].ToString()));
                        }
                        else
                        {
                            strValor = objDataRow[objDataColumn.Caption].ToString();
                        }

                        if (strColumnaTotalizar.IndexOf(objDataColumn.Caption) >= 0)
                        {
                            dblTotal = dblTotal + double.Parse(objDataRow[objDataColumn.Caption].ToString());
                        }

                        if (strParametros.Trim().Length == 0)
                        {
                            strParametros = strValor;
                        }
                        else
                        {
                            strParametros = string.Concat(strParametros, "|", strValor);
                        }
                    }
                    AgregarLinea(AgregaArticulosTabla(strParametros), objFont);
                }

                if (dblTotal > 0)
                {
                    AgregarLinea(LineasTotales(), objFont);
                    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                }
                AgregarLinea(LineasGuion());
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalleTabla_Ticket: ", ex.Message));
            }

        }

        #endregion

        private string AgregaArticulosVyuComm(string strArticulo, int intCantidad, double dblPrecio, string strOtro)
        {
            int rr = dblPrecio.ToString().Length;
            string[] strSplit;
            string strLinea = string.Empty;
            if (rr >= 7)
            {
                strPosicionTitulos = "21|10";
            }
            else if (rr == 6 || rr == 5)
            {
                strPosicionTitulos = "22|12";
            }
            else if (rr <= 4)
            {
                strPosicionTitulos = "23|13";
            }
            strSplit = strPosicionTitulos.Split('|');

            switch (intNumeroColumnas)
            {
                case 1:
                    strLinea = strArticulo;
                    break;
                case 2:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 24);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad);
                    break;
                case 3:
                    if (blnDocumento)
                    {
                        strArticulo = Utilidades.RecortarTexto(strArticulo, 24);
                    }
                    else
                    {
                        strArticulo = Utilidades.RecortarTexto(strArticulo, 20);
                    }
                    if (intCantidad > 0 && dblPrecio > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0 && strOtro.Trim().Length == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), "");
                    }
                    else if (intCantidad == 0 && dblPrecio > 0 || dblPrecio < 0)
                    {
                        //strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), " ", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), " ", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), "$ ", dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad == 0 && dblPrecio == 0 && strOtro.Trim().Length == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), " ", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0 && strOtro.Trim().Length > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }
                    else if (intCantidad == 0 && dblPrecio == 0 && strOtro.Trim().Length > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }
                    else if (intCantidad < 0 && dblPrecio == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }
                    break;
                case 4:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 16);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"), Utilidades.Replicar((int.Parse(strSplit[2].ToString()) - dblPrecio.ToString("C0").Length), " "), strOtro);
                    break;
            }

            return strLinea.Replace("-$ ", "-") + "\n";

        }
        //Modificacion ImprimirTicketFactura para VyuCoom
        private void GenerarEncabezadoVyuCoom()
        {
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("830008059-1"));
                AgregarLinea(TextoCentro("CRA. 71D 1 - 14 SUR"));
                AgregarLinea(TextoCentro("Tel 4142700"));
                AgregarLinea("");


                if (this.LineasHeader.Count > 0)
                {
                    AgregarMultilineaHeader();
                }
                if (_FacturaImprimir.CodigoFactura != null)
                {
                    if (_FacturaImprimir.CodigoFactura.StartsWith("DO"))
                    {
                        AgregarLinea(TextoCentro("Recibo de caja Donacion"), objFontBold);
                    }
                }
                AgregarLinea(TextoCentro("Recibo de caja"), objFontBold);
                AgregarLinea(TextoExtremos("Fecha ", this._FacturaImprimir.Fecha.ToString("dd/MM/yyyy")));
                AgregarLinea(TextoExtremos("Hora ", this._FacturaImprimir.Fecha.ToString("HH:mm")));
                //Se agrega para validar si es documento o factura para no crear un metodo nuevo GALD1
                if (!blnDocumento)
                {
                    AgregarLinea(TextoExtremos("PEDIDO No: ", this._FacturaImprimir.CodigoFactura));
                    //AgregarLinea(TextoExtremos("Nombre cliente:", this._FacturaImprimir.NombreCliente));
                    //AgregarLinea("");
                    if (this._FacturaImprimir.NombreCliente.Length > 12)
                    {
                        //AgregarLinea(TextoExtremos("Nombre cliente:", " ---------------------- "));
                        //AgregarLinea(TextoExtremos("-", this._FacturaImprimir.NombreCliente));
                        //AgregarLinea(TextoExtremosVyuCoom("-", this._FacturaImprimir.NombreCliente));
                        AgregarLinea(TextoExtremos("Identificacion:", this._FacturaImprimir.IdentificacionCliente));
                    }
                    else
                    {
                        AgregarLinea(TextoExtremos("Nombre cliente:", this._FacturaImprimir.NombreCliente));
                        AgregarLinea(TextoExtremos("Identificacion:", this._FacturaImprimir.IdentificacionCliente));
                    }

                    if (SinGUardar == 1)
                    {
                        AgregarLinea("--- Usuario por crear ---");
                    }
                    SinGUardar = 0;
                }

                //////

                ////

                if (_FacturaImprimir.CodigoFactura != null)
                {
                    if (_FacturaImprimir.CodigoFactura.StartsWith("DO") || _FacturaImprimir.CodigoFactura.StartsWith("RE"))
                    {
                        AgregarLinea(TextoExtremos("Identificacion cliente:", this._FacturaImprimir.IdentificacionCliente));
                    }
                    if (_FacturaImprimir.CodigoFactura.StartsWith("RE"))
                    {
                        AgregarLinea(TextoExtremos("Nombre cliente:", this._FacturaImprimir.NombreCliente));
                    }
                }
                if (!string.IsNullOrEmpty(this._FacturaImprimir.ConsecutivoNotaCredito))
                    AgregarLinea(TextoExtremos("Devolución No: ", this._FacturaImprimir.ConsecutivoNotaCredito));

                AgregarLinea("");
                //if (strUsuario.Trim().Length > 0)
                //{
                //    AgregarLinea(TextoIzquierda(strUsuario));
                //}
                if (strNombrePunto.Trim().Length > 0)
                {
                    AgregarLinea(TextoCentro(strNombrePunto), objFontBold);
                    Espacio();
                }

                //AgregarLinea(TextoCentro(strTituloTicket));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 329: ", ex.Message));
            }
        }
        private void GenerarDetalleVyuCoom()
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            try
            {
                this.strTituloColumnas = "PRODUCTO|CANT|ABONAR";
                AgregarLinea(GenerarTitulosColumnas(), objFontBold);

                foreach (string[] objArticulo in this._FacturaImprimir.ListaProductos)
                {
                    AgregarLinea(AgregaArticulosVyuComm(objArticulo[1], int.Parse(objArticulo[0]), double.Parse(objArticulo[2]), ""), objFont);
                    //if (objArticulo.Precio > 0)
                    //{
                    //    dblTotal = dblTotal + objArticulo.Precio;
                    //}
                }

                //if (dblTotal > 0)
                //{
                //    AgregarLinea(LineasTotales(), objFont);
                //    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                //}
                //AgregarLinea(LineasGuion());

                AgregarLinea("");
                foreach (string[] objPropina in this._FacturaImprimir.Propina)
                {
                    AgregarLinea(AgregaArticulos(objPropina[0], 0, double.Parse(objPropina[1]), ""), objFont);

                }

                AgregarLinea("");
                foreach (string[] objMetodosPago in this._FacturaImprimir.MetodosPago)
                {
                    if (!objMetodosPago[0].Equals("PUNTOS") && !objMetodosPago[0].Equals("NUEVO SALDO") && !objMetodosPago[0].Equals("VENCIMIENTO TARJETA"))
                    {
                        int cantEspacios = (30 - objMetodosPago[0].Length) / 2;
                        string strEspacio = Utilidades.Replicar(cantEspacios, " ");
                        if (double.Parse(objMetodosPago[1]) < 0)
                        {
                            //AgregarLinea(TextoCentro(objMetodosPago[0]), objFontBold);
                            //AgregarLinea(AgregaArticulos(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                            AgregarLinea(AgregaArticulosVyuComm(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                        }
                        else
                        {
                            //AgregarLinea(AgregaArticulos(strEspacio + objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                            AgregarLinea(AgregaArticulos(objMetodosPago[0], 0, double.Parse(objMetodosPago[1]), ""), objFont);
                        }
                    }
                }
                AgregarLinea("");

                if (_FacturaImprimir.MetodosPago.Exists(x => x[0].ToUpper().Equals("VENCIMIENTO TARJETA")))
                {
                    if (_FacturaImprimir.MetodosPago.Exists(x => x[0].Equals("PUNTOS")))
                    {
                        AgregarLinea("");
                        AgregarLinea($"Cuentas con {_FacturaImprimir.MetodosPago.First(x => x[0].Equals("PUNTOS"))[1]} puntos disponibles para ser redimidos");
                    }
                    if (_FacturaImprimir.MetodosPago.Exists(x => x[0].Equals("NUEVO SALDO")))
                    {
                        AgregarLinea("");
                        AgregarLinea($"Tu saldo es de {double.Parse(_FacturaImprimir.MetodosPago.First(x => x[0].Equals("NUEVO SALDO"))[1]).ToString("C")}");

                    }
                    if (_FacturaImprimir.MetodosPago.Exists(x => x[0].Equals("VENCIMIENTO TARJETA")))
                    {
                        AgregarLinea("");
                        var fecha = _FacturaImprimir.MetodosPago.First(x => x[0].Equals("VENCIMIENTO TARJETA"))[1];
                        AgregarLinea($"Tu tarjeta vence el {fecha.Substring(0, 4)}-{fecha.Substring(4, 2)}-{fecha.Substring(6, 2)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "TicketFactura_GenerarDetalle");
                throw new ArgumentException(string.Concat("Error en TicketFactura_GenerarDetalle: ", ex.Message));
            }
        }


        private string TextoExtremosVyuCoom(string par1, string par2)
        {
            max = par1.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;                             // agrega el primer parametro
            max = par2.Length;
            //if (max > 18)                                 // **********
            if (max > 48)                                 // **********
            {
                cort = max - 18;
                parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
            }
            else { parte2 = par2; }
            max = 35 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********

            return ticket += parte2 + "\n";                     // agrega el segundo parametro al final

        }


        //Metodo que utiliza la modificaciones de ticketFactura
        public string ImprimirTicketVyuCoom(int sinGuardar)
        {
            SinGUardar = sinGuardar;

            string strRetorno = string.Empty;

            try
            {
                if (PrinterExists(strImpresora))
                {
                    this.printFont = new Font(this.fontName, (float)this.fontSize, FontStyle.Regular);
                    PrintDocument document = new PrintDocument
                    {
                        PrinterSettings = { PrinterName = strImpresora }
                    };
                    document.PrintPage += new PrintPageEventHandler(this.pr_TicketVyuCoom);

                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error ImprimirTicket: ", ex.Message);
            }
            strRetorno = "Factura realizada correctamente";
            return strRetorno;
            //
        }


        public void pr_TicketVyuCoom(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezadoVyuCoom();
            this.GenerarDetalleVyuCoom();
            this.GenerarPiePagina();
            this.GenerarCodigoBarras();
            this.AgregarLinea("");
            this.AgregarLinea("");
        }
        #endregion
    }
}
