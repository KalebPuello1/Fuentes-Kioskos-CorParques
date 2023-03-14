using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("TB_ResolucionFactura")]
	public class ResolucionFactura
    {

        #region Propiedades

        public int IdResolucion { get; set; }
        public string Resolucion { get; set; }
        public int IdPunto { get; set; }
        public string NombrePunto { get; set; }
        public string Prefijo { get; set; }
        public int ConsecutivoInicial { get; set; }
        public int ConsecutivoFinal { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string UltimaFactura { get; set; }
        public int Usuario { get; set; }
        public int IdEstado { get; set; }
        public int Restante { get; set; }

        #endregion


    }
}
