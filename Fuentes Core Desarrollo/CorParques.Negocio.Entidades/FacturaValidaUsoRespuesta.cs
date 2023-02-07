using System;
using System.Collections.Generic;
using System.Text;

namespace CorParques.Negocio.Entidades
{
    public class FacturaValidaUsoRespuesta
    {
        public string Mensaje { get; set; }
        public string ConsecutivoFactura { get; set; }
        public int idProducto { get; set; }
        public string Consecutivo { get; set; }
        public bool Usado { get; set; }


    }
}

