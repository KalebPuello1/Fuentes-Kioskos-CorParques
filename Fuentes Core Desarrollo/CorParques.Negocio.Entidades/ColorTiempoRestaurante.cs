using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
   public class ColorTiempoRestaurante
    {
        public int IdColorTiempo { get; set; }
        public TimeSpan TiempoIntervalo { get; set; }
        public string ColorCSS { get; set; }
        public int Intermitencia { get; set; }

    }
}
