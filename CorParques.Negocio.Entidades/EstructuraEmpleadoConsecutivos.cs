using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_EstructuraEmpleadoConsecutivos")]
    public class EstructuraEmpleadoConsecutivos
    {
        [Key]
        [Column("IdEstructuraEmpleadoConsecutivos")]
        public int IdEstructuraEmpleadoConsecutivos { get; set; }
        [Column("CodigoSapEmpleado")]
        public string CodigoSapEmpleado { get; set; }
        [Column("Consecutivo")]
        public string Consecutivo { get; set; }
    }
}
