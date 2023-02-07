using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class Vyucoom
    {
        public string Id_Factura { get; set; }
        public int Id_Punto { get; set; }
        public int Id_Estado { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Producto { get; set; }
        public int Id_Apertura { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime Fecha { get; set; }
        public string IdMedioPago { get; set; }

        public string MontoPago { get; set; }
        public string Cliente { get; set; }
        public string TipDocumento { get; set; }
        public string Documento { get; set; }
        public string IdFactura { get; set; }
        public int Cantidad { get; set; }
        public int IdDetalleProducto { get; set; }
        public int NReferenciaCliente { get; set; }
        public int Cambio { get; set; }
        public string Factura { get; set; }
        public string CodFactura { get; set; }
        public string Valor { get; set; }

        //ReImprimirReciboCajaVyucoom

        public string CodSapCliente { get; set; }
        public string Nombres { get; set; }
        //public string Documento { get; set; }
        public string CodigoFactura { get; set; }
        public int IdPunto { get; set; }
        public int IdUsuarioCreacion { get; set; }
        //public int Cantidad { get; set; }
        public int Precio { get; set; }

        public int MontoPagado { get; set; }

        public int MontoAPagar { get; set; }

        public long CodPedido { get; set; }
        //public string Cambio { get; set; }
        //public string IdMedioPago { get; set; }
        public string NumReferencia { get; set; }

    }
}
