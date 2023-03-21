using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Producto")]
    public class Producto
    {

        #region Propiedades

        [Key]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Precio")]
        public double Precio { get; set; }

        [Column("CodigoSap")]
        public string CodigoSap { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("UsuarioCreacion")]
        public string UsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }
        
        [Column("CtgProducto")]
        public int CtgProducto { get; set; }

        [Editable(false)]
        public int IdTipoBrazalete { get; set; }

        [Editable(false)]
        public string MensajeValidacion { get; set; }

        [Editable(false)]
        public int Codigo { get; set; }
        [Editable(false)]
        public string Imagen { get; set; }
        [Editable(false)]
        public string CodSapTipoProducto { get; set; }
        [Editable(false)]
        public string TipoProducto { get; set; }
        [Editable(false)]
        public decimal PorcentajeImpuesto { get; set; }
        [Editable(false)]
        public string NombreImpuesto { get; set; }
        [Editable(false)]
        public int IdTipoProducto { get; set; }
        [Editable(false)]
        public int IdDetalleProducto { get; set; }
        [Editable(false)]
        public string ConseutivoDetalleProducto { get; set; }
        [Editable(false)]
        public string DataExtension { get; set; }
        [Editable(false)]
        public int Cantidad { get; set; }
        [Editable(false)]
        public double PrecioTotal { get; set; }

        [Editable(false)]
        public IEnumerable<RecetaProducto> objListaRecetaProducto { get; set; }

        [Editable(false)]
        public bool Entregado { get; set; }

        [Editable(false)]
        public string CodBarraInicio { get; set; }

        [Editable(false)]
        public string CodBarraFin { get; set; }

        [Editable(false)]
        public bool Institucional { get; set; }

        [Editable(false)]
        public string Pedido { get; set; }
        #endregion
        [Editable(false)]
        public int IdPuntoDescarga { get; set; }
        [Editable(false)]
        public int Posicion { get; set; }

        [Editable(false)]
        public int ActivoApp { get; set; }

        [Editable(false)]
        public string Descricpion { get; set; }

        [Editable(false)]
        public bool Araza { get; set; }

        [Editable(false)]
        public bool AplicaPunto { get; set; }

        [Editable(false)]
        public int Consecutivo { get; set; }


        [Editable(false)]
        public ArchivoBrazalete ArchivoProducto { get; set; }

        [Editable(false)]
        public int? IdDetallePedido { get; set; }

        [Editable(false)]
        public bool AplicaImpresionLinea { get; set; }


        [Editable(false)]
        public int? Id_TipoAcomp { get; set; }

        public string NombreTipoAcompa { get; set; }

        public int? Id_TipoProdRestaurante { get; set; }
        public string NombreTipoProdRestaurante { get; set; }

        public bool existe { get; set; }
        public bool MostrarTexto { get; set; }

        public string CodSapTipoImpuesto { get; set; }

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

        public string IdPuntos { get; set; }
        public string NombrePuntos { get; set; }

        public List<Puntos> ListaPuntos { get; set; }
        public string hdListPuntos { get; set; }
        

    }
}
