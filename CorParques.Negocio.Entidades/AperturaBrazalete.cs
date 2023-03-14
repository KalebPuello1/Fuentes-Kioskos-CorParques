using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_AperturaBrazalete")]
    public class AperturaBrazalete
    {
        [Key, Column("IdAperturaBrazalete")]
        public int Id { get; set; }

        [Column("IdBrazalete")]
        public int IdBrazalete { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("Observacion")]
        public string Observacion { get; set; }

        [Column("IdUsuarioCreado")]
        public int idUsuarioCreacion { get; set; }

        [Column("IdSupervisor")]
        public int IdSupervisor { get; set; }

        [Column("CantidadInicial")]
        public int CantidadInicial { get; set; } 

        [Column("CantidadFinal")]
        public int CantidadFinal { get; set; }

        [Column("EsReabastecimiento")]
        public bool EsReabastecimiento { get; set; }

        public AperturaBrazaleteDetalle BrazaleteDetalle { get; set; }        
        [Editable(false)]
        public string CodigoSap { get; set; }
        [Editable(false)]
        public string Unidad { get; set; }
    }
}
