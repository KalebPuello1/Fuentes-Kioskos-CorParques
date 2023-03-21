using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_PRIORIDAD")]
    public class PrioridadCorreo
    {
        [Key]
        [Column("CODPRIRDD")]
        public int Id { get; set; }
        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

    }
}
