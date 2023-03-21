using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class AperturaElementosInsert
    {
        public int IdPunto { get; set; }
        public string Fecha { get; set; }
        public List<AperturaElementos> Elementos { get; set; }
        public int IdUsuario { get; set; }

    }
}
