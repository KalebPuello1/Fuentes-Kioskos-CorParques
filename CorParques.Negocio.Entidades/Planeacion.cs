using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Planeacion")]
    public class Planeacion
    {

        #region Propiedades

        [Key]
        [Column("IdPlaneacion")]
        public int IdPlaneacion { get; set; }
        [Column("IdIndicador")]
        public int IdIndicador { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("Valor")]
        public decimal Valor { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Editable(false)]
        public IEnumerable<TipoGeneral> ListaIndicadores { get; set; }

        [Editable(false)]
        public string FechaTexto { get; set; }

        [Editable(false)]
        public string ValorTexto { get; set; }

        [Editable(false)]
        public DateTime FechaPlaneacion { get; set; }

        [Editable(false)]
        public IEnumerable<PlaneacionAuxiliar> Planeaciones { get; set; }

        #endregion


    }
}
