using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteFallaAtraccion
    {
        [Column("fechaFalla")]
        public string fechaFalla { get; set; }
        
        [Column("Atraccion")]
        public string atraccion { get; set; }

        [Column("HoraFalla")]
        public string HoraFalla { get; set; }

        [Column("HoraSolucion")]
        public string HoraSolucion { get; set; }

        [Column("TiempoRespuesta")]
        public TimeSpan TiempoRespuesta { get; set; }
        //public string TiempoRespuesta { get; set; }

        [Column("DescripcionFallaAuxiliar")]
        public string DescripcionFallaAuxiliar { get; set; }

        [Column("ObservacionMantenimiento")]
        public string ObservacionMantenimiento { get; set; }

        [Column("orden")]
        public string orden { get; set; }

        [Column("NombreTecnico")]
        public string NombreTecnico { get; set; }

        [Column("Area")]
        public string Area { get; set; }
        
        [Editable(false)]
        public string fechaInicial { get; set; }

        [Editable(false)]
        public string fechaFinal { get; set; }

        [Editable(false)]
        public int idAtraccion { get; set; }

        [Editable(false)]
        public int idArea { get; set; }
    }
}
