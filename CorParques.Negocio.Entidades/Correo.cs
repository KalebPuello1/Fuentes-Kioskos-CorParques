using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class Correo
    {
        public string De { get; set; }
        public string[] Para { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public List<string> attachmentList { get; set; }
        public MailPriority Prioridad { get; set; }
        public string Respuesta { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
