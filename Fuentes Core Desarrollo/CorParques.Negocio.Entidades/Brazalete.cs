using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("CARGUE_BRAZALETE")]
    public class Brazalete
    {
        [Key]
        public int IdBrazalete { get; set; }
        public int IdTipoBrazalete { get; set; }
        public int IdCargueBrazalete { get; set; }
        public double Consecutivo { get; set; }
        public int IdEstado { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? IdUsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
