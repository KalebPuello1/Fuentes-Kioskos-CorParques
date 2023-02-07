using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_AperturaElementosHeader")]
    public class AperturaElementosHeader
    {
        [Key]
        [Column("IdAperturaElementosHeader")]
        public int Id { get; set; }
        [Column("IdPunto")]
        public int IdPunto { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("ObservacionNido")]
        public string ObservacionNido { get; set; }
        [Column("ObservacionSupervisor")]
        public string ObservacionSupervisor { get; set; }
        [Column("ObservacionPunto")]
        public string ObservacionPunto { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("UsuarioCreado")]
        public int UsuarioCreado { get; set; }
        [Column("FechaCreado")]
        public DateTime FechaCreado { get; set; }
        [Column("UsuarioModificado")]
        public int? UsuarioModificado { get; set; }
        [Column("FechaModificado")]
        public DateTime? FechaModificado { get; set; }
        [Column("IdSupervisor")]
        public int IdSupervisor { get; set; }
        [Column("IdTaquillero")]
        public int IdTaquillero { get; set; }
    }
}
