using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_UbicacionPunto")]
    public class UbicacionPunto
    {

        #region Propiedades

        [Key]
        [Column("IdUbicacionPunto")]
        public int IdUbicacionPunto { get; set; }
        [Column("IdPunto")]
        public int IdPunto { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }
        [Column("RequiereAuxiliar")]
        public bool RequiereAuxiliar { get; set; }
        #endregion


    }
}
