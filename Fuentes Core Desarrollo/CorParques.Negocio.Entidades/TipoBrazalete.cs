using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_TIPO_BRAZALETE")]
    public class TipoBrazalete
    {
        [Key]
        [Column("IdTipoBrazalete")]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public double Precio { get; set; }

        public IEnumerable<TipoGeneral> Atracciones { get; set; }

        public IEnumerable<TipoGeneral> AtraccionesBrazalete { get; set; }

        public IEnumerable<TipoGeneral> Estados { get; set; }

        public string AtraccionesSeleccionadas { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        [Column("UsuarioCreado")]
        public int UsuarioCreacion { get; set; }
        [Column("FechaCreado")]
        public DateTime FechaCreacion { get; set; }
        [Column("UsuarioModificado")]
        public int? UsuarioModicifacion { get; set; }
        [Column("FechaModificado")]
        public DateTime? FechaModificacion { get; set; }
        [Editable(false)]
        public int CantInventario { get; set; }
        [Editable(false)]
        public string CodigoSap { get; set; }
    }
}
