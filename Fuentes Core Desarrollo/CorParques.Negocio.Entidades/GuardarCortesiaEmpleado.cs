using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class GuardarCortesiaEmpleado
    {
        public int idUsuario { get; set; }
        public string Documento { get; set; }
        public List<Producto> ListaProductos { get; set; }
    }
}
