using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ResumenCierre
    {
        public int IdPunto { get; set; }
        public string Nombre { get; set; }
        public string Dinero { get; set; }
        public string Elementos { get; set; }
        public string Inventario { get; set; }
    }
}
