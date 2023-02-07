using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_RecetaProducto")]
    public class RecetaProducto
    {

        #region Propiedades

        [Key]
        [Column("IdRecetaProducto")]
        public int IdRecetaProducto { get; set; }
        [Column("CodSapMaterial")]
        public string CodSapMaterial { get; set; }
        [Column("CodSapProducto")]
        public string CodSapProducto { get; set; }
        [Column("Cantidad")]
        public double Cantidad { get; set; }

        public string Material { get; set; }


        #endregion


    }
}
