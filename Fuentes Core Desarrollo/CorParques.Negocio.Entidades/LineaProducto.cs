using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_TipoProducto")]
    public class LineaProducto
    {
        [Column("IdTipoProducto"), Key]
        public int Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("CodigoSap")]
        public string CodigoSap { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
    }
}
