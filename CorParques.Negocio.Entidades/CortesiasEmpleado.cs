using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_CortesiasEmpleado")]
    public class CortesiasEmpleado
    {
        [Column("NumDocumento")]
        public string NumDocumento { get; set; }
        [Column("FechaInicial")]
        public DateTime FechaInicial { get; set; }
        [Column("Cantidad")]
        public int Cantidad { get; set; }
        [Column("Activo")]
        public bool Activo { get; set; }
        public string Nombres { get; set; }
    }
}
