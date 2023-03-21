using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Franquicias")]
    public class Franquicia
    {

        #region Propiedades

        [Column("IdFranquicia"), Key]
        public int Id { get; set; }

        [Column("NomFranquicia")]
        public string Nombre { get; set; }

        #endregion Propiedades

        #region Metodos 
        #endregion Metodos 

        #region Constructor 
        #endregion Constructor

    }
}
