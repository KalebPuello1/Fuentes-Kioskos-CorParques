using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Recambio")]
    public class Recambio
    {

        #region Propiedades

        [Key]
        [Column("IdRecambio")]
        public int IdRecambio { get; set; }
        [Column("IdUsuarioAsignacion")]
        public int IdUsuarioAsignacion { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("Valor")]
        public int Valor { get; set; }
        [Column("ObservacionRecambio")]
        public string ObservacionRecambio { get; set; }
        [Column("ObservacionAprovado")]
        public string ObservacionAprovado { get; set; }

        #endregion


    }
}
