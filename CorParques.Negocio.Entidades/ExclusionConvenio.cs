using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_ConvenioExclusion")]
    public class ExclusionConvenio
    {
        #region Constructor
        #endregion Constructor

        #region Propiedades

        [Column("IdConvenioExclusion"), Key]
        public int Id { get; set; }

        [Column("FechaInicio")]
        public DateTime FechaInicio { get; set; }

        [Editable(false)]
        public string strFechaInicio { get; set; }

        [Column("FechaFin")]
        public DateTime FechaFin { get; set; }

        [Editable(false)]
        public string strFechaFin { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; } 

        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("IdConvenio")]
        public int IdConvenio { get; set; }

        #endregion Propiedades

        #region Métodos 

        #endregion Métodos 
    }
}
