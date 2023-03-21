using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_CortesiaDestreza")]
    public class CortesiaDestreza
    {

        #region Propiedades

        [Key]
        [Column("IdCortesiaDestreza")]
        public int IdCortesiaDestreza { get; set; }
        [Column("IdPuntoDestreza")]
        public int IdPuntoDestreza { get; set; }
        [Column("IdProducto")]
        public int IdProducto { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Editable(false)]
        public int Entregadas { get; set; }

        [Editable(false)]
        public int PorEntregar { get; set; }

        [Editable(false)]
        public int Cantidad { get; set; }

        [Editable(false)]
        public string Destreza { get; set; }

        [Editable(false)]
        public string Atraccion { get; set; }

        [Editable(false)]
        public int Total { get { return Entregadas + PorEntregar; } }

        [Editable(false)]
        public string Usuario { get; set; }

        public IEnumerable<TipoGeneral> ListaDestrezas { get; set; }
        public IEnumerable<TipoGeneral> ListaAtracciones { get; set; }

        #endregion


    }
}
