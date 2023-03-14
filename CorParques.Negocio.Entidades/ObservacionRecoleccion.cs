using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_ObservacionRecoleccion")]
	public class ObservacionRecoleccion
	{

	#region Propiedades

	[Key]
	[Column("IdObservacionRecoleccion")]
	public int IdObservacionRecoleccion { get; set; }
	[Column("IdRecoleccion")]
	public int IdRecoleccion { get; set; }
	[Column("IdUsuarioCreacion")]
	public int IdUsuarioCreacion { get; set; }
	[Column("FechaCreacion")]
	public DateTime FechaCreacion { get; set; }
	[Column("Observacion")]
	public string Observacion { get; set; }

	#endregion


	}
}
