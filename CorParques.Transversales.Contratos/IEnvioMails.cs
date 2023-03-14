using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Transversales.Contratos
{
    public interface IEnvioMails
    {
        bool correoFidelizacion(DateTime? FechaNacimiento, string nombre, string correo, string telefono, List<string> adjunto);
        bool EnviarCorreo(string sTo, string sSubject, string sMensaje, MailPriority mpPriority, List<string> attachmentList);
    }
}
