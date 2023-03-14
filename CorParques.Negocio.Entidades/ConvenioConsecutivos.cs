using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_ConvenioConsecutivos")]
    public class ConvenioConsecutivos
    {
        [Key]
        [Column("IdConvenioConsecutivos")]
        public int IdConvenioConsecutivos { get; set; }

        [Editable(false)]
        [Column("CodSapConvenio")]
        public string CodSapConvenio { get; set; }

        [Column("Consecutivo")]
        public string Consecutivo { get; set; }

        [Column("FechasEspeciales")]
        public string FechasEspeciales { get; set; }

        [Editable(false)]
        [Column("CodSapProductos")]
        public string CodSapProductos { get; set; }

        //Nuevos parametros 30/10/2017
        [Column("FechaInicio")]
        public DateTime? FechaInicial { get; set; }
        [Column("FechaFin")]
        public DateTime? FechaFinal { get; set; }

        //EDSP nuevo propiedad
        [Column("IdConvenio")]
        public int IdConvenio { get; set; }
    }
}
