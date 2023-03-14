using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReimpresionBrazalete
    {
        public int IdFactura { get; set; }
        public string CodigoFactura { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public int Serial { get; set; }
    }
}
