using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_PlazoCortesia")]
    public class PlazoCortesia
    {
        #region Propiedades

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Bandera")]
        public int Bandera { get; set; }
        [Column("DiasVigencia")]
        public int DiasVigencia { get; set; }
        [Column("Estado")]
        public bool Estado { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        #endregion
    }
}
