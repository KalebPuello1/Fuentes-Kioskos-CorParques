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
    public class Ticket
    {

        #region Declaraciones

        private int count;
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

        private string ticket = "";
        private string parte1, parte2;
        private int max, cort;

        private int idInterno = 0;
        private string strImpresora = string.Empty;
        private string strUsuario = string.Empty;
        private string strNombrePunto = string.Empty;
        private int intNumeroColumnas = 0;
        private string strTituloColumnas = string.Empty;
        private IList<Articulo> objListaArticulos;
        private string strCodigoBarras = string.Empty;
        private string strTituloTicket = string.Empty;
        private string strDetallePtoEntrega = string.Empty;
        private string strLogoParque = string.Empty;
        private string strPosicionTitulos;

        private DataTable objTablaDetalle;
        private string strColumnaTotalizar = string.Empty;
        private string strColumnasMoneda = string.Empty;

        private IList<TicketImprimir> objListaTickets;
        private bool blnEsDataTable;
        private string strFirma = string.Empty;

        private ArrayList LineasHeader = new ArrayList(); //Manuel Ochoa
        string strLineaParqueadero = string.Empty; //GALD1


        #endregion

        #region Constructor

        public Ticket()
        {
            strImpresora = ConfigurationManager.AppSettings["NombreImpresora"].ToString();
            strLogoParque = ConfigurationManager.AppSettings["RutaLogoImprimir"].ToString();
        }

        #endregion

        #region Propiedades     

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

        public string DetallePtoEntrega
        {
            get { return strDetallePtoEntrega; }
            set { strDetallePtoEntrega = value; }
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

        public string PlacaParqueadero { get; set; }

        public string LineaParqueadero
        {
            get { return strLineaParqueadero; }
            set { strLineaParqueadero = value; }
        }

        public int IdInterno { get => idInterno; set => idInterno = value; }


        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Genera encabezado del recibo.
        /// </summary>
        private void GenerarEncabezado()
        {

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("Nit. 830008059-1"));

                if (strUsuario.Trim().Length > 0)
                {
                    AgregarLinea(TextoIzquierda(strUsuario));
                }
                if (strNombrePunto.Trim().Length > 0)
                {
                    AgregarLinea(TextoDerecha(strNombrePunto));
                }
                AgregarLinea(TextoExtremos(string.Concat("Fecha ", Utilidades.ObtenerFechaActual()), string.Concat("Hora ", Utilidades.ObtenerHoraActual())));
                AgregarLinea(TextoCentro(strTituloTicket),new Font(this.fontName, 8, FontStyle.Bold));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 216: ", ex.Message));
            }

        }

        /// <summary>
        /// RDSH: Genera el detalle de un recibo.
        /// </summary>
        private void GenerarDetalle()
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            try
            {
                AgregarLinea(GenerarTitulosColumnas(), objFont);

                foreach (Articulo objArticulo in objListaArticulos)
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
                if (strDetallePtoEntrega.Trim().Length > 0)
                {
                    AgregarLinea(TextoCentro("Pto Entrega:" + strDetallePtoEntrega));
                }
              
                AgregarLinea("");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalle_Ticket: ", ex.Message));
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
                        AgregarLinea("");
                        AgregarLinea(TextoCentro(strCodigoBarras));                        
                        //AgregarLinea(LineasGuion());
                        Utilidades.LimpiarTempCodigosBarra(strRutaCodigoBarras);
                    }
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket_GenerarCodigoDeBarras");
                throw new ArgumentException(string.Concat("Error en GenerarCodigoBarras_Ticket: ", ex.Message));
            }

        }
        
        /// <summary>
        /// RDSH: Imprime texto al final del recibo.
        /// </summary>
        private void GenerarPiePagina(Font objFuente = null)
        {
            try
            {
                if (this.Lineas.Count > 0)
                {
                    AgregarMultilinea(objFuente);
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

        private string AgregaArticulos(string strArticulo, int intCantidad, double dblPrecio, string strOtro,bool boleteria=false)
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
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), boleteria ? dblPrecio.ToString() : dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0 && strOtro.Trim().Length == 0)
                    {                        
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), "");
                    }
                    else if (intCantidad == 0 && dblPrecio > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), "", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), boleteria ? dblPrecio.ToString() : dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad == 0 && dblPrecio == 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), " ", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), boleteria ? dblPrecio.ToString(): dblPrecio.ToString("C0"));
                    }
                    else if (intCantidad > 0 && dblPrecio == 0 && strOtro.Trim().Length > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - (intCantidad.ToString().Length - 3)), " "), strOtro);
                    }

                    if (boleteria)
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
                    throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 438: ", ex.Message));
                }
            }
        }

        private float YPosition()
        {
            float Calculo = 0;
            Calculo = (this.topMargin + ((this.count * this.printFont.GetHeight(this.gfx)) + this.imageHeight));
            //if (UltimaPosicionY > Calculo)
            //{
            //    Calculo = UltimaPosicionY + (this.count * 2);
            //}
            return Calculo;
        }

        private void Espacio()
        {
            this.line = "";
            this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
            this.count++;
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

        private void AgregarMultilinea(Font objFuente = null)
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
                        AgregarLinea(this.line.Substring(startIndex, this.maxChar), objFuente);
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
                    if(ConfigurationManager.AppSettings["alturaImpresion"]!=null)
                        if (ConfigurationManager.AppSettings["alturaImpresion"] == "1")
                            document.DefaultPageSettings.PaperSize = new PaperSize("First custom size", document.DefaultPageSettings.PaperSize.Width + 30, 420);

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

        /// <summary>
        /// RDSH: Generacion de impresion de ticket. Retorna string con error.
        /// </summary>
        /// <returns></returns>
        public string ImprimirTicketParqueadero()
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
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageParqueadero);
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

        private void pr_PrintPageParqueadero(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezadoParqueadero();
            //this.GenerarDetalle();
            //this.GenerarCodigoBarras();   
            float texto = 7.5F ;
            Font printFont = new Font(this.fontName, texto, FontStyle.Regular);
            this.GenerarPiePaginaParqueadero(printFont);            
            this.GenerarCodigoBarrasParqueadero();
            this.AgregarCodigoBarrasParqueadero();
            AgregarLinea(LineasGuion());

        }

        /// <summary>
        /// RDSH: Genera encabezado del recibo.
        /// </summary>
        private void GenerarEncabezadoParqueadero()
        {

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 40, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("Nit. 830008059-1"));
                AgregarLinea(TextoCentro("VISITENOS EN"));
                AgregarLinea(TextoCentro("WWW.MUNDOAVENTURA.COM.CO")); 
                AgregarLinea(TextoCentro("CRA. 71D 1 - 14 SUR"));                
                AgregarLinea(TextoCentro("Tel 4142700"));
                AgregarLinea("");

                if (strUsuario.Trim().Length > 0)
                {
                    AgregarLinea(TextoIzquierda(strUsuario));
                }
                if (strNombrePunto.Trim().Length > 0)
                {
                    AgregarLinea(TextoDerecha(strNombrePunto));
                }
                AgregarLinea("");
                AgregarLinea(("PLACA: " + PlacaParqueadero));
                AgregarLinea(("CODIGO APP: " + IdInterno));
                AgregarLinea(("FECHA: " + Utilidades.ObtenerFechaActual()));
                AgregarLinea(("HORA DE INGRESO: " + Utilidades.ObtenerHoraActual()));
                AgregarLinea("");
                AgregarLinea(TextoCentro("CONDICIONES DE PRESTACION DEL SERVICIO"), new Font(this.fontName, 8, FontStyle.Bold));
                //AgregarLinea(TextoExtremos(string.Concat("Fecha ", Utilidades.ObtenerFechaActual()), string.Concat("Hora ", Utilidades.ObtenerHoraActual())));                
                //AgregarLinea(TextoCentro(strTituloTicket), new Font(this.fontName, 8, FontStyle.Bold));               
                
                if (this.LineaParqueadero.Length > 0)
                {
                    AdicionarContenidoParquedero();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 741: ", ex.Message));
            }

        }


        private void pr_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            //this.GenerarPiePagina();
            this.GenerarEncabezado();
            this.GenerarDetalle();
            this.GenerarCodigoBarras();
            AgregarLinea(LineasGuion());
            this.GenerarPiePagina();
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
            string[] strSplit;
            string[] stringSeparators = new string[] { "\r\n" };

            try
            {
                strSplit = strLinea.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < strSplit.Length; i++)
                {
                    this.Lineas.Add(strSplit[i].ToString());
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket_AdicionarContenido");
                this.Lineas.Add(strLinea);
            }
            
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
                        //AgregarLinea(LineasGuion());

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
                            AgregarLinea("");
                            AgregarLinea(LineasTotales(), objFont);                            
                            AgregarLinea(AgregaTotales("Total", dblTotal), objFont);
                            dblTotal = 0;
                            AgregarLinea("");
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

                    Grupo = objArticulo.Grupo;
                    subGrupo = objArticulo.subGrupo;
                }

                if (dblTotal > 0)
                {
                    AgregarLinea("");
                    AgregarLinea(LineasTotales(), objFont);
                    AgregarLinea(AgregaTotales("Total", dblTotal), objFont);                    
                    dblTotal = 0;
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

            this.Lineas.Add("");
            foreach (string item in strsplit)
            {
                this.Lineas.Add(item);
                this.Lineas.Add("");
                this.Lineas.Add(LineasContinuas());
            }
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
            this.GenerarEncabezadoGrupo();
            this.GenerarDetalleGrupo();
            this.GenerarPiePagina();
        }

        private void GenerarEncabezadoGrupo()
        {

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

                AgregarLinea(TextoExtremos("Fecha ", System.DateTime.Now.ToString("dd/MM/yyyy")));
                AgregarLinea(TextoExtremos("Hora ", System.DateTime.Now.ToString("HH:mm")));
                //Se agrega para validar si es documento o factura para no crear un metodo nuevo GALD1

                AgregarLinea("");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 1186: ", ex.Message));
            }

        }

        //Manuel Ochoa - Agregar contenido a la cabecera
        public void AdicionarContenidoHeader(string strLinea)
        {
            this.LineasHeader.Add(strLinea);
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
            this.Espacio();
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

        public void GenerarCodigoBarrasParqueadero()
        {

            string strRutaCodigoBarras = string.Empty;

            try
            {
                if (strCodigoBarras.Trim().Length > 0)
                {
                    strRutaCodigoBarras = CodigoBarras.GenerarCodigoDeBarras(strCodigoBarras, 50, 2);

                    if (strRutaCodigoBarras.IndexOf("Error") < 0)
                    {
                        
                        AgregarLinea("  ");
                        //AgregarLinea("");
                        AgregarImagen(Utilidades.RetornarImagen(strRutaCodigoBarras), 42, 15, 18);                        
                        //AgregarLinea(TextoCentro(strCodigoBarras));                        
                        Utilidades.LimpiarTempCodigosBarra(strRutaCodigoBarras);
                    }
                    else {
                        Utilidades.RegistrarError(new Exception("Genero error el codigo de barras"), "Genero error el codigo de barras");
                    }
                }
                else
                {
                    Utilidades.RegistrarError(new Exception("No hay codigo de barras"), "No hay codigo de barras");
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket_AdicionarContenido");
                throw new ArgumentException(string.Concat("Error en GenerarCodigoBarras_Ticket: ", ex.Message));
            }

        }

        public void AgregarCodigoBarrasParqueadero()
        {
            string strRutaCodigoBarras = string.Empty;

            try
            {
                if (strCodigoBarras.Trim().Length > 0)
                {
                    //AgregarLinea("");
                    //AgregarLinea("");
                    //AgregarLinea("");
                    //AgregarLinea("");
                    AgregarLinea("");
                    AgregarLinea("");
                    AgregarLinea(TextoCentro(strCodigoBarras));
                }
                else
                {
                    Utilidades.RegistrarError(new Exception("No genero codigo de barras"), "No genero para agregar codigo de barras");
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Genero error el codigo de barras");
                throw new ArgumentException(string.Concat("Error en AgregarCodigoBarrasParqueadero_Ticket: ", ex.Message));
            }
        }

        private void AdicionarContenidoParquedero()
        {
            string[] strSplit;
            string[] stringSeparators = new string[] { "\r\n" };

            try
            {
                strSplit = LineaParqueadero.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < strSplit.Length; i++)
                {
                    this.Lineas.Add(strSplit[i].ToString());
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket_AdicionarContenido");
                this.Lineas.Add(LineaParqueadero);
            }

        }

        private void AgregarMultilineaParqueadero(int maxCharacter, Font objFuente = null)
        {
            foreach (string str in this.Lineas)
            {
                if (str.Length > maxCharacter)
                {
                    int startIndex = 0;
                    for (int i = str.Length; i > maxCharacter; i -= maxCharacter)
                    {
                        this.line = str;
                        //this.gfx.DrawString(this.line.Substring(startIndex, this.maxChar), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                        //this.count++;
                        AgregarLinea(this.line.Substring(startIndex, maxCharacter).Trim(), objFuente);
                        startIndex += maxCharacter;
                    }
                    this.line = str;
                    AgregarLinea(this.line.Substring(startIndex, this.line.Length - startIndex).Trim(), objFuente);
                    //this.gfx.DrawString(this.line.Substring(startIndex, this.line.Length - startIndex), this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                }
                else
                {
                    this.line = str;
                    //this.gfx.DrawString(this.line, this.printFont, this.myBrush, this.leftMargin, this.YPosition(), new StringFormat());
                    //this.count++;
                    AgregarLinea(this.line.Trim(), objFuente);
                }
            }
            this.leftMargin = 0f;
            this.Espacio();
        }

        private void GenerarPiePaginaParqueadero(Font objFuente = null)
        {

            try
            {
                if (this.Lineas.Count > 0)
                {
                    int maxCharacter = 0x2B; //this.maxChar + int.Parse((this.fontSize - objFuente.Size).ToString());
                    AgregarMultilineaParqueadero(maxCharacter, objFuente);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarPiePagina_Ticket: ", ex.Message));
            }
        }




        #endregion

        #region Ticket Cortesias

        public string ImprimirTicketCortesias()
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
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageCortesias);
                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error ImprimirTicketCortesias: ", ex.Message);
            }

            return strRetorno;

        }

        private void pr_PrintPageCortesias(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;           
            this.GenerarEncabezadoCortesias();            
            this.GenerarCodigoBarrasCortesias();
            this.GenerarPiePagina();
        }

        private void GenerarEncabezadoCortesias()
        {

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("Nit. 830008059-1"));
                Espacio();
                AgregarLinea(TextoCentro(strTituloTicket), new Font(this.fontName, 8, FontStyle.Bold));
                Espacio();
                AgregarLinea(TextoExtremos(string.Concat("Fecha ", Utilidades.ObtenerFechaActual()), string.Concat("Hora ", Utilidades.ObtenerHoraActual())));
                Espacio();
                AgregarLinea(string.Concat("Valido para: ", Utilidades.ObtenerFechaActual(), "    Usos: 1"));
                AgregarLinea(string.Concat("Atendido por: ", strUsuario));
                AgregarLinea(string.Concat("Atracción: ", objListaArticulos[0].Nombre));
                Espacio();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 1633: ", ex.Message));
            }

        }

        public void GenerarCodigoBarrasCortesias()
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
                        Espacio();
                        AgregarLinea(TextoCentro(strCodigoBarras));
                        Espacio();                        
                        Utilidades.LimpiarTempCodigosBarra(strRutaCodigoBarras);
                    }
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket_GenerarCodigoDeBarrasCortesias");
                throw new ArgumentException(string.Concat("Error en GenerarCodigoDeBarrasCortesias_Ticket: ", ex.Message));
            }

        }

        #endregion

        #region Ticket Atracciones y Destrezas

        public string ImprimirTicketAtraccionesDestrezas()
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
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageAtraccionesDestrezas);
                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error ImprimirTicketAtraccionesDestrezas: ", ex.Message);
            }

            return strRetorno;

        }

        private void pr_PrintPageAtraccionesDestrezas(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezadoAtraccionesDestrezas();
            this.GenerarCodigoBarrasCortesias();
            this.GenerarPiePagina();
        }

        private void GenerarEncabezadoAtraccionesDestrezas()
        {

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("Nit. 830008059-1"));
                Espacio();
                AgregarLinea(TextoCentro(strTituloTicket), new Font(this.fontName, 8, FontStyle.Bold));
                Espacio();
                AgregarLinea(TextoExtremos(string.Concat("Fecha ", Utilidades.ObtenerFechaActual()), string.Concat("Hora ", Utilidades.ObtenerHoraActual())));
                Espacio();
                AgregarLinea(string.Concat("Valido para: ", Utilidades.ObtenerFechaActual(), "    Usos: ", objListaArticulos[0].Cantidad));
                //AgregarLinea(string.Concat("Atendido por: ", strUsuario));
                AgregarLinea(string.Concat("Atracción: ", objListaArticulos[0].Nombre));
                AgregarLinea(string.Concat("Total: ", objListaArticulos[0].Precio.ToString("C0")));
                Espacio();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 1738: ", ex.Message));
            }

        }



        #endregion

        #region Ticket Arqueo

        /// <summary>
        /// RDSH: Imprime una coleccion de tickets en un solo recibo.
        /// </summary>
        /// <returns></returns>
        public string ImprimirTicketArqueo()
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
                    document.PrintPage += new PrintPageEventHandler(this.pr_PrintPageArqueo);
                    document.Print();
                }
                else
                {
                    strRetorno = "Error: No se ha configurado la impresora.";
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Ticket-ImprimirTicketArqueo");
                strRetorno = string.Concat("Error ImprimirTicketArqueo: ", ex.Message);
            }

            return strRetorno;

        }

        private void pr_PrintPageArqueo(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezado();
            this.GenerarTicketArqueo();            
            this.GenerarPiePagina();
        }

        /// <summary>
        /// RDSH: Recorre una coleccion de tickets y segun su tipo genera una impresion para cada uno en un solo recibo.
        /// </summary>
        private void GenerarTicketArqueo()
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
                        GenerarDetalleTablaArqueo(objTicketImprimir);
                    }
                    else
                    {
                        GenerarDetalleArqueo(objTicketImprimir);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(string.Concat("Error en GenerarTicketMasivo_Ticket: ", ex.Message));
            }
        }

        /// <summary>
        /// RDSH: Para impresion de la boleteria.
        /// </summary>
        /// <param name="objTicketImprimir"></param>
        private void GenerarDetalleArqueo(TicketImprimir objTicketImprimir)
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);

            try
            {
                strTituloColumnas = objTicketImprimir.TituloColumnas;
                AgregarLinea(strTituloTicket, objFontBold);
                AgregarLinea(GenerarTitulosColumnas(), objFontBold);

                foreach (Articulo objArticulo in objTicketImprimir.ListaArticulos)
                {
                    AgregarLinea(AgregaArticulos(objArticulo.Nombre, objArticulo.Cantidad, objArticulo.Precio, objArticulo.Otro,objArticulo.Boleteria), objFont);
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

        /// <summary>
        /// RDSH: Para impresion del dinero.
        /// </summary>
        /// <param name="objTicketImprimir"></param>
        private void GenerarDetalleTablaArqueo(TicketImprimir objTicketImprimir)
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);
            string strParametros = string.Empty;
            string strValor = string.Empty;


            try
            {

                AgregarLinea(strTituloTicket, objFontBold);
                Espacio();
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
                AgregarLinea(GenerarTitulosColumnas(), objFontBold);

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

        public string ImprimirTicketCambioBoleta(/*TicketImprimir objTicket */)
        {
            //SinGUardar = sinGuardar;

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
                    document.PrintPage += new PrintPageEventHandler(this.pr_TicketCambioBoleta);

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

        public void pr_TicketCambioBoleta(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            this.gfx = e.Graphics;
            this.GenerarEncabezadoCambioBoleta();
            this.GenerarDetalleCambioBoleta();
            this.GenerarPiePaginaCambioboleta();
            this.GenerarCodigoBarras();
            this.AgregarLinea("");
            this.AgregarLinea("");
        }

        private void GenerarPiePaginaCambioboleta()
        {
            try
            {
                {
                    AgregarLinea("");
                    AgregarLinea("ATENDIDO POR: " + this.strUsuario);
                    AgregarLinea("PUNTO: " + this.strNombrePunto);
                    AgregarLinea("");
                    AgregarLinea("");
                    AgregarLinea("FIRMA: ________________________________");
                    AgregarLinea("");
                    AgregarLinea("CEDULA:________________________________");
                    AgregarLinea("");
                }
                if (this.Lineas.Count > 0)
                {
                    AgregarMultilinea();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarCambioboleta_Ticket: ", ex.Message));
            }
        }

        private void GenerarEncabezadoCambioBoleta()
        {
            Font objFontBold = new Font(this.fontName, 8, FontStyle.Bold);

            try
            {

                if (strLogoParque.Trim().Length > 0)
                {
                    AgregarImagen(Utilidades.RetornarImagen(strLogoParque), 70, 20, 1);
                }

                AgregarLinea(TextoCentro("C O R P A R Q U E S"));
                AgregarLinea(TextoCentro("Nit. 830008059-1"));
                AgregarLinea(TextoCentro("CRA. 71D 1 - 14 SUR"));
                AgregarLinea(TextoCentro("Tel 4142700"));
                AgregarLinea("");

                //if (strUsuario.Trim().Length > 0)
                //{
                //    AgregarLinea(TextoIzquierda(strUsuario));
                //}
                //if (strNombrePunto.Trim().Length > 0)
                //{
                //    AgregarLinea(TextoDerecha(strNombrePunto));
                //}
                AgregarLinea(TextoCentro(strTituloTicket), new Font(this.fontName, 8, FontStyle.Bold));
                AgregarLinea(TextoExtremos(string.Concat("Fecha ", Utilidades.ObtenerFechaActual()), string.Concat("Hora ", Utilidades.ObtenerHoraActual())));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en AgregarImagen_Ticket 216: ", ex.Message));
            }

        }

        private void GenerarDetalleCambioBoleta()
        {
            double dblTotal = 0;
            Font objFont = new Font(this.fontName, 8, FontStyle.Regular);
            try
            {
                AgregarLinea("");
                AgregarLinea(GenerarTitulosColumnas(), objFont);
                foreach (Articulo objArticulo in objListaArticulos)
                {
                    AgregarLinea(AgregaArticulosCambioBoleta(objArticulo.Nombre, objArticulo.NumTicket, objArticulo.NumTicket, objArticulo.Otro), objFont);
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
                AgregarLinea("");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalle_Ticket: ", ex.Message));
            }
        }

        private string AgregaArticulosCambioBoleta(string strArticulo, long intCantidad, long dblPrecio, string strOtro)
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
                    /*if (blnDocumento)
                    {
                        strArticulo = Utilidades.RecortarTexto(strArticulo, 24);
                    }
                    else
                    {
                        strArticulo = Utilidades.RecortarTexto(strArticulo, 20);
                    }*/
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

        private string TextoExtremosCambioBoleta(string par1, string par2)
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

        #endregion
    }
}

