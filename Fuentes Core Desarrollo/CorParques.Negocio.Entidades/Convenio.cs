using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Convenio")]
    public class Convenio
    {
        [Key]
        [Column("IdConvenio")]
        public int IdConvenio { get; set; }

        [Editable(false)]
        [Column("CodSapConvenio")]
        public string CodSapConvenio { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("FechaInicial")]
        public DateTime FechaInicial { get; set; }

        [Column("FechaFinal")]
        public DateTime FechaFinal { get; set; }

        [Column("EsCodigoBarras")]
        public bool EsCodigoBarras { get; set; }

        [Column("CodSapPedido")]
        public string CodSapPedido { get; set; }

        [Editable(false)]
        public ConvenioDetalle Detalle { get; set; }

        //RDSH: Se agregan propiedades.
        [Column("ReutilizaCodigoBarras")]
        public bool ReutilizaCodigoBarras { get; set; }
        [Column("EsActivo")]
        public bool EsActivo { get; set; }

        [Column("IdUsuarioCreacion")]
        public int? IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Editable(false)]   
        public string FechaInicialString { get; set; }

        [Editable(false)]
        public string FechaFinalString { get; set; }

        public IEnumerable<TipoGeneral> ListaTipoProducto { get; set; }

        public IEnumerable<ListaProducto> ListaProducto { get; set; }

        //Para detalle producto

        [Editable(false)]
        public string CodSapTipoProducto { get; set; }

        [Editable(false)]
        public string CodSapProducto { get; set; }
        
        [Editable(false)]
        public int Valor { get; set; }

        [Editable(false)]
        public int Cantidad { get; set; }

        [Editable(false)]
        public int CantidadPorTransaccion { get; set; }

        public IEnumerable<ConvenioDetalle> ListaDetalle { get; set; }

    }

    public class ListaProducto
    {
        public string CodSapTipoProducto { get; set; }
        public string CodSapProducto { get; set; }
        public string Nombre { get; set; }        
        //EDSP 27/12/2017 
        public double Precio { get; set; }
    }
}


