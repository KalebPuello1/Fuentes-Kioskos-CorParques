using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReimpresionFiltros
    {
        public string CodInicialFactura { get; set; }
        public string CodFinalFactura { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string CodBrazalete { get; set; }
        public int? CodPunto { get; set; }
        public int? Validado { get; set; }
        public IEnumerable<Reimpresion> objReimpresion { get; set; }
    }
}
