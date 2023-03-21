using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_TipoReserva")]
    public class TipoReserva
    {

        #region Propiedades

        [Key]
        [Column("IdTipoReserva")]
        public int IdTipoReserva { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Color")]
        public string Color { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }

        #endregion


    }
}
