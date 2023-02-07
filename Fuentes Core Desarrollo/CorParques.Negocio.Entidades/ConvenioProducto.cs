using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_convenioProducto")]
    public class ConvenioProducto
    {
        #region Propiedades

        [Key,Column("IdConvenioProducto")]
        public int IdConvenioProducto { get; set; }

        [Column("IdConvenio")]
        public int IdConvenio { get; set; }

        [Column("CodSapProducto")]
        public string CodSapProducto { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        //[Column("IdUsuarioModificacion")]
        //public int? IdUsuarioModificacion { get; set; }

        //[Column("FechaModificacion")]
        //public DateTime? FechaModificacion { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }
        
        #endregion Propiedades

        #region Métodos 

        #endregion Métodos

        #region Contructor
        #endregion Contructor
    }
}
