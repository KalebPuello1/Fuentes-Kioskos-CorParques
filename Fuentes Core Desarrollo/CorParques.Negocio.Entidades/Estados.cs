using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("ESTADO")]
    public class Estados
    {
        [Key]
        [Column("CODESTADO")]
        public int Id { get; set; }
        [Column("DESCRIPCION")]
        public string Nombre { get; set; }             
        [Column("CREADO")]
        public int UsuarioCreacion { get; set; }
        [Column("FECCREADO")]
        public DateTime FechaCreacion { get; set; }
        [Column("MODIFICADO")]
        public int? UsuarioModificacion { get; set; }
        [Column("FECMODIFCD")]
        public DateTime? FechaModificacion { get; set; }
    }
}
