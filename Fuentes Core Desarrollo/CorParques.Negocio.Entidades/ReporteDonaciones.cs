using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteDonaciones
    {
        public DateTime? Fecha { get; set; }
        public string NombrePunto { get; set; }
        public string Consecutivo { get; set; }
        public string Donante { get; set; }
        public string Producto { get; set; }
        public int? Cantidad { get; set; }
        public int NotaCredito { get; set; }
        public string Anulaciones { get; set; }
        public string NumNotaCredito { get; set; }
        public string MedioPago { get; set; }
        public int? IdPunto { get; set; }
        public int? IdProducto { get; set; }

    }
}
