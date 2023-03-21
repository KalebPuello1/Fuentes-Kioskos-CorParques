using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_TipoAjusteInventario")]
    public class TipoAjusteInventario
    {
        [Key]
        [Column("CodSapAjuste")]
        public string CodSapAjuste { get; set; }

        [Column("Ajuste")]
        public string Ajuste { get; set; }

        [Column("Alta")]
        public bool Alta { get; set; }
    }
}
