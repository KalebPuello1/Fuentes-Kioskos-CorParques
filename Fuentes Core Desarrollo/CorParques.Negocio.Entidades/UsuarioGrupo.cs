using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_USUGRUPO")]
    public class UsuarioGrupo
    {
        [Key]
        [Column("Id_usuario_Grupo")]
        public int Id { get; set; }
        public int Idgrupo { get; set; }
        public int IdUsuario { get; set; }
        
        public IEnumerable<TipoGeneral> Grupos { get; set; }
        
        public IEnumerable<Usuario> Usuarios { get; set; }
        
      
    }
}
    