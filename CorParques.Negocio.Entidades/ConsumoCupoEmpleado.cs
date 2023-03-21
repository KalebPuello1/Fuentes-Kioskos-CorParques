using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_ConsumoCupoEmpleado")] 
    public class ConsumoCupoEmpleado
    {
        [Key]
        [Column("IdConsumoCupoEmpleado")]
        public int IdConsumoCupoEmpleado { get; set; }

        [Column("CodigoSapEmpleado")]
        public string CodigoSapEmpleado { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("Valor")]
        public decimal Valor { get; set; }

        [Column("Procesado")]
        public bool Procesado { get; set; }

    }
}
