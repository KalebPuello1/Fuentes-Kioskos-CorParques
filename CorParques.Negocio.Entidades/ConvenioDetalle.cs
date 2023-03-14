using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_ConvenioDetalle")]
    public class ConvenioDetalle
    {
        [Key]
        [Column("IdConvenioDetalle")]
        public int IdConvenioDetalle { get; set; }

        [Editable(false)]
        [Column("CodSapConvenio")]
        public string CodSapConvenio { get; set; }


        [Column("CodSapProducto")]
        public string CodSapProducto { get; set; }

        [Column("CodSapTipoProducto")]
        public string CodSapTipoProducto { get; set; }

        [Column("Valor")]
        public decimal Valor { get; set; }

        [Column("Cantidad")]
        public int? Cantidad { get; set; }

        [Column("CantidadxDia")]
        public int? CantidadxDia { get; set; }

        [Column("CantidadDisponible")]
        public int? CantidadDisponible { get; set; }

        //RDSH: Se agregan propiedades.
        [Column("IdUsuarioCreacion")]
        public int? IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Editable(false)]
        public int CantidadInicial { get; set; }

        //[Editable(false)]
        //public string TipoProducto { get; set; }

        //[Editable(false)]
        //public string Producto { get; set; }

        //EDSP 27/12/2017
        //Description: Se adiciona propiedad para obtener el porcentaje de descuento
        [Editable(false)]
        public float Porcentaje { get; set; } 

        [Column("IdConvenio")]
        public int IdConvenio { get; set; }
        //FIN EDSP 27/12/2017

    }
}


