using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class BitacoraDiaLista
    {
        public IEnumerable<BitacoraDia> BitacoraDiaList { get; set; }
        public string Mensaje { get; set; }
        public IEnumerable<TipoGeneral> SegmentoDia { get; set; }
        public IEnumerable<TipoGeneral> Clima{ get; set; }
        public int? CantidadPersonas { get; set; }
    }
}
