using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_TIPOSVEHICULOS")]
    public class TipoVehiculo
    {
        public TipoVehiculo()
        {
            Creado = 1;
            FechaCreado = DateTime.Now;
            Modificado = 0;
            FechaModificado = DateTime.Now;
            Estado = "A";
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("ESTADO")]
        public string Estado { get; set; }
        [Column("CREADO")]
        public int Creado { get; set; }
        [Column("FECCREADO")]
        public DateTime FechaCreado { get; set; }
        [Column("MODIFICADO")]
        public int Modificado { get; set; }
        [Column("FECMODIFCD")]
        public DateTime FechaModificado { get; set; }

    }
}
