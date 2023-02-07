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
        #endregion


	}
}
