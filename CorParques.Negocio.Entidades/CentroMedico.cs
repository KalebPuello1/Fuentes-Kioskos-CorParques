using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CorParques.Negocio.Entidades
{
    [Table("TB_CentroMedico")]
    public class CentroMedico
    {

        #region Propiedades

        [Key]
        [Column("IdCentroMedico")]
        public int IdCentroMedico { get; set; }
        [Column("Ubicacion")]
        public string Ubicacion { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Editable(false)]
        public int Id { get; set; }

        [Editable(false)]
        public string Nombre { get; set; }
        #endregion


    }
}
