using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Factura")]
    public class Factura
    {

        #region Propiedades

        [Key]
        [Column("Id_Factura")]
        public int Id_Factura { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("CodSapConvenio")]
        public string CodSapConvenio { get; set; }

        //Nueva propiedad
        [Column("IdConvenio")]
        public int IdConvenio { get; set; }

        [Column("ConsecutivoConvenio")]
        public string ConsecutivoConvenio { get; set; }

        [Column("CodigoFactura")]
        public string CodigoFactura { get; set; }

        [Column("IdPunto")]
        public string IdPunto { get; set; }

        [Column("IdApertura")]
        public string IdApertura { get; set; }

        [Editable(false)]
        public IEnumerable<DetalleFactura> DetalleFactura { get; set; }

        [Editable(false)]
        public IEnumerable<MediosPagoFactura> MediosPago { get; set; }

        #endregion


    }
}
