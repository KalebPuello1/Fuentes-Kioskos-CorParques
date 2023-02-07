using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_AperturaElementos")]
    public class AperturaElementos
    {
        [Key]
        [Column("IdAperturaElemento")]
        public int Id { get; set; }

        [Column("IdAperturaElementosHeader")]
        public int IdAperturaElementosHeader { get; set; }

        [Column("IdApertura")]
        public int IdApertura { get; set; }

        [Column("IdElemento")]
        public int IdElemento { get; set; }
        public TipoElementos Elemento { get; set; }

        [Column("CodigoBarras")]
        public string CodigoBarras { get; set; }

        [Column("Observacion")]
        public string Observacion { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("EsReabastecimiento")]
        public bool EsReabastecimiento { get; set; }
        [Column("ValidNido")]
        public bool ValidNido { get; set; }
        [Column("ValidSupervisor")]
        public bool ValidSupervisor { get; set; }
        [Column("ValidTaquilla")]
        public bool ValidTaquilla { get; set; }
        [Editable(false)]
        public bool ValidaCierre { get; set; }
        

    }
}
