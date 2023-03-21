using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("Tb_RetornoDetalle")]
    public class RetornoDetalle
    {
        [Key]
        [Column("IdRetornoDetalle")]
        public int IdRetornoDetalle { get; set; }
        [Column("NumeroOrden")]
        public long NumeroOrden { get; set; }
        [Column("NumeroOperacion")]
        public int NumeroOperacion { get; set; }
        [Column("Observacion")]
        public string Observacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("Aprobado")]
        public bool Aprobado { get; set; }
        [Column("TiempoSolucion")]
        public int TiempoSolucion { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        public bool Procesado { get; set; }

        public int IdOrdenMantenimiento { get; set; }
    }
}
