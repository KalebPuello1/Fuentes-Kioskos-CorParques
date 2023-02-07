using System;

namespace CorParques.Negocio.Entidades
{

    public class BrazaleteReimpresion
    {
        public string NumeroFactura { get; set; }

        public DateTime FechaCompra { get; set; }

        public string NombreProducto { get; set; }

        public string Consecutivo { get; set; }

        public int IdProducto { get; set; }
    }
}
