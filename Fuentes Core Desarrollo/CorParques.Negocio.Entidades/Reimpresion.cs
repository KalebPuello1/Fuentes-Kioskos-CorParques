using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class Reimpresion
    {
        #region Propiedades

        [Key]
        [Column("IdFactura")]
        public int IdFactura { get; set; }
        [Column("CodigoFactura")]
        public string CodigoFactura { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("Punto")]
        public int Punto { get; set; }
        [Column("Productos")]
        public string Productos { get; set; }
        [Column("NombrePunto")]
        public string NombrePunto { get; set; }
        #endregion
    }
}
