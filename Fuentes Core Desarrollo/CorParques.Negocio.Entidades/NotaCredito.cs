using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_NotaCredito")]
    public class NotaCredito
    {
        #region Propiedades

        [Column("IdNotaCredito"), Key]
        public int Id { get; set; }

        [Column("Observacion")]
        public string Observacion { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("IdSupervisor")]
        public int IdSupervisor { get; set; }

        [Column("IdPunto")]
        public int IdPunto { get; set; }

        public List<DetalleFactura> DetalleFactura { get; set; }
                
        #endregion Propiedades

        #region Metodos
        #endregion Metodos

        #region Constructor 
        #endregion Constructor

    }
}
