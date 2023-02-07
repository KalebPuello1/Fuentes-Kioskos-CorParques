using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_PARAMETROS")]
    public class Parametro
    {
        [Key]
        [Column("CODPARAMETRO")]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public int Creado { get; set; }
        [Column("FECCREADO")]
        public DateTime FechaCreacion { get; set; }
        public int? Modificado { get; set; }
        [Column("FECMODIFCD")]
        public DateTime? FechModificacion { get; set; }
    }
}
