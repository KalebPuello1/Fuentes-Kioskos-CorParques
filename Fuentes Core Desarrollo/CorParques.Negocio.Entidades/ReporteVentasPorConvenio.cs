using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentasPorConvenio
    {
        public string Fecha { get; set; }
        public string NombreConvenio { get; set; }
        public string TipoProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string PrecioSinDescuento { get; set; }
        public string PrecioconDescuento { get; set; }
        public string PorcentajeDescuento { get; set; }
        public string ValorSinImpuesto { get; set; }
        public string Cantidad { get; set; }
    }
}
