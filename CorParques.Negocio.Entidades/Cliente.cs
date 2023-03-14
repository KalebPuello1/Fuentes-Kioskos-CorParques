using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Cliente")]
    public class Cliente
    {

        #region Propiedades

        [Key]
        [Column("IdCliente")]
        public int IdCliente { get; set; }
        [Column("CodSapCliente")]
        public string CodSapCliente { get; set; }
        [Column("Nombres")]
        public string Nombres { get; set; }
        [Column("TipoDocumento")]
        public string TipoDocumento { get; set; }
        [Column("Documento")]
        public string Documento { get; set; }

        #endregion


    }
}
