using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("tb_puntobrazalete")]
    public class PuntoBrazalete
    {
        [Key]
        [Column("IdPuntoBrazalete")]
        public int Id { get; set; }

        [Column("IdPunto")]
        public int IdPunto { get; set; }

        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("UsuarioCreacion")]
        public string UsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("UsuarioModificacion")]
        public string UsuarioModifica { get; set; }
        
        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }
    }
}
