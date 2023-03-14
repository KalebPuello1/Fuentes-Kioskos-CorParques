using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class PedidosPorCliente
    {
        public string IdCliente { get; set; }
        public string Pedido { get; set; }
        public int IdUsuario { get; set; }
    }
}
