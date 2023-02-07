using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_Espacio")]
	public class Espacio
	{

	#region Propiedades

	[Key]
	[Column("IdEspacio")]
	public int IdEspacio { get; set; }
	[Column("IdTipoEspacio")]
	public int IdTipoEspacio { get; set; }
	[Column("Nombre")]
	public string Nombre { get; set; }
	[Column("IdEstado")]
	public int IdEstado { get; set; }

	#endregion


	}
}
