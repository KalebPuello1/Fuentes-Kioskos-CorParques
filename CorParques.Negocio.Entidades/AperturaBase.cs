using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_AperturaBase")]
	public class AperturaBase
	{

	#region Propiedades

	[Key]
	[Column("IdAperturaBase")]
	public int IdAperturaBase { get; set; }
	[Column("IdApertura")]
	public int IdApertura { get; set; }
	[Column("IdTipoDenominacion")]
	public int IdTipoDenominacion { get; set; }
	[Column("CantidadNido")]
	public double CantidadNido { get; set; }
	[Column("CantidadSupervisor")]
	public double CantidadSupervisor { get; set; }
	[Column("CantidadPunto")]
	public double CantidadPunto { get; set; }
    public double TotalNido { get; set; }
    public double TotalSupervisor { get; set; }
    public double TotalPunto { get; set; }   
        
    
    #endregion


    }
}
