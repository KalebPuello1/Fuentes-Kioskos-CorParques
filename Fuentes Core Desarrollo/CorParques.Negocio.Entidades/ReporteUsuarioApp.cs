using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteUsuarioApp
    {        
        public string Mailcliente { get; set; }
        public string CodigoFactura { get; set; }
        public string IdDetalleFactura { get; set; }
        public string Nombre { get; set; }
        public string Redimido { get; set; }
        public string CodeQR { get; set; }
        public int Precio { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaUso { get; set; }
        public int IdRegistroUsoBoleteria { get; set; }
        public string Consecutivo { get; set; }
        public DateTime FechaHoraUso { get; set; }
        public object Id { get; set; }
        public DateTime DateTransaction { get; set; }
        public int PriceInitial { get; set; }
        public string User { get; set; }
        public string Data { get; set; }
        public DateTime? DateResponse { get; set; }
        public int IdResponse { get; set; }
        public string Bank { get; set; }
        public int? Price { get; set; }



    }
}
