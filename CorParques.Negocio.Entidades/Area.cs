using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_Area")]
	public class Area
	{

	#region Propiedades

	[Key]
	[Column("idArea")]
	public int IdArea { get; set; }
	[Column("descripcion")]
	public string descripcion { get; set; }
	//[Column("IdEstado")]
	//public int IdEstado { get; set; }
	//[Column("IdUsuarioCreacion")]
	//public int IdUsuarioCreacion { get; set; }
	//[Column("FechaCreacion")]
	//public DateTime FechaCreacion { get; set; }
	//[Column("IdUsuarioModificacion")]
	//public int IdUsuarioModificacion { get; set; }
	//[Column("FechaModificacion")]
	//public DateTime FechaModificacion { get; set; }

	#endregion


	}
}
