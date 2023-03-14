using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    public class ObtenerFacturaReagendamiento
    {

        #region Propiedades


        public IEnumerable<Factura> Factura { get; set; }
        public IEnumerable<DetalleFactura> DetalleFactura { get; set; }
        public IEnumerable<Boleteria> Boleteria { get; set; }
        public IEnumerable<MedioPagoFactura> MedioPagoFactura { get; set; }
        public IEnumerable<Producto> Productos { get; set; }


        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("IdPunto")]
        public string IdPunto { get; set; }

        [Column("IdDetalleFactura")]
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

        public string Nombre { get; set; }
        public int IdLineaProducto { get; set; }
        public string CodigoFactura { get; set; }
        public string CodigoSap { get; set; }

        [Column("IdBoleteria")]
        public int IdBoleteria { get; set; }
        [Column("IdProducto")]
        public int IdProducto { get; set; }
        [Column("Consecutivo")]
        public string Consecutivo { get; set; }
        [Column("IdSolicitudBoleteria")]
        public DateTime FechaImpresion { get; set; }
        [Column("FechaUsoInicial")]
        public DateTime FechaUsoInicial { get; set; }
        [Column("FechaUsoFinal")]
        public DateTime FechaUsoFinal { get; set; }
        [Column("FechaInicioEvento")]
        public DateTime FechaInicioEvento { get; set; }
        [Column("FechaFinEvento")]
        public DateTime FechaFinEvento { get; set; }
        [Column("IdUsuarioCreacion")]

        [Editable(false)]
        public string NombreProducto { get; set; }

        [Editable(false)]
        public int CantidadPasaportes { get; set; }

        [Editable(false)]
        public int UsosPasaportes { get; set; }
        [Editable(false)]

        public int AtraccionesDisponibles { get; set; }

        [Editable(false)]
        public int ReagendamientoPasaportes { get; set; }

        [Editable(false)]
        public DateTime FechaBoleteria { get; set; }

        [Editable(false)]
        public int Id_Estado { get; set; }

        [Editable(false)]
        public string FechaEditada { get; set; }

        [Editable(false)]
        public int ProductoId { get; set; }

        #endregion Propiedades
    }
}
