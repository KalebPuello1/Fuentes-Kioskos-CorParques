using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_NovedadArqueo")]
    public class NovedadArqueo
    {
        [Key]
        [Column("IdNovedadArqueo")]
        public int Id { get; set; }
        public int IdPunto { get; set; }
        public int IdEstado { get; set; }
        public int IdTaquillero { get; set; }
        public int TipoNovedad { get; set; }
        public double Valor { get; set; }
        public DateTime FechaCreado { get; set; }

        //FALTA ID_APERTURA EN EL MODELO

        public int UsuarioCreado { get; set; }
        public int? UsuarioModificado { get; set; }
        public DateTime? FechaModificado { get; set; }
        public int IdTipoNovedadArqueo { get; set; }
        public string Observaciones { get; set; }
        [Editable(false)]
        public string TipoNovedadNombre { get; set; }

    }
}
