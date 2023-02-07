using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_TipoElementos")]
    public class TipoElementos
    {
        [Key]
        [Column("IdTipoElmento")]
        public int Id { get; set; }

        [Column("NombreTipoElemento")]
        public string Nombre { get; set; }
    }
}
