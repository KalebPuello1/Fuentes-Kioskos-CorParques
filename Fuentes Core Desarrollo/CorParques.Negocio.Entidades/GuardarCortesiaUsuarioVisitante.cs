using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class GuardarCortesiaUsuarioVisitante
    {
        public int TipoCortesia { get; set; }
        public int idUsuario { get; set; }
        public string Documento { get; set; }

        public string ValorGenerico { get; set; }
        public List<Producto> ListaProductos { get; set; }

        public List<DetalleCortesia> ListaProductosAPP { get; set; }
        public int IdDetalleCortesia { get; set; }
    }
}
