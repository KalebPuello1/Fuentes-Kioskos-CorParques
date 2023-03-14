using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("Tb_Operaciones")]
    public class Operaciones
    {
        [Key]
        [Column("IdOperaciones")]
        public int IdOperaciones { get; set; }
        [Column("NumeroOrden")]
        public long NumeroOrden { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; }
        [Column("NumeroOperacion")]
        public int NumeroOperacion { get; set; }
        [Column("Procesado")]
        public bool Procesado { get; set; }

        public IEnumerable<OrdenMantenimiento> objOrdenMantenimiento { get; set; }
    }
}
