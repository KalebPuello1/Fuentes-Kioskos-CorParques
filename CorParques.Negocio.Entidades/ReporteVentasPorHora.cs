using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentasPorHora
    {
        public int IdPunto { get; set; }
        public string Punto { get; set; }
        public string TipoProducto { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Cantidad { get; set; }
        public string CostoUnitario { get; set; }
        public string CostoTotal { get; set; }
        public string ValorSinImpuesto { get; set; }
        public string ValorConImpuesto { get; set; }
        public string PCosto { get; set; }
        public string PParticipacion { get; set; }        
    }
}
