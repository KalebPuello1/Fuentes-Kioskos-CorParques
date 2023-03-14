using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_Indicadores")]
	public class Indicadores
	{

	#region Propiedades

	[Key]
	[Column("IdIndicador")]
	public int IdIndicador { get; set; }
	[Column("Nombre")]
	public string Nombre { get; set; }

	#endregion


	}
}
