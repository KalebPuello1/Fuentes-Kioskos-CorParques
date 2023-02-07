using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_CierreElementos")]
    public class CierreElementos
    {
        [Key]
        [Column("IdCierreElemento")]
        public int Id { get; set; }

        [Column("IdAperturaElementosHeader")]
        public int IdAperturaElementosHeader { get; set; }

        [Column("IdElemento")]
        public int IdElemento { get; set; }
        public TipoElementos Elemento { get; set; }
      
        public string  ObservacionesNido { get; set; }
        public string ObservacionesSupervisor { get; set; }
        public string ObservacionesPunto { get; set; }

        public int IdAperturaElemento { get; set; }
        public int? IdEstadoNido { get; set; }
        public int? IdEstadoSupervisor { get; set; }
        public int IdEstadoPunto { get; set; }
        public int? IdUsuarioNido { get; set; }
        public int? IdUsuarioSupervisor { get; set; }
        public int IdUsuarioPunto { get; set; }
        public DateTime FechaCreacion { get; set; }

        public IEnumerable<TipoGeneral> Estados { get; set; }

        [Editable(false)]
        public string EstadoPunto { get; set; }
        [Editable(false)]
        public string EstadoSupervisor { get; set; }
        [Editable(false)]
        public string EstadoNido { get; set; }
        [Editable(false)]
        public string CodigoBarras { get; set; }
        [Editable(false)]
        public string NombreElemento { get; set; }
        [Editable(false)]
        public string Estado { get; set; }
        [Editable(false)]
        public string Observacion { get; set; }
    }
}
