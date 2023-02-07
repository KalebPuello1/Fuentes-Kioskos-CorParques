using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_DetalleRecoleccionDocumento")]
    public class DetalleRecoleccionDocumento
    {

        #region Propiedades

        [Key]
        [Column("IdDetalleRecoleccionDocumento")]
        public int IdDetalleRecoleccionDocumento { get; set; }
        [Column("IdRecoleccion")]
        public int IdRecoleccion { get; set; }
        [Column("IdTipoRecoleccion")]
        public int IdTipoRecoleccion { get; set; }
        [Column("IdMedioPagoFactura")]
        public int IdMedioPagoFactura { get; set; }
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
        [Column("IdUsuarioSupervisor")]
        public int IdUsuarioSupervisor { get; set; }
        [Column("IdUsuarioNido")]
        public int IdUsuarioNido { get; set; }
        [Column("RevisionTaquillero")]
        public bool RevisionTaquillero { get; set; }
        [Column("RevisionSupervisor")]
        public bool RevisionSupervisor { get; set; }
        [Column("RevisionNido")]
        public bool RevisionNido { get; set; }
        [Column("NumeroSobre")]
        public string NumeroSobre { get; set; }

        [Editable(false)]
        public string Tipo { get; set; }

        [Editable(false)]
        public string NumReferencia { get; set; }

        [Editable(false)]
        public string Franquicia { get; set; }

        [Editable(false)]
        public string Fecha { get; set; }

        [Editable(false)]
        public long Valor { get; set; }

        #endregion


    }
}
