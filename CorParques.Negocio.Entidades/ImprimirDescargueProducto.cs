using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ImprimirDescargueProducto
    {
        public string TituloRecibo { get; set; }
        public string CodigoBarrasProp { get; set; }
        public string TituloColumnas { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public float Precio { get; set; }
        public string Consecutivo { get; set; }
    }
}
