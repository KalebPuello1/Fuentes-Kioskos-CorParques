using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_CargueBoleteria")]
    public class CargueBoleteria
    {

        #region Propiedades

        [Key]
        [Column("IdCargueBoleteria")]
        public int IdCargueBoleteria { get; set; }
        [Column("IdProducto")]
        public int IdProducto { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; }
        [Column("ConsecutivoInicial")]
        public long ConsecutivoInicial { get; set; }
        [Column("ConsecutivoFinal")]
        public long ConsecutivoFinal { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int? IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Column("FechaInicioVigencia")]
        public DateTime FechaInicioVigencia { get; set; }

        [Column("FechaFinVigencia")]
        public DateTime FechaFinVigencia { get; set; }

        [Editable(false)]
        public string Producto { get; set; }

        [Editable(false)]
        public string Estado { get; set; }

        public IEnumerable<TipoGeneral> TipoBoleteria { get; set; }

        [Editable(false)]
        public string FechaInicioVigenciaDDMMAAAA { get; set; }

        [Editable(false)]
        public string FechaFinVigenciaDDMMAAAA { get; set; }

        #endregion


    }
}
