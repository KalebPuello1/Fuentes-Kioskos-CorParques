using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_SolicitudBoleteria")]
    public class SolicitudBoleteria
    {
        #region Propiedades

        [Column("IdSolicitudBoleteria"), Key]
        public int IdSolicitudBoleteria { get; set; }

        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("FechaEntregaMaterial")]
        public DateTime? FechaEntregaMaterial { get; set; }

        [Column("FechaUsoInicial")]
        public DateTime FechaUsoInicial { get; set; }

        [Column("FechaUsoFinal")]
        public DateTime FechaUsoFinal { get; set; }

        [Column("FechaInicioEvento")]
        public DateTime FechaInicioEvento { get; set; }

        [Column("FechaFinEvento")]
        public DateTime FechaFinEvento { get; set; }

        [Column("FechaVenta")]
        public DateTime? FechaVenta { get; set; }

        [Column("CodigoVenta")]
        public string CodigoVenta { get; set; }

        [Column("IdEstadoMaterial")]
        public int IdEstadoMaterial { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("Observaciones")]
        public string Observaciones { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }

        [Column("Valor")]
        public long? Valor { get; set; }

        [Column("Vendedor")]
        public string Vendedor { get; set; }

        [Column("Vencido")]
        public bool Vencido { get; set; }

        //Propiedes String
        [Editable(false)]
        public string Cliente { get; set; }

        [Editable(false)]
        public string strvalor { get; set; }

        [Editable(false)]
        public string FechaEntregaMaterialDDMMYYY { get; set; }

        [Editable(false)]
        public string FechaUsoInicialDDMMYYY { get; set; }

        [Editable(false)]
        public string FechaUsoFinalDDMMYYY { get; set; }

        [Editable(false)]
        public string FechaInicioEventoDDMMYYY { get; set; }

        [Editable(false)]
        public string FechaFinEventoDDMMYYY { get; set; }

        [Editable(false)]
        public string FechaVentaDDMMYYY { get; set; }

        [Editable(false)]
        public string NombreTipoProducto { get; set; }
        [Editable(false)]
        public string NombreProducto { get; set; }
        [Editable(false)]
        public string NombreEstado { get; set; }

        [Editable(false)]
        public string Consecutivo { get; set; }

        [Editable(false)]
        public int IdEstadoBoleta { get; set; }

        [Editable(false)]
        public bool EsBoletaControl { get; set; }

        [Editable(false)]
        public bool Procesar { get; set; }

        #endregion Propiedades

        #region Constructor 
        #endregion Constructor

        #region Métodos
        #endregion Métodos

    }
}
