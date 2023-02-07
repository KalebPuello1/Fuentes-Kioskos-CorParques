using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("sec.TB_MenuPerfil")]
    public class MenuPerfil
    {
        public int IdMenu { get; set; }
        public int IdPerfil { get; set; }
    }
}
