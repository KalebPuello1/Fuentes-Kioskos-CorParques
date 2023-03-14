using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteRecoleccion
    {
        public DateTime? Fecha { get; set; }
        public string TipoRecoleccion { get; set; }
        public string NumeroRecoleccion { get; set; }
        public int? IdRecoleccion { get; set; }
        public string Punto { get; set; }
        public string Taquillero { get; set; }
        public string NumReferencia { get; set; }
        public int? Total { get; set; }
        public string Supervisor { get; set; }
        public int? Cierre { get; set; }
        public int? IdTaquillero { get; set; }
        public int? IdRecolector { get; set; }
        public int? IdSupervisor { get; set; }
        public int? IdPunto { get; set; }
   //     public IEnumerable<AperturaElementosHeader> Taquilleros { get; set; }
     public IEnumerable<ReporteRecoleccion> objReporteRecoleccion { get; set; }
        public string idProducto { get; set; }
        public IEnumerable<ReporteRecoleccion> Recoleccion { get; set; }
    }
}
