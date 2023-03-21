using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ImprimirBoletaControl
    {
        public List<Producto> ListaProductos { get; set; }
        public List<AdicionPedidos> ListaAdicionPedidos { get; set; }
        public string CodBarraInicio { get; set; }
        public string CodBarraFinal { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstadoBoleta { get; set; }
        public int IdPunto { get; set; }
        public string IdSolicitudBoleteria { get; set; }
        public string CodigoPedido { get; set; }

    }
}
