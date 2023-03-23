using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_Materiales")]
	public class Materiales
	{

		#region Propiedades

		[Key]
		[Column("IdMaterial")]
		public int Id { get; set; }
		[Column("Nombre")]
		public string Nombre { get; set; }
		[Column("CodigoSap")]
		public string CodigoSap { get; set; }
		[Column("FechaCreacion")]
		public DateTime FechaCreacion { get; set; }
		[Column("UsuarioCreacion")]
		public string UsuarioCreacion { get; set; }
		[Column("FechaModificacion")]
		public DateTime FechaModificacion { get; set; }
		[Column("UsuarioModificacion")]
		public string UsuarioModificacion { get; set; }
		[Editable(false)]
		public double CantidadDisponible { get; set; }
		[Editable(false)]
		public double Cantidad { get; set; }
		[Editable(false)]
		public string Unidad { get; set; }
		[Column("CostoPromedio")]
		public double CostoPromedio { get; set; }

		[Editable(false)]
		public string CodSapAjuste { get; set; }

		[Editable(false)]
		public string CodSapMotivo { get; set; }

		[Editable(false)]
		public string Observaciones { get; set; }

		public string ObservacionesFisico { get; set; }
		public string NombresFisico { get; set; }
		public string ArregloTeoricoFisico { get; set; }
		public string ArregloInventarioFisico { get; set; }
		public string Diferencias { get; set; }
		public string TipoMovimientos { get; set; }

		public string CantidadFisica { get; set; }
		public string CantidadTeorica { get; set; }
		public string Diferencia { get; set; }
		public string Observacion { get; set; }
		public string CodigoSapAlmacen { get; set; }
		public string NombrePunto { get; set; }
		public string NombreUsuario { get; set; }
		public DateTime FechaInventario { get; set; }
		public string CodSapAlmacen { get; set; }
		public string TipoMovimiento { get; set; }

		public int IdUsuario { get; set; }
		public string Tipo_Mov { get; set; }
		public int id_Punto { get; set; }
		public int Id_Supervisor { get; set; }
		public string NombreSupervisor { get; set; }


		#endregion


	}
}
