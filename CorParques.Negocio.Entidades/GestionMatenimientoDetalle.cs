using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{

    [Table("GESTION_MANTENIMIENTO_DETALLE")]
    public class GestionMantenimientoDetalle
    {
        [Key]
        [Column("IDMANTENIMIENTO_DETALLE")]
        public int Id { get; set; }
        [Column("IDMANTENIMIENTO")]
        public int IdGestion { get; set; }
        [Column("IDMANTENIMIENTO_CONTROL")]
        public int IdControl { get; set; }
        public bool Verificado{ get; set; }
        public String Descripcion { get; set; }

    }
}
    