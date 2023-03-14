using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("GESTION_MANTENIMIENTO")]
    public class GestionMantenimiento 
    {
        
        [Key]
        [Column("IDMANTENIMIENTO")]
        public int Id { get; set; }
        public int IdAtraccion { get; set; }
        public string Descripcion { get; set; }

    }
}
    