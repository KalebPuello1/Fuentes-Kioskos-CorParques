using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_AperturaBrazaleteDetalle")]
    public class AperturaBrazaleteDetalle
    {
        [Column("IdAperturaBrazaleteDetalle"), Key]
        public int Id { get; set; }
        
        [Column("IdAperturaBrazalete")]
        public int IdAperturaBrazalete { get; set; }

        [Column("IdTaquillero")]
        public int IdTaquillero { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Editable(false)]
        public string CodigoSap { get; set; }


    }
}
