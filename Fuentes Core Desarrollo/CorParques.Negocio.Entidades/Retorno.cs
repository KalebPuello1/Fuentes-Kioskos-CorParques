using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("Tb_Retorno")]
    public class Retorno
    {
        [Key]
        [Column("IdRetorno")]
        public int? IdRetorno { get; set; }
        [Column("NumeroOrden")]
        public long? NumeroOrden { get; set; }
        [Column("Aprobado")]
        public int? Aprobado { get; set; }
        [Column("Observacion")]
        public string Observacion { get; set; }
        [Column("Procesado")]
        public int? Procesado { get; set; }

        public int? IdPunto { get; set; }
        public int? IdUsuarioAprobador { get; set; }
        public int? IdOperaciones { get; set; }
        public string CodSapPunto { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        IEnumerable<RetornoDetalle> objRetornoDetalle { get; set; }
    }
}
