using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    
    [Table("TB_SerieCodigoBarras")]
    public class SerieCodigoBarras
    {

        #region Propiedades

        [Key]
        [Column("IdConsecutivo")]
        public int Id { get; set; }

        [Column("Consecutivo")]
        public string Consecutivo { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }
        
        #endregion Propiedades

        #region Constructor
        #endregion Constructor

        #region Metodos
        #endregion Metodos

    }
}
