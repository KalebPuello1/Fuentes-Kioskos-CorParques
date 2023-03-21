using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Configuration;
using CorParques.Negocio.Entidades;

namespace CorParques.Transversales.Util
{
    public class Impresion
    {

        #region Declaraciones

        private string ticket = "";
        private string parte1, parte2;
        private string impresora = string.Empty; //"EPSON TM-T88V Receipt"; // nombre exacto de la impresora como esta en el panel de control
        private int max, cort;
        private string strTextoPieDePagina = string.Empty;
        private string strUsuario = string.Empty;
        private string strNombrePunto = string.Empty;
        private int intNumeroColumnas = 0;
        private string strTituloColumnas = string.Empty;
        private IList<Articulo> objListaArticulos;
        private string strCodigoBarras = string.Empty;
        private string strTituloTicket = string.Empty;
        private string strLogoParque = string.Empty;
        private string strPosicionTitulos;

        #endregion

        #region Constructor

        public Impresion()
        {
            impresora = ConfigurationManager.AppSettings["NombreImpresora"].ToString();
            strLogoParque = ConfigurationManager.AppSettings["RutaLogoImprimir"].ToString();
            strTituloTicket = "Factura de venta";
        }

        #endregion

        #region Propiedades

        public string TextoPieDePagina
        {
            get { return strTextoPieDePagina; }
            set { strTextoPieDePagina = value; }
        }

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

        #endregion

        #region Metodos

        private void LineasGuion()
        {
            ticket = "----------------------------------------\n";   // agrega lineas separadoras -
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
        }
        private void LineasAsterisco()
        {
            ticket = "****************************************\n";   // agrega lineas separadoras *
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
        }
        private void LineasIgual()
        {
            ticket = "========================================\n";   // agrega lineas separadoras =
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
        }
        private void LineasTotales()
        {
            ticket = "                             -----------\n"; ;   // agrega lineas de total
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime linea
        }
        private void EncabezadoVenta()
        {
            ticket = "Articulo        Can    P.Unit    Importe\n";   // agrega lineas de  encabezados
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
        }
        private void TextoIzquierda(string par1)                          // agrega texto a la izquierda
        {
            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);        // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1 + "\n";
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
        }
        private void TextoDerecha(string par1)
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
            ticket += parte1 + "\n";                    //Agrega el texto
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
        }
        private void TextoCentro(string par1)
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
            ticket += parte1 + "\n";
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
        }
        private void TextoExtremos(string par1, string par2)
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
            max = 40 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********
            ticket += parte2 + "\n";                     // agrega el segundo parametro al final
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
        }
        private void AgregaTotales(string par1, double total)
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
            max = 40 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios para poner el valor de moneda al final
            }                                            // **********
            ticket += parte2 + "\n";
            RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
        }
        private void AgregaArticulo(string par1, int cant, double precio, double total)
        {
            if (cant.ToString().Length <= 3 && precio.ToString("c").Length <= 10 && total.ToString("c").Length <= 11) // valida que cant precio y total esten dentro de rango
            {
                max = par1.Length;
                if (max > 16)                                 // **********
                {
                    cort = max - 16;
                    parte1 = par1.Remove(16, cort);          // corta a 16 la descripcion del articulo
                }
                else { parte1 = par1; }                      // **********
                ticket = parte1;                             // agrega articulo
                max = (3 - cant.ToString().Length) + (16 - parte1.Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios para poner el valor de cantidad
                }
                ticket += cant.ToString();                   // agrega cantidad
                max = 10 - (precio.ToString("c").Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += precio.ToString("c"); // agrega precio
                max = 11 - (total.ToString().Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios
                }                                            // **********
                ticket += total.ToString("c") + "\n"; // agrega precio
                RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            }
            else
            {
                //MessageBox.Show("Valores fuera de rango");
                RawPrinterHelper.SendStringToPrinter(impresora, "Error, valor fuera de rango\n"); // imprime texto
            }
        }
        private void CortaTicket()
        {
            string corte = "\x1B" + "m";                  // caracteres de corte
            string avance = "\x1B" + "d" + "\x09";        // avanza 9 renglones
            RawPrinterHelper.SendStringToPrinter(impresora, avance); // avanza
            RawPrinterHelper.SendStringToPrinter(impresora, corte); // corta
        }
        private void AbreCajon()
        {
            string cajon0 = "\x1B" + "p" + "\x00" + "\x0F" + "\x96";                  // caracteres de apertura cajon 0
            string cajon1 = "\x1B" + "p" + "\x01" + "\x0F" + "\x96";                 // caracteres de apertura cajon 1
            RawPrinterHelper.SendStringToPrinter(impresora, cajon0); // abre cajon0
            //RawPrinterHelper.SendStringToPrinter(impresora, cajon1); // abre cajon1
        }
        private void SaltoDeLinea()
        {
            RawPrinterHelper.SendStringToPrinter(impresora, "\x1B" + "d" + "\x01");
        }

        /// <summary>
        /// RDSH: Genera encabezado del recibo.
        /// </summary>
        public void GenerarEncabezado()
        {

            try
            {
                if (strLogoParque.Trim().Length > 0)
                {
                    RawPrinterHelper.SendStringToPrinter(impresora, GetLogo(strLogoParque));
                }
                TextoCentro("C O R P A R Q U E S");
                TextoCentro("Nit. 830008059-1");

                if (strUsuario.Trim().Length > 0)
                {
                    TextoIzquierda(strUsuario);
                }
                if (strNombrePunto.Trim().Length > 0)
                {
                    TextoDerecha(strNombrePunto);
                }
                TextoExtremos(string.Concat("Fecha ", Utilidades.ObtenerFechaActual()), string.Concat("Hora ", Utilidades.ObtenerHoraActual()));
                TextoCentro(strTituloTicket);


            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarEncabezado_Impresion: ", ex.Message));
            }

        }

        /// <summary>
        /// RDSH: Imprime texto al final del recibo.
        /// </summary>
        public void GenerarPiePagina()
        {
            try
            {
                if (strTextoPieDePagina.Trim().Length > 0)
                {
                    RawPrinterHelper.SendStringToPrinter(impresora, strTextoPieDePagina);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarPiePagina_Impresion: ", ex.Message));
            }
        }

        /// <summary>
        /// RDSH: Genera el detalle de un recibo.
        /// </summary>
        public void GenerarDetalle()
        {
            double dblTotal = 0;

            try
            {
                GenerarTitulosColumnas();

                foreach (Articulo objArticulo in objListaArticulos)
                {
                    AgregaArticulos(objArticulo.Nombre, objArticulo.Cantidad, objArticulo.Precio, objArticulo.Otro);
                    if (objArticulo.Precio > 0)
                    {
                        dblTotal = dblTotal + objArticulo.Precio;
                    }
                }
                if (dblTotal > 0)
                {
                    LineasTotales();
                    AgregaTotales("Total", dblTotal);
                }
                LineasGuion();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarDetalle_Impresion: ", ex.Message));
            }


        }

        /// <summary>
        /// RDSH: Genera el emcabezado de las columnas del ticket.
        /// </summary>
        private void GenerarTitulosColumnas()
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

            RawPrinterHelper.SendStringToPrinter(impresora, strEncabezados + "\n");

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
                        RawPrinterHelper.SendStringToPrinter(impresora, GetLogo(strRutaCodigoBarras));
                        RawPrinterHelper.SendStringToPrinter(impresora, strCodigoBarras + '\n');
                        LineasGuion();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en GenerarCodigoBarras_Impresion: ", ex.Message));
            }

        }

        /// <summary>
        /// RDSH: Imprime una factura.
        /// </summary>
        public void Imprimir()
        {

            StringBuilder objStringBuilder = new StringBuilder();

            try
            {
                GenerarEncabezado();
                GenerarDetalle();
                GenerarCodigoBarras();
                GenerarPiePagina();
                SaltoDeLinea();
                CortaTicket();                
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en Imprimir_Impresion: ", ex.Message));
            }
            finally
            {
                objStringBuilder = null;
            }
        }

        public string GetLogo(string strNombreImagen)
        {
            string logo = "";
            if (!File.Exists(strNombreImagen))
                return null;
            BitmapData data = GetBitmapData(strNombreImagen);
            BitArray dots = data.Dots;
            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write((char)0x1B);
            bw.Write('@');

            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)24);

            while (offset < data.Height)
            {
                bw.Write((char)0x1B);
                bw.Write('*');         // bit-image mode
                bw.Write((byte)33);    // 24-dot double-density
                bw.Write(width[0]);  // width low byte
                bw.Write(width[1]);  // width high byte

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            // Calculate the location of the pixel we want in the bit array.
                            // It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            // If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }
                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }

                        bw.Write(slice);
                    }
                }
                offset += 24;
                bw.Write((char)0x0A);
            }
            // Restore the line spacing to the default of 30 dots.
            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)30);

            bw.Flush();
            byte[] bytes = stream.ToArray();
            return logo + Encoding.Default.GetString(bytes);
        }

        public BitmapData GetBitmapData(string bmpFileName)
        {
            using (var bitmap = (Bitmap)Bitmap.FromFile(bmpFileName))
            {
                var threshold = 127;
                var index = 0;
                double multiplier = 300; // this depends on your printer model. for Beiyang you should use 1000
                double scale = (double)(multiplier / (double)bitmap.Width);
                int xheight = (int)(bitmap.Height * scale);
                int xwidth = (int)(bitmap.Width * scale);
                var dimensions = xwidth * xheight;
                var dots = new BitArray(dimensions);

                for (var y = 0; y < xheight; y++)
                {
                    for (var x = 0; x < xwidth; x++)
                    {
                        var _x = (int)(x / scale);
                        var _y = (int)(y / scale);
                        var color = bitmap.GetPixel(_x, _y);
                        var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                        dots[index] = (luminance < threshold);
                        index++;
                    }
                }

                return new BitmapData()
                {
                    Dots = dots,
                    Height = (int)(bitmap.Height * scale),
                    Width = (int)(bitmap.Width * scale)
                };
            }
        }

        private void AgregaArticulos(string strArticulo, int intCantidad, double dblPrecio, string strOtro)
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
                    if (intCantidad > 0)
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }
                    else
                    {
                        strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), "", Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"));
                    }

                    
                    break;
                case 4:
                    strArticulo = Utilidades.RecortarTexto(strArticulo, 16);
                    strLinea = string.Concat(strArticulo, Utilidades.Replicar((int.Parse(strSplit[0].ToString()) - strArticulo.Length), " "), intCantidad, Utilidades.Replicar(((int.Parse(strSplit[1].ToString()) - 3) - intCantidad.ToString().Length), " "), dblPrecio.ToString("C0"), Utilidades.Replicar((int.Parse(strSplit[2].ToString()) - dblPrecio.ToString("C0").Length), " "), strOtro);
                    break;
            }


            if (strLinea.Trim().Length > 0)
                RawPrinterHelper.SendStringToPrinter(impresora, string.Concat(strLinea, "\n"));



            //if (cant.ToString().Length <= 3 && precio.ToString("c").Length <= 10 && otro.Length <= 11) // valida que cant precio y total esten dentro de rango
            //{
            //    max = par1.Length;
            //    if (max > 16)                                 // **********
            //    {
            //        cort = max - 16;
            //        parte1 = par1.Remove(16, cort);          // corta a 16 la descripcion del articulo
            //    }
            //    else { parte1 = par1; }                      // **********
            //    ticket = parte1;                             // agrega articulo
            //    max = (3 - cant.ToString().Length) + (16 - parte1.Length);
            //    for (int i = 0; i < max; i++)                // **********
            //    {
            //        ticket += " ";                           // Agrega espacios para poner el valor de cantidad
            //    }
            //    ticket += cant.ToString();                   // agrega cantidad
            //    max = 10 - (precio.ToString("c").Length);
            //    for (int i = 0; i < max; i++)                // **********
            //    {
            //        ticket += " ";                           // Agrega espacios
            //    }                                            // **********
            //    if(precio > 0)
            //        ticket += precio.ToString("c"); // agrega precio
            //    max = 11 - (otro.Length);
            //    for (int i = 0; i < max; i++)                // **********
            //    {
            //        ticket += " ";                           // Agrega espacios
            //    }                                            // **********
            //    ticket += otro  + "\n"; // agrega precio
            //    RawPrinterHelper.SendStringToPrinter(impresora, ticket); // imprime texto
            //}
            //else
            //{
            //    //MessageBox.Show("Valores fuera de rango");
            //    RawPrinterHelper.SendStringToPrinter(impresora, "Error, valor fuera de rango\n"); // imprime texto
            //}
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

        #endregion

    }

    public class BitmapData
    {
        public BitArray Dots
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
    }

}
