using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_BitacoraElementosDetalle")]
    public class BitacoraElementosDetalle
    {
        [Column("IdBitacoraElementosDetalle"), Key]
        public int Id { get; set; }

        public int IdBitacoraElementos { get; set; }

        public int IdAperturaElemento { get; set; }

        public bool ValidTaquillaEntrega { get; set; }

        public bool ValidTaquillaRecibe { get; set; }
        
    }
}
