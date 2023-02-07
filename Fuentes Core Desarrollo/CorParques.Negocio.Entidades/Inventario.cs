using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_Inventario")]
	public class Inventario
	{

	#region Propiedades

	[Key]
	[Column("Id")]
	public int Id { get; set; }
	[Column("CodSapAlmacen")]
	public string CodSapAlmacen { get; set; }
	[Column("CodSapMaterial")]
	public string CodSapMaterial { get; set; }
	[Column("Cantidad")]
	public int Cantidad { get; set; }
	[Column("UnidadMedida")]
	public string UnidadMedida { get; set; }
	[Column("FechaInventario")]
	public DateTime FechaInventario { get; set; }
    [Editable(false)]
    public int IdPunto { get; set; }
    [Editable(false)]
    public int IdUsuarioCeado { get; set; }
    public IEnumerable<Producto> Productos { get; set; }
	public string NProducto { get; set; }
	public string arregloTeorico { get; set; }
	public string arregloCantidadDisponible { get; set; }
	public string Diferencia { get; set; }
	public string arregloTipoMovimiento { get; set; }
	public string Motivo { get; set; }
	public string Observacion { get; set; }
	public string Perfil { get; set; }
    	#endregion


	}
}
