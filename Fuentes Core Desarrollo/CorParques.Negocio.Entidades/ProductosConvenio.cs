using System;
using System.Collections.Generic;
using System.Text;

namespace CorParques.Negocio.Entidades
{
    public class ProductosConvenio
    {
            public string Usuario { get; set; }
            public IEnumerable<CodigoSapProducto> CodigoSapProductos { get; set; }
            public int OtroConvenio { get; set; }
        
    }

    public class CodigoSapProducto
    {
        public string CodigoSapProductos { get; set; }

    }

}
