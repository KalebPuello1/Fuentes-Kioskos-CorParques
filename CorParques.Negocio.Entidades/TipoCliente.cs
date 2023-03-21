using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_TipoCliente")]
	public class TipoCliente
	{

	#region Propiedades

	[Column("IdTipoCliente"), Key]
	public int IdTipoCliente { get; set; }

	[Column("Nombre")]
	public string Nombre { get; set; }

	#endregion


	}
}
