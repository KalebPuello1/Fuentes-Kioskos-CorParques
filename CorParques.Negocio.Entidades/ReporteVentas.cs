using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteVentas
    {
        public DateTime? Fecha { get; set; }
        public string Consecutivo { get; set; }
        public int? IdPunto { get; set; }
        public string Taquilla { get; set; }
        public int? IdTaquillero { get; set; }
        public string Taquillero { get; set; }
        public int? IdProducto { get; set; }
        public string Producto { get; set; }
        public int? Cantidad { get; set; }
        public double? ValorSinImpuesto { get; set; }
        public double? Impuesto { get; set; }
        public int? IdMedioPago { get; set; }
        public string MedioPago { get; set; }
        public int? IdFranquicia { get; set; }
        public string Franquicia { get; set; }
        public string NumAprobacion { get; set; }
        public double? TotalRecibido { get; set; }
        public string Anulaciones { get; set; }
        public string NotaCredito { get; set; }
        public string NumNotaCredito { get; set; }
        public string Propina { get; set; }
        public string TipoCliente { get; set; }
        public string idCliente { get; set; }
        public string Nombre { get; set; }
        public string Convenio { get; set; }
        public string SupervisorNotaCredito { get; set; }


    }
}
