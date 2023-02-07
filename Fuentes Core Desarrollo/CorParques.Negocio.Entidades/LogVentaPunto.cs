using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    //Tabla base de datos local 

    [Table("TB_LogVentasPunto")]
    public class LogVentaPunto
    {
        [Column("Id"), Key]
        public int Id { get; set; }

        [Column("Idpunto")]
        public int Idpunto { get; set; }

        [Column("IdTaquillero")]
        public int IdTaquillero { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("CodigoFactura")]
        public string CodigoFactura { get; set; }
    }
}
