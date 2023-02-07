using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_TipoMovimiento")]
    public class TipoMovimiento
    {
        [Column("Id"), Key]
        public int Id { get; set; } 
        [Column("TipoMovimiento")]
        public string Descripcion { get; set; }
        [Column("IdUsuarioCreado")]
        public int IdUsuarioCreado { get; set; }
        [Column("FechaCreado")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificado")]
        public int? IdUsuarioModificado { get; set; }
        [Column("FechaModificado")]
        public DateTime? FechaModificado { get; set; }

    }
}
