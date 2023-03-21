using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteBoleteria
    {        
        public DateTime? Fecha { get; set; }
        public string Taquillero { get; set; }
        public string TipoBrazalete { get; set; }
        public int Asignados { get; set; }
        public int TotalVendidos { get; set; }
        public int Redenciones { get; set; }
        public int Adiciones { get; set; }
        public int Entregados { get; set; }
        public int Sobrante { get; set; }
        public int Faltante { get; set; }
        public int impresionEnLinea { get; set; }
        public int EnCaja { get; set; }
    }
}
