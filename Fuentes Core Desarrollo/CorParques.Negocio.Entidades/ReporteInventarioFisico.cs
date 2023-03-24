using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
	public class ReporteInventarioFisico
	{
		public string ObservacionesFisicoR { get; set; }
		public string NombresFisicoR { get; set; }
		public string ArregloTeoricoFisicoR { get; set; }
		public string ArregloInventarioFisicoR { get; set; }
		public string DiferenciasR { get; set; }
		public string TipoMovimientosR { get; set; }
		public string PersonaResponsableR { get; set; }
		public string NombrePuntoR { get; set; }
		public string NombreUsuarioR { get; set; }
		public DateTime FechaInventarioR { get; set; }
		public string CodSapAlmacenR { get; set; }
		public int IdSupervisor { get; set; }
		public int NombreSupervisor { get; set; }

	}
}