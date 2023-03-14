using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TIPO_TARIFA_PARQUEADERO")]
    public class TipoTarifaParqueadero
    {

        [Key]
        [Column("IdTipoTarifaParqueadero")]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int  IdEstado { get; set; }

        [Editable(false)]
        public string Estado { get; set; }

        [Column("IdUsuarioCreacion")]
        public int UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public IEnumerable<TipoGeneral> ListaEstados { get; set; }

    }
}
