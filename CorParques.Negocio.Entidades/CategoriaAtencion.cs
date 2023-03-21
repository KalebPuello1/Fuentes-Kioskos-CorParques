using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_CategoriaAtencion")]
	public class CategoriaAtencion
	{

	#region Propiedades

	[Key]
	[Column("IdCategoriaAtencion")]
	public int IdCategoriaAtencion { get; set; }
	[Column("Nombre")]
	public string Nombre { get; set; }
	[Column("IdEstado")]
	public int IdEstado { get; set; }

	#endregion


	}
}
