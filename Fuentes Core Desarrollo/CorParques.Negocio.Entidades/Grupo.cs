using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_GRUPO")]
    public class Grupo
    {
        

        [Key]
        [Column("CODGRUPO")]
        public int Id { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }
        [Column("IdEstado")]
        public int EstadoId { get; set; }
        [Editable(false)]
        public bool Prioritario { get; set; }
        [Editable(false)]
        public string Estado { get; set; }
        [Column("CREADO")]
        public int Creado { get; set; }
        [Column("FECCREADO")]
        public DateTime FechaCreado { get; set; }
        [Column("MODIFICADO")]
        public int Modificado { get; set; }
        [Column("FECMODIFCD")]
        public DateTime FechaModificado { get; set; }
        public IEnumerable<Puntos> Puntos { get; set; }
        [Editable(false)]
        public string PuntosAsociados { get; set; }
        public IEnumerable<TipoGeneral> ListaEstados { get; set; }
        [Editable(false)]
        public bool ValidaNombre { get; set; }
    }
}
