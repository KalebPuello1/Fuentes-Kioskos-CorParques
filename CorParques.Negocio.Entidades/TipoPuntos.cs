using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace CorParques.Negocio.Entidades
{
    
    [Table("TB_TipoPunto", Schema ="sec")]
    public class TipoPuntos
    {
        [Column("IdTipoPunto"), Key]
        public int Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }
                
        public int UsuarioCreado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificado { get; set; }        
        public DateTime FechaModificado { get; set; }       
    }
}
