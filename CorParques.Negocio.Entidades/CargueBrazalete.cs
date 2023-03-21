using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("CARGUE_BRAZALETE")]
    public class CargueBrazalete
    {
        [Key]
        public int IdCargueBrazalete { get; set; }
        [Column("IdTipoBrazalete")]
        public int IdTipoBrazalete { get; set; } 
        public string Descripcion { get; set; }
        public double ConsecutivoInicial { get; set; }
        public double ConsecutivoFinal { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int? UsuarioModificacion { get; set; }        
        public DateTime? FechaModificacion { get; set; }
        public IEnumerable<TipoGeneral> TiposBrazaletes { get; set; }        
        public string TipoBrazalete { get; set; }        
        public string Estado { get; set; }
        public string MensajeError { get; set; }
    }
}
