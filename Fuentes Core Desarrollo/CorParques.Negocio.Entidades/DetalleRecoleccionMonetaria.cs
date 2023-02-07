using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_DetalleRecoleccionMonetaria")]
    public class DetalleRecoleccionMonetaria
    {

        #region Propiedades

        [Key]
        [Column("IdDetalleRecoleccionMonetaria")]
        public int IdDetalleRecoleccionMonetaria { get; set; }
        [Column("IdRecoleccion")]
        public int IdRecoleccion { get; set; }
        [Column("IdTipoRecoleccion")]
        public int IdTipoRecoleccion { get; set; }
        [Column("IdTipoDenominacion")]
        public int IdTipoDenominacion { get; set; }
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
        [Column("CantidadTaquillero")]
        public int CantidadTaquillero { get; set; }
        [Column("CantidadSupervisor")]
        public int CantidadSupervisor { get; set; }
        [Column("CantidadNido")]
        public int CantidadNido { get; set; }
        [Column("NumeroSobre")]
        public string NumeroSobre { get; set; }


        [Editable(false)]
        public string Tipo { get; set; }

        [Editable(false)]
        public string Denominacion { get; set; }


        #endregion


    }
}
