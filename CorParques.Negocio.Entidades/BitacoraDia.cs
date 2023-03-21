using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_BitacoraDia")]
    public class BitacoraDia
    {
        [Key]
        [Column("IdBitacora")]
        public int IdBitacora { get; set; }
        [Column("IdClimaFK")]
        public int IdClimaFK { get; set; }
        [Column("IdSegmentoDiaFK")]
        public int IdSegmentoDiaFK { get; set; }
        [Column("Fecha")]
        public DateTime? Fecha { get; set; }
        [Column("Observacion")]
        public string Observacion { get; set; }
        [Editable(false)]
        public string Mensaje { get; set; }
        [Editable(false)]
        public int? CantidadPersonans { get; set; }

        /// <summary>
        /// RDSH: Se adiciona para presentar en dashboard
        /// </summary>
        [Editable(false)]
        public string Clima { get; set; }

        /// <summary>
        /// RDSH: Se adiciona para presentar en dashboard
        /// </summary>
        [Editable(false)]
        public string SegmentoDia { get; set; }

    }
}
