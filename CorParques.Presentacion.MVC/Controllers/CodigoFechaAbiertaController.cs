using CorParques.Negocio.Entidades;
using System.Web.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System.IO;
using CorParques.Transversales.Util;
/// <summary>
/// Trae de mails
/// </summary>
using System.Configuration;
using System.Net.Mail;
/// <summary>
/// Reportes
/// </summary>
using ClosedXML.Excel;
using Zen.Barcode;
using System.Linq;
using static CorParques.Transversales.Util.Enumerador;


namespace CorParques.Presentacion.MVC.Controllers
{

    public class CodigoFechaAbiertaController : ControladorBase
    {
        #region Properties
        public static string location = "";
        //Este dato es para traer una lista de datos 
        public static string Buscar = "";
        //Esta dato es para poder eliminar el archivo excel
        public static string rutaEliminar;
        //Este parametro es para eliminar las imagenes en codigo de barras
        public static List<string> rutaElimina = new List<string>();
        //Este parametro es para guardar las direciones de los codigos de barras
        public static List<string> codSap;
        //Este parametro es para comparar los datos que se piden con los datos que se van a imprimir en el excel, 
        public static IEnumerable<CodigoFechaAbierta> codExistente;
        #endregion

        #region Constructor
        public CodigoFechaAbiertaController()
        {


        }
        #endregion

        #region metodos

        /// <summary>
        /// Levanta la pagina principal, puede recibir datos de entrada
        /// </summary>
        /// <returns>Una vista con estilos</returns>
        public async Task<ActionResult> Index()
        {
            
            var f = 1;
            List<CodigoFechaAbierta> coddi = new List<CodigoFechaAbierta>();
            var rf = f == 1;
            f = f == 1 ? 0 : 1;

            /*
            string strResultado = string.Empty;
            string strPathTemp = string.Empty;
            MemoryStream streamm = new MemoryStream();
            CodigoFechaAbierta codFecha = new CodigoFechaAbierta();
            codFecha.IdSAPCliente = 22021212;
            codFecha.rtaLogo = "D:/Usuarios/User/itrujillo/Pictures/Screenshots/imgpru.png";
            var dato = await PostAsync<CodigoFechaAbierta, MemoryStream>("CodigoFechaAbierta/intentoImgBD", codFecha);

            //Image.FromStream(dato, true).Save();
            Image imgGuarda = Image.FromStream((MemoryStream)dato.Elemento, true);

            string strCodigoBarras = "RELOL";
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            imgGuarda.Save(streamm, ImageFormat.Jpeg);
            imgGuarda.Save(strPathTemp, ImageFormat.Jpeg);
            
            var fff = imgGuarda;
            imgGuarda = null;

             */
            /*Code128BarcodeDraw BarCode = BarcodeDrawFactory.Code128WithChecksum;
            MemoryStream stream = new MemoryStream();
            Image img = BarCode.Draw(strCodigoBarras, intAlto, intAncho);
            img.Save(stream, ImageFormat.Png);
            strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
            Image CodigoBarras = System.Drawing.Image.FromStream(stream);
            CodigoBarras.Save(strPathTemp, ImageFormat.Jpeg);
            strResultado = strPathTemp;
*/
            return View();
        }

        /// <summary>
        /// Va editar las redenciones a redimidos para poder tener control de los datos que se exportaron 
        /// </summary>  
        /// <param name="daT"></param>
        /// <returns>Vista con los datos actualizado</returns>
        [HttpPut]
        public async Task<ActionResult> editar(string id)
        {
            CodigoFechaAbierta edi = new CodigoFechaAbierta();
            //var listaa = await PutAsync<CodigoFechaAbierta, string>("CodigoFechaAbierta/Editar", edi);
            return Json(new { F = true });
        }


        /// <summary> 
        /// Busca codigos para mostrar en pantalla y poder exportar en el archivo excel 
        /// </summary>
        /// <param name="daT"></param>
        /// <param name="check"></param>
        /// <returns>La vista con los datos que se consultaron</returns>
        [HttpGet]
        public async Task<ActionResult> BuscarCodigos(string daT, string check)
        {
            var datt = await GetAsync<IEnumerable<CodigoFechaAbierta>>($"CodigoFechaAbierta/VerFacturas/{daT}" + "/" + check);
            codSap = new List<string>();
            if (datt != null)
            {
                foreach (CodigoFechaAbierta c in datt)
                {
                    if (c.Redencion == 0)
                    {
                        codSap.Add(c.CodBarrasBoletaControl);
                    }
                }
            }
            //return Json(new RespuestaViewModel { Correcto = false }, JsonRequestBehavior.AllowGet);
            codExistente = datt;
            GenerarQR("345324", 2, 2);
            return PartialView("_check", datt);
        }


        #region EnviarMails
        public bool EnviarCorreo(string sTo, string sSubject, string sMensaje, MailPriority mpPriority, List<string> attachmentList)
        {

            var mail = new MailMessage();

            var smtpServer = new SmtpClient("smtp.gmail.com", 587);
            var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
            var MailPass = ConfigurationManager.AppSettings["MailPass"].ToString();
            var Port = ConfigurationManager.AppSettings["Port"].ToString();
            mail.From = new MailAddress((string)MailFrom, ConfigurationManager.AppSettings["Mask"].ToString());
            mail.To.Add(sTo);
            mail.Subject = sSubject;
            string tbody = sMensaje;
            mail.Body = tbody;
            mail.IsBodyHtml = true;
            mail.Priority = mpPriority;

            var ms = new MemoryStream();
            if (attachmentList != null)
            {
                foreach (var cln in attachmentList)
                {
                    ms = new MemoryStream(System.IO.File.ReadAllBytes(cln));

                    try
                    {
                        mail.Attachments.Add(new Attachment(ms, new FileInfo(cln).Name));
                    }
                    catch (Exception) { }
                }
            }
            smtpServer.Port = int.Parse(Port);
            smtpServer.EnableSsl = true;
            smtpServer.Credentials = new System.Net.NetworkCredential(MailFrom, MailPass);

            try
            {
                smtpServer.Send(mail);
                ms.Close();
                ms.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region generar reporte 
        private string[] ObtenerEncabezado(object elemento)
        {
            List<string> encabezado = new List<string>();
            foreach (var item in elemento.GetType().GetProperties())
            {
                encabezado.Add(item.Name);
            }
            return encabezado.ToArray();
        }

        private IEnumerable<string[]> ObtenerDetalle(IEnumerable<object> lista)
        {
            List<string[]> detalle = new List<string[]>();

            foreach (var item in lista)
            {
                List<string> registro = new List<string>();
                foreach (var reg in item.GetType().GetProperties())
                {

                    var value = reg.GetValue(item, null) == null ? string.Empty : reg.GetValue(item, null).ToString();
                    registro.Add(value);
                }
                detalle.Add(registro.ToArray());
            }
            return detalle;
        }

        /// <summary>
        /// Genera los reportes de una hoja
        /// </summary>
        /// <param name="lista">lista del elemento donde se encuentran los datos,los campos que tienen numeros, seran en fomato numerico</param>
        /// <param name="list">Esto contiene los datos para poder compararlo el tamaño de los datos del detalles para poder capturar solo el numerp de datos que se necesitan</param>
        /// <param name="mapeo">nombre de los encabezados, de como se mapean de base de datos al reporte</param>
        /// <param name="NombreReporte">nombre de como se llamara el reporte</param>
        /// <param name="decimales">las columnas que deben tener decimales, EJ: new string[]{"columna","A","D"};</param>
        /// <returns></returns>
        public string GenerarReporteExcel(IEnumerable<object> lista, List<CodigoFechaAbierta> list, IEnumerable<Parametro> mapeo, string NombreReporte = "", string[] decimales = null, string[] sumatoria = null)
        {
            if (lista.Count() == 0)
            {
                throw new Exception("No hay datos en la consulta");
            }

            var Encabezado = ObtenerEncabezado(lista.FirstOrDefault());
            var Detalle = ObtenerDetalle(lista);
            string strNombreArchivo = string.Empty;
            string strRutaArchivo = string.Empty;
            string strRutaReportes = string.Empty;
            string semiRuta = string.Empty;
            List<int> idsMapeo = new List<int>();
            XLWorkbook wb = new XLWorkbook(); //new XLWorkbook()
            try
            {
                strRutaReportes = Utilidades.RutaReportes();
                if (strRutaReportes.Trim().IndexOf("Error") >= 0)
                {
                    throw new ArgumentException(strRutaReportes);
                }

                strNombreArchivo = string.Concat(NombreReporte, "_", Utilidades.FechaString(), ".xlsx");//tellez 2017/05/24 - ahora exporrta en archivo excel
                semiRuta = "excel/";
                semiRuta = string.Concat(semiRuta, strNombreArchivo);
                //Ruta del aplicativo
                //strRutaReportes = System.AppDomain.CurrentDomain.BaseDirectory;
                //strRutaReportes = string.Concat(strRutaReportes, "excel/");
                //strRutaReportes = "D:\\Usuarios\\ITrujillo\\Downloads\\";
                strRutaArchivo = string.Concat(strRutaReportes, strNombreArchivo);
                var ws = wb.Worksheets.Add("Reporte");

                int contadorFila = 1;
                int contador = 2;
                if (NombreReporte.Equals("ReporteVentasDiarias"))
                {
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = "Comprobante Informe Diario";
                    contador = 3;
                }


                for (int i = 1; i <= Encabezado.Count(); i++)
                {
                    if (mapeo.Any(M => M.Nombre.Equals(Encabezado[i - 1])))
                    {
                        Parametro valorParametro = mapeo.Where(M => M.Nombre.Equals(Encabezado[i - 1])).FirstOrDefault();
                        if (NombreReporte.Equals("ReporteVentasDiarias"))
                        {
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}2").Value = valorParametro.Valor;
                        }
                        else
                        {
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = valorParametro.Valor;
                        }

                        idsMapeo.Add(i - 1);
                        contadorFila++;
                    }
                }
                //DataGridView f = new DataGridView();
                int n = 0;
                contadorFila = 1;
                foreach (var item in Detalle)
                {
                    var col = "E" + n.ToString();
                    for (int i = 1; i <= item.Count(); i++)
                    {
                        if (idsMapeo.Any(M => M == i - 1))
                        {
                            double valor;
                            if (double.TryParse(item[i - 1], out valor))
                            {
                                ws.RowHeight = 95;
                                ws.ColumnWidth = 95;
                                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = valor;
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Style.NumberFormat.NumberFormatId = 1;// 0_General - 2_0.00 - 3_#,##0
                            }
                            else if (item[10] != null && n < list.Count())
                            {
                                ws.AddPicture((item[i - 1].ToString())).MoveTo(ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Address).ScaleHeight(1, false);//Scale(1.0);
                            }
                            else
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = item[i - 1];
                            contadorFila++;
                        }
                    }
                    contadorFila = 1;
                    contador++;
                }

                if (decimales != null)
                    for (int i = 0; i < decimales.Length; i++)
                    {
                        if (decimales[i].Split(':').Length > 1)
                        {
                            ws.Column(decimales[i].Split(':')[0]).Style.NumberFormat.Format = string.Concat("0.", new String('0', int.Parse(decimales[i].Split(':')[1].ToString())));
                        }
                        else
                        {
                            ws.Column(decimales[i]).Style.NumberFormat.NumberFormatId = 2;
                        }
                    }

                if (sumatoria != null)
                {
                    double suma = 0;
                    int k = 0;
                    for (int i = 0; i < sumatoria.Length; i++)
                    {
                        for (k = 0; k < lista.Count(); k++)
                            suma += double.Parse(ws.Cell(sumatoria[i] + (k + 2)).Value.ToString());
                        ws.Cell(k + 2, sumatoria[i]).Value = suma;
                        ws.Cell(k + 2, sumatoria[i]).Style.NumberFormat.NumberFormatId = 1;
                        suma = 0;
                    }
                }


                ws.Columns().AdjustToContents();
                wb.SaveAs(strRutaArchivo);
            }
            catch (Exception ex)
            {
                strNombreArchivo = string.Concat("Error en GenerarReporte: ", ex.Message, " ", strNombreArchivo);
            }

            
            location = strRutaArchivo;
            //return semiRuta;
            return strNombreArchivo; 
        }
        #endregion
        #region codigo barras
        public static string GenerarCodigoDeBarras(string strCodigoBarras, int intAlto, int intAncho)
        {

            string strResultado = string.Empty;
            string strPathTemp = string.Empty;

            Code128BarcodeDraw BarCode = BarcodeDrawFactory.Code128WithChecksum;
            MemoryStream stream = new MemoryStream();

            try
            {
                Image img = BarCode.Draw(strCodigoBarras, intAlto, intAncho);
                img.Save(stream, ImageFormat.Png);
                strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
                Image CodigoBarras = System.Drawing.Image.FromStream(stream);
                CodigoBarras.Save(strPathTemp, ImageFormat.Jpeg);
                strResultado = strPathTemp;
                //MessageBox.Show(strResultado);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CodigoBarras_GenerarCodigoDeBarras");
                strResultado = string.Concat("Error en GenerarCodigoDeBarras_CodigoBarras: ", ex.Message);
            }
            finally
            {
                BarCode = null;
                stream = null;
            }
            rutaElimina.Add(strPathTemp);
            return strPathTemp;
        }
        #endregion


        public static string GenerarQR(string strCodigoBarras, int intAlto, int intAncho)
        {

            string strResultado = string.Empty;
            string strPathTemp = string.Empty;

            CodeQrBarcodeDraw BarCode = BarcodeDrawFactory.CodeQr;
            MemoryStream stream = new MemoryStream();

            try
            {
                Image img = BarCode.Draw(strCodigoBarras, intAlto, intAncho);

                try
                {
                    //System.Drawing.Image logo = System.Drawing.Image.FromFile("D:/Usuarios/ITrujillo/Downloads/iielee.jpeg"); 
                    System.Drawing.Image logo = System.Drawing.Image.FromFile(@"D:\Usuarios\BCaicedo\Documents\Visual Studio 2019\dev.azure\Fuentes Core Desarrollo\CorParques.Presentacion.MVC\Images\user.png");

                    int left = (img.Width / 2) - (logo.Width / 2);
                    int height = (img.Height / 2) - (logo.Height / 2);
                    Graphics g = Graphics.FromImage(img);

                    g.DrawImage(logo, new Point(left, height));

                } catch (Exception e)
                {
                    strResultado = "No se genero el logo para el QR";
                }

                img.Save(stream, ImageFormat.Png);
                strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
                Image CodigoBarras = System.Drawing.Image.FromStream(stream);
                CodigoBarras.Save(strPathTemp, ImageFormat.Jpeg);
                strResultado = strPathTemp;
                //MessageBox.Show(strResultado);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CodigoBarras_GenerarCodigoDeBarras");
                strResultado = string.Concat("Error en GenerarCodigoDeBarras_CodigoBarras: ", ex.Message);
            }
            finally
            {
                BarCode = null;
                stream = null;
            }
             
            rutaElimina.Add(strPathTemp);
            //System.Drawing.Image logo = System.Drawing.Image.FromFile(strPathTemp + )
            return strPathTemp;
        }

        /// <summary>
        /// Este metodo funciona para poder exportar los codigos y la direcion de las imagenes para dibujarlas en el excel
        /// </summary>
        /// <param name="l">Recibe datos para exportar a excel</param>
        /// <returns>Retorna la ruta del archivo</returns>
        [HttpPost]
        public async Task<string> exportarCod(Array l)
        {
            List<string> corr = new List<string>();
            CorParques.Transversales.Util.Reportes r = new CorParques.Transversales.Util.Reportes();
            List<Parametro> parametro = new List<Parametro>();
            /* Estos datos son los parametros que se pintaran en excel */
            //parametro.Add(new Parametro { Nombre = "IdPedidoBoletaControl" });
            //parametro.Add(new Parametro { Nombre = "CodSapPedido" });
            parametro.Add(new Parametro { Nombre = "CodBarrasBoletaControl" });
            parametro.Add(new Parametro { Nombre = "codigoBarras" });
            IEnumerable<CodigoFechaAbierta> datoExistente = codExistente;
            List<CodigoFechaAbierta> preDato = new List<CodigoFechaAbierta>();
            foreach (var it in datoExistente)
            {
                foreach (string ii in codSap)
                {
                    if (it.CodBarrasBoletaControl == ii)
                    {
                        string ruta = ImprimirCodigoBarras(ii);
                        it.codigoBarras = ruta;
                        preDato.Add(it);
                    }
                }
            }
            List<CodigoFechaAbierta> otro = preDato;
            IEnumerable<CodigoFechaAbierta> datoFinal = preDato;
            string fg = GenerarReporteExcel(datoFinal, otro, parametro, "Codigos_fecha_abierta");
            foreach (string f in rutaElimina)
            {
                if (System.IO.File.Exists(f))
                {
                    System.IO.File.Delete(f);
                }
            }
            //rutaEliminar = fg;
            //return Json(new { name = fg });
            return fg;
        }




        /// <summary>
        ///Genera el codigo de barras en imagen y las almacena en la temporal -> despues se eliminaran
        /// </summary>
        /// <param name="c">Ya es el dato descomprimido de un objeto que sera convertido en codigo de barras</param>
        /// <returns>La ruta de la imagen en codigo de barras</returns>
        public string ImprimirCodigoBarras(string c)
        {
            var str = string.Empty;
            Reportes r = new Reportes();
            //str = GenerarCodigoDeBarras(c, 100, 1);
            str = GenerarQR(c, 2, 2);
            rutaEliminar += str + " ,";
            return str;
        }

        /// <summary>
        /// Este metodo va a eliminar el archivo excel
        /// </summary>
        /// <param name="rutaEliminar">Recibe un parametro con el nombre del archivo para eliminar</param>
        /// <returns>"Termino el proceso"</returns>
 
        [HttpPost]
        public string Eliminar()
        {
            //Ruta del aplicactivo
            string ubicacion = System.AppDomain.CurrentDomain.BaseDirectory;
            string dir = ubicacion + rutaEliminar;
            if (System.IO.File.Exists(dir))
            {
                System.IO.File.Delete(dir);
            }
            return "Termino el proceso";
        }


        [HttpGet]
        public async Task<ActionResult> Obtener(string id)
        {
            var dato = await GetAsync<CodigoFechaAbierta>("CodigoFechaAbierta/ObtenerId/" + id);
            return PartialView("_editar", dato);
            //return Json(new { f = dato });


        }

        [HttpGet]
        public ActionResult EnviarCorreo(string id, string lol)
        {
            var dato = "relol";
            return PartialView("EnviarCorreo", dato);
        }

        [HttpGet]
        public async Task<string> EnviandoCorreo(string qr) 
        {
            string preQr = "bcaicedo@corparques.co";
            //string envQR = preQr.Replace(".", "|");
            string PpathQR = GenerarQR("345324", 2, 2);
            //string PpathQR = "D:||rel||lol||ff.Jpeg";
            string ff = PpathQR.Replace("D:", "");
            string fff = ff.Replace("\\", "||");
            string pathQR = fff.Replace(".Jpeg", "");

            //await GetAsync<string>($"CodigoFechaAbierta/EnviarQR/{qr}/{pathQR}");
            await GetAsync<string>($"CodigoFechaAbierta/EnviarQR/{preQr}/{pathQR}");
            return $"Enviando correo con QR = {qr}";
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerTodo(string CodSapPedido)
        {
            var datosTodos = await GetAsync<IEnumerable<CodigoFechaAbierta>>("CodigoFechaAbierta/ObtenerTodo/" + CodSapPedido);
            return Json(new { ff = datosTodos });
        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception e)
            {
                return null;
            }
            return objFileContentResult;
        }

        //[HttpGet]
        //public void enviarMail()
        //{
        //    List<string> attachment = new List<string>();
        //    EnviarCorreo("trujilloivanzx@gmail.com", "tested", "this is test", MailPriority.Low, attachment);
        //}

        public async Task<ActionResult> test()
        {
            return Json(new { f = "data" });
        }
        #endregion


    }

}