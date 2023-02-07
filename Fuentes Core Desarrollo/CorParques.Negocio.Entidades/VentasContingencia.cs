using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class VentasContingencia
    {
        public int Id_Factura { get; set; }
        public DateTime Fecha { get; set; }
        public int IdPunto { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdEstado { get; set; }
        public string CodigoFactura { get; set; }
        public int Id_Producto { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
        public int IdDetalleProducto { get; set; }
        public bool Entregado { get; set; }
    }
}
