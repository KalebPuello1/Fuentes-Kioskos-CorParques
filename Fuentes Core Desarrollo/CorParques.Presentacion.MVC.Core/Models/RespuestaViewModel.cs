using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorParques.Presentacion.MVC.Core.Models
{
    public class RespuestaViewModel
    {
        public bool Correcto { get; set; }
        public object Elemento { get; set; }
        public string Mensaje { get; set; }
    }
}