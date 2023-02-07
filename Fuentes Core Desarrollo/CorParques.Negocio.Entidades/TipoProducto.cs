using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_TipoProducto")]
	public class TipoProducto
	{

	#region Propiedades

	[Key]
	[Column("IdTipoProducto")]
	public int IdTipoProducto { get; set; }
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

	#endregion


	}
}
