using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_TipoDenominacion")]
	public class TipoDenominacion
	{

	#region Propiedades
    [Key]
	[Column("IdTipoDenominacion")]
	public int IdTipoDenominacion { get; set; }
	[Column("Tipo")]
	public string Tipo { get; set; }
	[Column("Denominacion")]
	public string Denominacion { get; set; }
    public int IdEstado { get; set; }

        #endregion


    }
}
