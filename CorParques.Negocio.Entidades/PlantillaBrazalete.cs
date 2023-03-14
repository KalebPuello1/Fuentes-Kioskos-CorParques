using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_PLANTILLABRAZALETE")]
    public class PlantillaBrazalete
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("OBSERVACION")]
        public string Observacion { get; set; }

        [Column("FECHAINICIO")]
        public DateTime FechaInicio { get; set; }

        [Column("FECHAFIN")]
        public DateTime FechaFin { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }

    }
}
