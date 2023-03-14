using System;
using System.Collections.Generic;
using System.Text;

namespace CorParques.Negocio.Entidades
{
    public class FacturaSolicitud
    {

        public IEnumerable<ProductosTienda> productosTienda { get; set; }

        public List<PagoFacturaMediosPago> mediosPago  { get; set; }

        public int IdUsuario { get; set; }

        public int idPunto { get; set; }
    }
}

