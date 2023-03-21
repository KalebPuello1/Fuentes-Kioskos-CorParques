using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_MediosPagoFactura")]
    public class MediosPagoFactura
    {

        #region Propiedades

        [Column("IdMedioPago"), Key]
        public int IdMedioPago { get; set; }

        [Column("Id_Factura")]
        public int Id_Factura { get; set; }

        [Column("Valor")]
        public long Valor { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Column("IdFranqicia")]
        public int IdFranqicia { get; set; }

        [Column("Cambio")]
        public long Cambio { get; set; }
        [Editable(false)]
        public int IdMedioPagoFactura { get; set; }

        [Editable(false)]
        public string NumReferencia { get; set; }

        [Editable(false)]
        public string Franquicia { get; set; }

        [Editable(false)]
        public int Tipo { get; set; }

        #endregion


    }
}
