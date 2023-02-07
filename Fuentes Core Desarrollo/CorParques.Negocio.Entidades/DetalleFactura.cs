using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_DetalleFactura")]
	public class DetalleFactura
	{

        #region Propiedades
        [Column("IdDetalleFactura"), Key]
        public int IdDetalleFactura { get; set; }

        [Column("Id_Factura")]
        public decimal Id_Factura { get; set; }

        [Column("Id_Producto")]
        public int Id_Producto { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("Precio")]
        public long Precio { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Column("IdNotaCredito")]
        public int IdNotaCredito { get; set; }

        [Column("PrecioNotaCredito")]
        public long PrecioNotaCredito { get; set; }

        [Column("IdDetalleProducto")]
        public int IdDetalleProducto { get; set; }

        [Editable(false)]
        public string Nombre { get; set; }
        [Editable(false)]
        public bool NotaCredito { get; set; }
        [Editable(false)]
        public int IdLineaProducto { get; set; }
        [Editable(false)]
        public string CodigoFactura { get; set; }
        [Editable(false)]
        public string CodigoSap { get; set; }
        [Editable(false)]
        public string Consecutivo { get; set; }

        [Editable(false)]
        public string CodSapTipoProducto { get; set; }

        public DateTime FechaImpresion { get; set; }
        [Column("FechaUsoInicial")]
        public DateTime FechaUsoInicial { get; set; }
        [Column("FechaUsoFinal")]
        public DateTime FechaUsoFinal { get; set; }
        [Column("FechaInicioEvento")]
        public DateTime FechaInicioEvento { get; set; }
        [Column("FechaFinEvento")]
        public DateTime FechaFinEvento { get; set; }
        [Editable(false)]
        public int Estado { get; set; }

        #endregion Propiedades


    }
}
