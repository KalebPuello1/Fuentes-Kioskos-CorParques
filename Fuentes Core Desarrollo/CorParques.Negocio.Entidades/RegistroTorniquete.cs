using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_RegistroTorniquete")]
	public class RegistroTorniquete
	{

	#region Propiedades

	[Key]
	[Column("IdRegistroTorniquete")]
	public int IdRegistroTorniquete { get; set; }
	[Column("IdPunto")]
	public int IdPunto { get; set; }
	[Column("Inicio")]
	public int Inicio { get; set; }
	[Column("Fin")]
	public int Fin { get; set; }
	[Column("IdUsuarioCreacion")]
	public int IdUsuarioCreacion { get; set; }
	[Column("FechaCreacion")]
	public DateTime FechaCreacion { get; set; }
	[Column("IdUsuarioModificacion")]
	public int? IdUsuarioModificacion { get; set; }
	[Column("FechaModificacion")]
	public DateTime? FechaModificacion { get; set; }

	#endregion


	}
}
