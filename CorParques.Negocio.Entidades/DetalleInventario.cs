using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class DetalleInventario
    {
        public IEnumerable<Apertura> Apertura { get; set; }
        public IEnumerable<TipoDenominacion> TipoDenomicacionMoneda { get; set; }
        public int IdSupervisor { get; set; }
        public List<BrazaletesApertura> Brazaletes { get; set; }
        public string Observacion { get; set; }
        public string hdPuntos { get; set; }

        //RDSH: Se agrega este campo para poder enviar el id del taquillero en reabastecimiento supervisor-taquilla.
        public int IdTaquillero { get; set; }
    }
}
