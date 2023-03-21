using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("sec.TB_PuntoUsuario")]
    public class PuntoUsuario
    {
        public int IdPunto { get; set; }
        public int IdUsuario { get; set; }
    }
}
