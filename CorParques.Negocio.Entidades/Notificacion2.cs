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
    public class Notificacion2
    {
        [Key]
        [Column("CODNOTFCCN")]
        public int Id { get; set; }

        [Column("ASUNTO")]
        public string Asunto { get; set; }

        [Column("CONTENIDO")]
        public string Contenido { get; set; }

        [Column("PRIORIDAD")]
        public int Prioridad { get; set; }

        public int[] Grupos { get; set; }

        public IEnumerable<Usuario> Correos { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("ESTADO")]
        public string Estado { get; set; }

        [Column("CREADO")]
        public int Creado { get; set; }

        [Column("FECCREADO")]
        public DateTime FechaCreado { get; set; }

        [Column("MODIFICADO")]
        public int? Modificado { get; set; }

        [Column("FECMODIFCD")]
        public DateTime? FechaModificado { get; set; }

        [Column("PARA")]
        public string Para { get; set; }

        [Column("CONCOPIA")]
        public string ConCopia { get; set; }

        [Column("GRUPO")]
        public string GrupoGuardar { get; set; }

        [Editable(false)]
        public string Repuestas { get; set; }

        public string[] OtrosCorreos { get; set; }
    }
}
