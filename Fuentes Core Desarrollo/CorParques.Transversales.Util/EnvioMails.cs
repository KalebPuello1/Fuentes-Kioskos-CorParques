using CorParques.Negocio.Entidades;
using CorParques.Transversales.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Net;

namespace CorParques.Transversales.Util
{
    public class EnvioMails : IEnvioMails
    {

        public bool correoFidelizacion(DateTime? FechaNacimiento,string nombre, string correo, string telefono, List<string> adjunto)
        {
            var cumpleanos = string.Empty;
            if (FechaNacimiento != null)
            {
                var mes = string.Empty;
                switch (FechaNacimiento.Value.Month)
                {
                    case 1:
                        mes = "Enero";
                        break;
                    case 2:
                        mes = "Febrero";
                        break;
                    case 3:
                        mes = "Marzo";
                        break;
                    case 4:
                        mes = "Abril";
                        break;
                    case 5:
                        mes = "Mayo";
                        break;
                    case 6:
                        mes = "Junio";
                        break;
                    case 7:
                        mes = "Julio";
                        break;
                    case 8:
                        mes = "Agosto";
                        break;
                    case 9:
                        mes = "Septiembre";
                        break;
                    case 10:
                        mes = "Octubre";
                        break;
                    case 11:
                        mes = "Noviembre";
                        break;
                    case 12:
                        mes = "Diciembre";
                        break;
                    default:
                        break;
                }
                 cumpleanos= $"{FechaNacimiento.Value.ToString("dd")} de {mes} de {FechaNacimiento.Value.Year}";
            }

            var msj = $@"<table class='x_MsoNormalTable' border='0' cellpadding='0' width='700' style='width:525.0pt; background:#EBE6ED'>
<tbody>
<tr>
<td colspan = '2' style = 'padding:0cm 22.5pt 22.5pt 22.5pt' >
   <p class='x_MsoNormal' align='center' style='text-align:center'><b><span style = 'font-size:13.0pt' ><img data-imagetype='External' blockedimagesrc='http://www.mundoaventura.com.co/web2018/wp-content/uploads/2018/08/logoMundoAventura.png' border='0' id='x__x0000_i1026'></span></b></p>
</td>
</tr>
<tr>
<td colspan = '2' style='padding:15.0pt 0cm 15.0pt 0cm'>
<p class='x_MsoNormal'><span style = 'font-size:13.0pt' > Gracias por vincularte a nuestro programa Super Fan.Tus datos han sido registrados satisfactoriamente:
</span></p>
</td>
</tr>
<tr>
<td width = '344' style='width:258.35pt; border:none; border-bottom:solid #EA058F 1.5pt; padding:7.5pt 0cm 7.5pt 0cm; display:inline-block'>
<p class='x_MsoNormal' style='margin-right:26.25pt'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > Nombre:</span></strong><span style = 'font-size:13.0pt' >
    <br >
    {nombre }</span></p>
</td>
<td width = '350' style='width:262.15pt; border:none; border-bottom:solid #EA058F 1.5pt; padding:7.5pt 0cm 7.5pt 0cm; display:inline-block'>
<p class='x_MsoNormal'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > Correo:</span></strong><span style = 'font-size:13.0pt' >
    <br >
    </ span ><a href='mailto:{correo}' target='_blank' rel='noopener noreferrer' data-auth='NotApplicable'>{correo
}</a><span style = 'font-size:13.0pt' >
</ span ></ p >
</ td >
</ tr >
<tr >
<td width='344' style='width:258.35pt; border:none; border-bottom:solid #EA058F 1.5pt; padding:7.5pt 0cm 7.5pt 0cm; display:inline-block'>
<p class='x_MsoNormal' style='margin-right:26.25pt'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > Teléfono:</span></strong><span style = 'font-size:13.0pt' >
    <br >
{telefono}</span></p>
</td>
<td width = '350' style='width:262.15pt; border:none; border-bottom:solid #EA058F 1.5pt; padding:7.5pt 0cm 7.5pt 0cm; display:inline-block'>
<p class='x_MsoNormal'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > Fecha de nacimiento:
</span></strong><span style = 'font-size:13.0pt' ><br >
{cumpleanos}</span></p>
</td>
</tr>
<tr>
<td width = '696' colspan='2' style='width:522.0pt; border:none; border-bottom:solid #EA058F 1.5pt; padding:7.5pt 0cm 7.5pt 0cm; display:inline-block'>
<p class='x_MsoNormal' style='margin-right:26.25pt'><span style = 'font-size:13.0pt' > Agradecemos nos informes tus puntos de interés
    </span></p>
<p class='x_MsoNormal' style='margin-right:26.25pt'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > Que es lo que más te gusta del parque?
</span></strong></p>
<p class='x_MsoNormal' style='margin-right:26.25pt'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > &nbsp;</span></strong></p>
<p class='x_MsoNormal' style='margin-right:26.25pt'><strong><span style = 'font-size:13.0pt; font-family:&quot;Calibri&quot;,sans-serif' > Que te gustaría recibir en tú próxima visita?
</span></strong></p>
</td>
</tr>
<tr>
<td width = '696' colspan='2' style='width:522.0pt; border:none; border-bottom:solid #EA058F 1.5pt; padding:7.5pt 0cm 7.5pt 0cm; display:inline-block'>
<p class='x_MsoNormal'><strong><span style = 'font-family:&quot;Calibri&quot;,sans-serif' > Recuerda aceptar nuestra política de tratamiento de datos personales</span></strong></p>
<p class='x_MsoNormal'><span style = 'font-size:13.0pt' ><a href='https://www.mundoaventura.com.co/web2018/privacidad/' target='_blank' rel='noopener noreferrer' data-auth='NotApplicable'>Acepto la política de Tratamiento de Datos personales</a></span></p>
</td>
</tr>
</tbody>
</table>";
            return EnviarCorreo(
                correo,
                "Fidelización de clientes",
                msj,
                System.Net.Mail.MailPriority.Normal,
                adjunto
                );
        }

        public bool EnviarCorreo(string sTo, string sSubject, string sMensaje, MailPriority mpPriority, List<string> attachmentList)
        {

            var mail = new MailMessage();

            var smtpServer = new SmtpClient(ConfigurationManager.AppSettings["Smtp"].ToString()); ;
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
                    ms = new MemoryStream(File.ReadAllBytes(cln));

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
    }
}
