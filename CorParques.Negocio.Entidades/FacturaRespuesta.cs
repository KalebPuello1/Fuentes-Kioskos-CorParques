using System;
using System.Collections.Generic;
using System.Text;

namespace CorParques.Negocio.Entidades
{
    public class FacturaRespuesta
    {

        public string ConsecutivoFactura { get; set; }
        
        public List<ProductosTiendaRespuesta> Productos { get; set; }
        
        public string MensajeRespuesta { get; set; }
    }
}

