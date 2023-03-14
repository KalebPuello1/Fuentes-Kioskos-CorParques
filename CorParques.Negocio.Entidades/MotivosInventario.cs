using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_MotivosInventario")]
    public class MotivosInventario
    {
        [Key]
        [Column("CodSapMotivo")]
        public string CodSapMotivo { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("CodSapAjuste")]
        public string CodSapAjuste { get; set; }
        
    }
}
