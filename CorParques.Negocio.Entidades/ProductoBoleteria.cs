using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    class ProductoBoleteria
    {
        

            #region Propiedades

            [Key]
            [Column("IdBoleteria")]
            public int IdBoleteria { get; set; }
            [Column("IdProducto")]
            public int IdProducto { get; set; }
            [Column("Consecutivo")]
            public string Consecutivo { get; set; }
            [Column("IdSolicitudBoleteria")]
            public int? IdSolicitudBoleteria { get; set; }
            [Column("IdEstado")]
            public int IdEstado { get; set; }
            [Column("Valor")]
            public long? Valor { get; set; }
            [Column("CodigoSapConvenio")]
            public string CodigoSapConvenio { get; set; }
            [Column("CodigoVenta")]
            public string CodigoVenta { get; set; }
            [Column("FechaImpresion")]
            public DateTime FechaImpresion { get; set; }
            [Column("FechaUsoInicial")]
            public DateTime FechaUsoInicial { get; set; }
            [Column("FechaUsoFinal")]
            public DateTime FechaUsoFinal { get; set; }
            [Column("FechaInicioEvento")]
            public DateTime FechaInicioEvento { get; set; }
            [Column("FechaFinEvento")]
            public DateTime FechaFinEvento { get; set; }
            [Column("IdUsuarioCreacion")]
            public int IdUsuarioCreacion { get; set; }
            [Column("Saldo")]
            public long? Saldo { get; set; }
            [Column("IdUsuarioModificacion")]
            public int? IdUsuarioModificacion { get; set; }
            [Column("FechaModificacion")]
            public DateTime? FechaModificacion { get; set; }
            /// <summary>
            /// RDSH: Se agrega esta propiedad para poder usar bono regalo como medio de pago.
            /// </summary>
            [Column("EsMedioPago")]
            public bool EsMedioPago { get; set; }


            [Editable(false)]
            public string NombreProducto { get; set; }

            //EDSP bloqueo de tarjeta
            [Column("IdUsuarioBloqueo")]
            public int? IdUsuarioBloqueo { get; set; }
            [Column("FechaBloqueo")]
            public DateTime? FechaBloqueo { get; set; }
            [Column("PuntoBloqueo")]
            public int? PuntoBloqueo { get; set; }

    //FIN 




    #endregion


}
}
