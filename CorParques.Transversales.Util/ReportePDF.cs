using CorParques.Negocio.Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Transversales.Util
{
    public class ReportePDF
    {
        public string nombre;
        public ReportePDF()
        { 
        
        }

        public string generarEncabezado() 
        {
            return "";
        }
        public string insertarValores()
        {
            return "";
        }
        public string generarPDF(List<CodigoFechaAbierta> datos, string rutaQR)
        {
            string pdfpath = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string imagepath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            var rutaPdf = "";
            Document doc = new Document();
            var inicial = datos.First().FechaInicial;
            var final = datos.First().FechaFinal;
            var nombreCliente = datos.First().NombreClientePDF;


            try

            {

                //iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
                rutaPdf = pdfpath + $"/{datos.First().CodSapPedido}_{DateTime.Now.Millisecond}.pdf";
                PdfWriter.GetInstance(doc, new FileStream(rutaPdf, FileMode.Create));
                doc.Open();

                /*Image codigo1 = Image.GetInstance(datos.First().rtaLogo);
                *//*codigo1.Alignment = 520;
                codigo1.Alignment = Element.ALIGN_CENTER; *//*
                 codigo1.Alignment = Element.ALIGN_RIGHT;
                //codigo1.ScaleAbsolute(20, 20);
                codigo1.SetAbsolutePosition(140, 670);
                //codigo1.Width = 20;
                doc.Add(codigo1);*/


                Image codigo1 = Image.GetInstance(datos.First().rtaLogo);
                codigo1.Alignment = Element.ALIGN_RIGHT;
                codigo1.SetAbsolutePosition(140, 670);
                doc.Add(codigo1);

                //Aqui se debe colocar el icono, se debe verificar el id del cliente y se debe apuntar hacer la consulta de la imagen
                /**/
                //
                /*


                var ruta = Path.GetDirectoryName("./");
                var ruta1 = Path.GetFullPath("./");
                //var ruta2 = Directory.GetCurrentDirectory() + "/Images/logo_ma_color.jpg";
                //var ruta2 = Directory.GetCurrentDirectory();
                //Este es el logo de mundo aventura
                //Image codigo2 = Image.GetInstance("");
                Image codigo2 = Image.GetInstance(ruta2);
                //doc.Add(codigo2);
*/




                var logoMundoAventura = imagepath + "/logoExternoo.png";
                Image codigo2 = Image.GetInstance(logoMundoAventura);
                codigo2.Alignment = Element.ALIGN_LEFT;
                //codigo1.ScaleAbsolute(20, 20);
                codigo2.SetAbsolutePosition(310, 670);
                //Image codigo2 = Image.GetInstance(ruta2);
                doc.Add(codigo2);


                //doc.Add(codigo1,codigo2);

                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                      "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));


                //Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de(Nombre del Cliente)

                //Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada; Solo
                //necesita ingresar al parque Mundo Aventura y empezar a Divertirse!

                //Recuerde que los podrá usar cualquier día que el parque esté abierto desde el _______ hasta el __________

                doc.Add(new Paragraph($"Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura {nombreCliente}"));
                doc.Add(new Paragraph("Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada;"));
                doc.Add(new Paragraph("Solo necesita ingresar al parque Mundo Aventura y empezar a Divertirse!"));
                doc.Add(new Paragraph($"Recuerde que los podrá usar cualquier día que el parque esté abierto desde el {inicial} hasta el {final}"));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                   "));
                doc.Add(new Paragraph("                                                     "));


                //doc.Add(new Paragraph("Esto son codigos virtuales, para poder disfutar de tus productos solo necesitas venir al parque Mundo aventura y empezar a divertirte"));


                string datoEn = "";
                foreach (var item in datos)
                {
                    doc.Add(new Paragraph(item.Nombre));

                    //Image codigo = Image.GetInstance(imagepath + "/6.Jpeg");
                    Image codigo = Image.GetInstance(item.RutaQR);
                    datoEn += datos.First().rtaLogo + " " + item.Nombre;
                    doc.Add(codigo);
                }

                doc.Add(new Paragraph("Deberia enviar ver....." + " " + datoEn));

            }

            catch (Exception ex)

            {

                //Log error;

            }

            finally

            {

                doc.Close();

            }
            return rutaPdf;
        }
        public string log(string mensaje)
        {
            //Consulta logos de  empresa

            string pdfpath = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string imagepath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            var rutaPdf = "";
            Document doc = new Document();
            //var inicial = datos.First().FechaInicial;
            //var final = datos.First().FechaFinal;
            try

            {

                //iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
                rutaPdf = pdfpath + $"/lollog.pdf";
                PdfWriter.GetInstance(doc, new FileStream(rutaPdf, FileMode.Create));
                doc.Open();

                //Image codigo1 = Image.GetInstance(datos.First().rtaLogo);
                /*codigo1.Alignment = 520;
                codigo1.Alignment = Element.ALIGN_CENTER;*/
                //codigo1.Alignment = Element.ALIGN_RIGHT;
                //codigo1.ScaleAbsolute(20, 20);
                //codigo1.SetAbsolutePosition(140, 670);
                //codigo1.Width = 20;
                //doc.Add(codigo1);
                //Aqui se debe colocar el icono, se debe verificar el id del cliente y se debe apuntar hacer la consulta de la imagen
                /**/
                //
                /*

                var ruta = Path.GetDirectoryName("./");
                var ruta1 = Path.GetFullPath("./");
                //var ruta2 = Directory.GetCurrentDirectory() + "/Images/logo_ma_color.jpg";
                //var ruta2 = Directory.GetCurrentDirectory();
                //Este es el logo de mundo aventura
                //Image codigo2 = Image.GetInstance("");
                Image codigo2 = Image.GetInstance(ruta2);
                //doc.Add(codigo2);
*/




           /*     var logoMundoAventura = imagepath + "/logoExternoo.png";
                Image codigo2 = Image.GetInstance(logoMundoAventura);
                codigo2.Alignment = Element.ALIGN_LEFT;
                //codigo1.ScaleAbsolute(20, 20);
                codigo2.SetAbsolutePosition(310, 670);
                //Image codigo2 = Image.GetInstance(ruta2);
                doc.Add(codigo2);*/


                //doc.Add(codigo1,codigo2);

                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                      "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                     *******************************                               "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));


                //Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de(Nombre del Cliente)

                //Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada; Solo
                //necesita ingresar al parque Mundo Aventura y empezar a Divertirse!

                //Recuerde que los podrá usar cualquier día que el parque esté abierto desde el _______ hasta el __________
                /*
                                doc.Add(new Paragraph($"Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de {nombreClienteEmpresa}"));
                                doc.Add(new Paragraph("Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada;"));
                                doc.Add(new Paragraph("Solo necesita ingresar al parque Mundo Aventura y empezar a Divertirse!"));
                                doc.Add(new Paragraph($"Recuerde que los podrá usar cualquier día que el parque esté abierto desde el {inicial} hasta el {final}"));
                                doc.Add(new Paragraph("                                                     "));
                                doc.Add(new Paragraph("                                                   "));
                                doc.Add(new Paragraph("                                                     "));

                */
                //doc.Add(new Paragraph("Esto son codigos virtuales, para poder disfutar de tus productos solo necesitas venir al parque Mundo aventura y empezar a divertirte"));

                /*   foreach (var item in datos)
                   {
                       doc.Add(new Paragraph(item.Nombre));

                       //Image codigo = Image.GetInstance(imagepath + "/6.Jpeg");
                       Image codigo = Image.GetInstance(item.RutaQR);

                       doc.Add(codigo);
                   }
   */
                doc.Add(new Paragraph(mensaje));

            }

            catch (Exception ex)

            {

                //Log error;

            }

            finally

            {

                doc.Close();

            }
            return rutaPdf;
        }

        public string MuestraDato(string mensaje)
        {
            //Consulta logos de  empresa

            string pdfpath = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string imagepath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            var rutaPdf = "";
            Document doc = new Document();
            //var inicial = datos.First().FechaInicial;
            //var final = datos.First().FechaFinal;
            try

            {

                //iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
                rutaPdf = pdfpath + $"/lol.pdf";
                PdfWriter.GetInstance(doc, new FileStream(rutaPdf, FileMode.Create));
                doc.Open();

                //Image codigo1 = Image.GetInstance(datos.First().rtaLogo);
                /*codigo1.Alignment = 520;
                codigo1.Alignment = Element.ALIGN_CENTER;*/
                //codigo1.Alignment = Element.ALIGN_RIGHT;
                //codigo1.ScaleAbsolute(20, 20);
                //codigo1.SetAbsolutePosition(140, 670);
                //codigo1.Width = 20;
                //doc.Add(codigo1);
                //Aqui se debe colocar el icono, se debe verificar el id del cliente y se debe apuntar hacer la consulta de la imagen
                /**/
                //
                /*

                var ruta = Path.GetDirectoryName("./");
                var ruta1 = Path.GetFullPath("./");
                //var ruta2 = Directory.GetCurrentDirectory() + "/Images/logo_ma_color.jpg";
                //var ruta2 = Directory.GetCurrentDirectory();
                //Este es el logo de mundo aventura
                //Image codigo2 = Image.GetInstance("");
                Image codigo2 = Image.GetInstance(ruta2);
                //doc.Add(codigo2);
*/




                /*     var logoMundoAventura = imagepath + "/logoExternoo.png";
                     Image codigo2 = Image.GetInstance(logoMundoAventura);
                     codigo2.Alignment = Element.ALIGN_LEFT;
                     //codigo1.ScaleAbsolute(20, 20);
                     codigo2.SetAbsolutePosition(310, 670);
                     //Image codigo2 = Image.GetInstance(ruta2);
                     doc.Add(codigo2);*/


                //doc.Add(codigo1,codigo2);

                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                      "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                     *******************************                               "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));


                //Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de(Nombre del Cliente)

                //Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada; Solo
                //necesita ingresar al parque Mundo Aventura y empezar a Divertirse!

                //Recuerde que los podrá usar cualquier día que el parque esté abierto desde el _______ hasta el __________
                /*
                                doc.Add(new Paragraph($"Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de {nombreClienteEmpresa}"));
                                doc.Add(new Paragraph("Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada;"));
                                doc.Add(new Paragraph("Solo necesita ingresar al parque Mundo Aventura y empezar a Divertirse!"));
                                doc.Add(new Paragraph($"Recuerde que los podrá usar cualquier día que el parque esté abierto desde el {inicial} hasta el {final}"));
                                doc.Add(new Paragraph("                                                     "));
                                doc.Add(new Paragraph("                                                   "));
                                doc.Add(new Paragraph("                                                     "));

                */
                //doc.Add(new Paragraph("Esto son codigos virtuales, para poder disfutar de tus productos solo necesitas venir al parque Mundo aventura y empezar a divertirte"));

                /*   foreach (var item in datos)
                   {
                       doc.Add(new Paragraph(item.Nombre));

                       //Image codigo = Image.GetInstance(imagepath + "/6.Jpeg");
                       Image codigo = Image.GetInstance(item.RutaQR);

                       doc.Add(codigo);
                   }
   */
                doc.Add(new Paragraph(mensaje));

            }

            catch (Exception ex)

            {

                //Log error;

            }

            finally

            {

                doc.Close();

            }
            return rutaPdf;
        }
        public string repositorio(string mensaje)
        {
            //Consulta logos de  empresa

            string pdfpath = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string imagepath = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            var rutaPdf = "";
            Document doc = new Document();
            //var inicial = datos.First().FechaInicial;
            //var final = datos.First().FechaFinal;
            try

            {

                //iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
                rutaPdf = pdfpath + $"/repo.pdf";
                PdfWriter.GetInstance(doc, new FileStream(rutaPdf, FileMode.Create));
                doc.Open();

                //Image codigo1 = Image.GetInstance(datos.First().rtaLogo);
                /*codigo1.Alignment = 520;
                codigo1.Alignment = Element.ALIGN_CENTER;*/
                //codigo1.Alignment = Element.ALIGN_RIGHT;
                //codigo1.ScaleAbsolute(20, 20);
                //codigo1.SetAbsolutePosition(140, 670);
                //codigo1.Width = 20;
                //doc.Add(codigo1);
                //Aqui se debe colocar el icono, se debe verificar el id del cliente y se debe apuntar hacer la consulta de la imagen
                /**/
                //
                /*

                var ruta = Path.GetDirectoryName("./");
                var ruta1 = Path.GetFullPath("./");
                //var ruta2 = Directory.GetCurrentDirectory() + "/Images/logo_ma_color.jpg";
                //var ruta2 = Directory.GetCurrentDirectory();
                //Este es el logo de mundo aventura
                //Image codigo2 = Image.GetInstance("");
                Image codigo2 = Image.GetInstance(ruta2);
                //doc.Add(codigo2);
*/




                /*     var logoMundoAventura = imagepath + "/logoExternoo.png";
                     Image codigo2 = Image.GetInstance(logoMundoAventura);
                     codigo2.Alignment = Element.ALIGN_LEFT;
                     //codigo1.ScaleAbsolute(20, 20);
                     codigo2.SetAbsolutePosition(310, 670);
                     //Image codigo2 = Image.GetInstance(ruta2);
                     doc.Add(codigo2);*/


                //doc.Add(codigo1,codigo2);

                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                      "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                     *******************************                               "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                     "));
                doc.Add(new Paragraph("                                                    "));
                doc.Add(new Paragraph("                                                     "));


                //Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de(Nombre del Cliente)

                //Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada; Solo
                //necesita ingresar al parque Mundo Aventura y empezar a Divertirse!

                //Recuerde que los podrá usar cualquier día que el parque esté abierto desde el _______ hasta el __________
                /*
                                doc.Add(new Paragraph($"Estimado(a) visitante: Adjunto encontrará todos los servicios que ha adquirido para el parque Mundo Aventura a través de {nombreClienteEmpresa}"));
                                doc.Add(new Paragraph("Estos son códigos virtuales, que va a poder usar directamente en el parque sin necesidad de detenerse en las taquillas de entrada;"));
                                doc.Add(new Paragraph("Solo necesita ingresar al parque Mundo Aventura y empezar a Divertirse!"));
                                doc.Add(new Paragraph($"Recuerde que los podrá usar cualquier día que el parque esté abierto desde el {inicial} hasta el {final}"));
                                doc.Add(new Paragraph("                                                     "));
                                doc.Add(new Paragraph("                                                   "));
                                doc.Add(new Paragraph("                                                     "));

                */
                //doc.Add(new Paragraph("Esto son codigos virtuales, para poder disfutar de tus productos solo necesitas venir al parque Mundo aventura y empezar a divertirte"));

                /*   foreach (var item in datos)
                   {
                       doc.Add(new Paragraph(item.Nombre));

                       //Image codigo = Image.GetInstance(imagepath + "/6.Jpeg");
                       Image codigo = Image.GetInstance(item.RutaQR);

                       doc.Add(codigo);
                   }
   */
                doc.Add(new Paragraph(mensaje));

            }

            catch (Exception ex)

            {

                //Log error;

            }

            finally

            {

                doc.Close();

            }
            return rutaPdf;
        }
    }
}
