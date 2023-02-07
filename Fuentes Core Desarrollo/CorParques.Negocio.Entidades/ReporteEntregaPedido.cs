using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteEntregaPedido
    {        
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaImpresion{ get; set; }
        public DateTime FechaUso { get; set; }
        public string Pedido { get; set; }
        public string Cliente { get; set; }
        public string Asesor { get; set; }
        public string TipoVenta { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }
        public string EntregadoPor { get; set; }
        public string RecibidoPor { get; set; }
        public int Devolucion { get; set; }
        public string DevolucionRecibe { get; set; }
        public string DevolucionEntrega { get; set; }
        public int CantidadRetorno { get; set; }


    }
}
