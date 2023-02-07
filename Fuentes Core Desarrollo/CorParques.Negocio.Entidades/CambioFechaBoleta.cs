using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class CambioFechaBoleta
    {
        public string Consecutivo { get; set; }

        public string CodigoFactura { get; set; }

        public string FechaUsoInicial { get; set; }
        public string FechaUsoFinal { get; set; }
    }
}
