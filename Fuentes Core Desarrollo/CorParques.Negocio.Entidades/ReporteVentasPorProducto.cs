using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentasPorProducto
    {
        public int IdPunto { get; set; }
        public string Punto { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Cantidad { get; set; }
        public string ValorSinImpuesto { get; set; }
        public string ValorConImpuesto { get; set; }

        public string Nombre { get; set; }
        public decimal Porcentaje { get; set; }
        public int CostoUnitario { get; set; }
        public int CostoTotal { get; set; }
        public decimal porcentajeCosto { get; set; }
        public decimal porcentajeParticipacion { get; set; }
    }
}
