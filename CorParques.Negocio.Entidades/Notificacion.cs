using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_NOTIFICACION")]
    public class Notificacion
    {
        [Column("IdNotificacion")]
        [Key]
        public int Id { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public int IdPunto { get; set; }
        public int IdEstado { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public bool Prioritario { get; set; }
        public int? UsuarioModificacion { get; set; }
        [Editable(false)]
        public string ListaGrupos { get; set; } 
        public DateTime? FechaModificacion { get; set; }
    }
}
