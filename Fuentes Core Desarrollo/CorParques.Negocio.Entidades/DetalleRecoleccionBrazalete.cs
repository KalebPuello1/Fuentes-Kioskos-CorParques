using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_DetalleRecoleccionBrazalete")]
    public class DetalleRecoleccionBrazalete
    {

        #region Propiedades

        [Key]
        [Column("IdDetalleRecoleccionBrazalete")]
        public int IdDetalleRecoleccionBrazalete { get; set; }
        [Column("IdRecoleccion")]
        public int IdRecoleccion { get; set; }
        [Column("IdTipoBrazalete")]
        public int IdTipoBrazalete { get; set; }
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

        [Column("IdAperturaBrazaleteDetalle")]
        public int IdAperturaBrazaleteDetalle { get; set; }

        [Editable(false)]
        public string TipoBrazalete { get; set; }

        [Editable(false)]
        public int Asignados { get; set; }

        [Editable(false)]
        public int TotalVendidos { get; set; }

        [Editable(false)]
        public string NumeroSobre { get; set; }

        [Editable(false)]
        public string UnidadMedida { get; set; }

        [Editable(false)]
        public string CodigoSapProducto { get; set; }

        #endregion


    }
}
