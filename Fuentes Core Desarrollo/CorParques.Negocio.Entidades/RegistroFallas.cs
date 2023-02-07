using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class RegistroFallas
    {
        #region Propiedades
        public int idPunto { get; set; }
        public int idArea { get; set; }
        public int idOrdenFalla { get; set; }
        public string descripcion { get; set; }
        public string tecnico { get; set; }
        public DateTime fechaFalla { get; set; }
        public TimeSpan horaFalla { get; set; }
        public string observacionTecnica { get; set; }
        public DateTime fechaRespuesta { get; set; }
        public TimeSpan horaRespuesta { get; set; }
        public DateTime fechaLlegadaTec { get; set; }
        public TimeSpan horaLlegadaTec { get; set; }

         
        #endregion
    }
}
